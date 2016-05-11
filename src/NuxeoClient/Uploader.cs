/*
 * (C) Copyright 2015-2016 Nuxeo SA (http://nuxeo.com/) and others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Contributors:
 *     Gabriel Barata <gbarata@nuxeo.com>
 */

using NuxeoClient.Wrappers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
        private int processedFilesCounter = 0;
        private object syncCounter = new object();
        private SemaphoreSlim semaphore;

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
            semaphore = new SemaphoreSlim(NumConcurrentUploads);
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
        /// Sets the number of maximum concurent uploads.
        /// </summary>
        /// <param name="num">The number of concurrent uploads.</param>
        /// <returns>The current <see cref="Uploader"/> instance.</returns>
        public Uploader SetNumConcurrentUploads(int num)
        {
            NumConcurrentUploads = num;
            semaphore = new SemaphoreSlim(NumConcurrentUploads);
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
        /// <returns>A <see cref="Task"/> that will will return an instance of <see cref="EntityList{T}"/> containing
        /// one instance of <see cref="BatchFile"/> per file uploaded.</returns>
        public async Task<Entity> UploadFiles()
        {
            processedFilesCounter = 0;
            try
            {
                // perform handshake if needed
                Batch = Batch ?? await client.Batch();
            }
            catch (ServerErrorException exception)
            {
                throw new FailedHandshakeException("Failed to initialize batch with the server.", exception);
            }

            foreach (string path in filesToUpload)
            {
                await ProcessFile(path);
            }
            return await Batch.Info();
        }

        private async Task<Batch> ProcessFile(string path)
        {
            Batch batch = null;
            await semaphore.WaitAsync();
            try
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

                UploadJob job = new UploadJob(Blob.FromFile(path));
                job.SetFileId(i);
                job.SetChunked(IsChunkedUpload);
                job.SetChunkSize(ChunkSize);
                batch = await UploadBlob(job);
            }
            finally
            {
                semaphore.Release();
            }
            return batch;
        }

        private async Task<Batch> UploadBlob(UploadJob job)
        {
            try
            {
                return await Batch.Upload(job);
            }
            catch (ServerErrorException exception)
            {
                throw new FailedToUploadException(job.ToString(), exception);
            }
        }
    }
}