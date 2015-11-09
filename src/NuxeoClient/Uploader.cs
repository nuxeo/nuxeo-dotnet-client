/*
 * (C) Copyright 2015 Nuxeo SA (http://nuxeo.com/) and others.
 *
 * All rights reserved. This program and the accompanying materials
 * are made available under the terms of the GNU Lesser General Public License
 * (LGPL) version 2.1 which accompanies this distribution, and is available at
 * http://www.gnu.org/licenses/lgpl-2.1.html
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * Contributors:
 *     Gabriel Barata <gbarata@nuxeo.com>
 */

using NuxeoClient.Wrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NuxeoClient
{
    /// <summary>
    /// Provides a cass for uploading files to a Nuxeo server.
    /// Files can be uploaded as a whole or in several chunks.
    /// </summary>
    /// <remarks>No server-side error handling was implemented.
    /// If an upload job fails, a <see cref="FailedToUploadException"/> is thrown.</remarks>
    public class Uploader
    {
        private Client client;
        private Queue<string> filesToUpload = new Queue<string>();
        private object syncFiles = new object();
        private int processedFilesCounter = 0;
        private object syncCounter = new object();

        /// <summary>
        /// Gets the batch to upload.
        /// </summary>
        public Batch Batch { get; private set; } = null;

        /// <summary>
        /// Gets the number of maximum concurrent uploads.
        /// </summary>
        public int NumConcurrentUploads { get; private set; } = 5;

        /// <summary>
        /// Gets whether uploads should be perform in chunks or not.
        /// </summary>
        public bool IsChunkedUpload { get; private set; } = false;

        /// <summary>
        /// Gets the chunk size in bytes.
        /// </summary>
        public int ChunkSize { get; private set; } = 1024;

        /// <summary>
        /// Initializes a new instance of <see cref="Uploader"/>.
        /// </summary>
        /// <param name="client">The Nuxeo <see cref="Client"/> throw which the upload requests will be made.</param>
        public Uploader(Client client)
        {
            this.client = client;
        }

        /// <summary>
        /// Sets whether files should be uploaded in chunks or not.
        /// </summary>
        /// <param name="isChunkedUpload">A boolean that should be <c>true</c> if the upload is to be in chunks, or <c>false</c> otherwise.</param>
        /// <returns>The current <see cref="Uploader"/> instance.</returns>
        public Uploader SetChunked(bool isChunkedUpload)
        {
            IsChunkedUpload = isChunkedUpload;
            return this;
        }

        /// <summary>
        /// Sets the chunk size.
        /// </summary>
        /// <param name="size">Chunk size in bytes.</param>
        /// <returns>The current <see cref="Uploader"/> instance.</returns>
        public Uploader SetChunkSize(int size)
        {
            ChunkSize = size;
            return this;
        }

        /// <summary>
        /// Adds a file to the upload queue.
        /// </summary>
        /// <param name="file">The path to the file to be uploaded.</param>
        /// <returns>The current <see cref="Uploader"/> instance.</returns>
        public Uploader AddFile(string file)
        {
            filesToUpload.Enqueue(file);
            return this;
        }

        /// <summary>
        /// Adds several files to the upload queue.
        /// </summary>
        /// <param name="files">An array containing the paths to the files to be uploaded.</param>
        /// <returns>The current <see cref="Uploader"/> instance.</returns>
        public Uploader AddFiles(string[] files)
        {
            files?.ToList().ForEach(file => AddFile(file));
            return this;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Operation"/> to be executed on all uploaded
        /// files in the current batch.
        /// </summary>
        /// <param name="operationId">The operation id.</param>
        /// <returns>A new <see cref="Operation"/> instance.</returns>
        public Operation Operation(string operationId)
        {
            return Batch.Operation(operationId);
        }

        /// <summary>
        /// Uploads all files in the upload queue.
        /// </summary>
        /// <returns>A <see cref="Task"/> that will will return an instance of <see cref="EntityList"/> containing
        /// one instance of <see cref="BatchFile"/> per file uploaded.</returns>
        public async Task<Entity> UploadFiles()
        {
            processedFilesCounter = 0;
            try
            {
                // perform handshake if needed
                Batch = Batch ?? client.Batch().Result;
            }
            catch (ServerException exception)
            {
                throw new FailedHandshakeException("Failed to initialize batch with the server.", exception);
            }

            lock (syncFiles)
            {
                Parallel.ForEach(filesToUpload, new ParallelOptions { MaxDegreeOfParallelism = NumConcurrentUploads }, path =>
                {
                    if (!File.Exists(path))
                    {
                        throw new FileNotFoundException(path);
                    }

                    int i;
                    lock (syncCounter)
                    {
                        i = processedFilesCounter++;
                    }

                    if (IsChunkedUpload)
                    {
                        FileStream fs = File.OpenRead(path);
                        int readBytes;
                        byte[] buffer = new byte[ChunkSize];
                        UploadJob job;
                        List<Blob> blobs = new List<Blob>();

                        while ((readBytes = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            byte[] temp = new byte[readBytes];
                            Array.Copy(buffer, temp, readBytes);
                            blobs.Add(new Blob(Path.GetFileName(path)).SetContent(temp));
                            Array.Clear(buffer, 0, ChunkSize);
                        }

                        int chunksProcessed = 0;
                        foreach (Blob blob in blobs)
                        {
                            job = new UploadJob(blob).SetFileId(i)
                                                     .SetChunkIndex(chunksProcessed)
                                                     .SetChunkCount(blobs.Count);
                            UploadBlob(job, true);
                            chunksProcessed++;
                        }
                    }
                    else
                    {
                        byte[] contents = File.ReadAllBytes(path);
                        Blob blob = new Blob(Path.GetFileName(path)).SetContent(contents);
                        UploadJob job = new UploadJob(blob).SetFileId(i);
                        UploadBlob(job);
                    }
                });
            }
            return await Batch.Info();
        }

        private void UploadBlob(UploadJob job, bool isChuncked = false)
        {
            try
            {
                if (isChuncked)
                {
                    Batch.UploadChunk(job).Wait();
                }
                else
                {
                    Batch.Upload(job).Wait();
                }
            }
            catch (ServerException exception)
            {
                throw new FailedToUploadException(job.ToString(), exception);
            }
        }
    }
}