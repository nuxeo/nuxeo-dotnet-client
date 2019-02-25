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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents a remote batch in the Nuxeo server, to which one ore more files
    /// can be uploaded.
    /// </summary>
    /// <remarks>For more details about file uploading, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC/How+to+Upload+a+File+in+Nuxeo+Platform+Using+REST+API+Batch+Processing+Endpoint">
    /// How to Upload a File @ Nuxeo Documentation Center</a>.
    ///  For more information about batch upload, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC/Blob+Upload+for+Batch+Processing">
    /// Blob Upload for Batch Processing @ Nuxeo Documentation Center</a>.</remarks>
    public class Batch : BatchInfo
    {
        /// <summary>
        /// Gets the endpoint of the batch.
        /// </summary>
        [JsonIgnore]
        public string Endpoint { get; private set; }

        /// <summary>
        /// Gets the <see cref="Client"/> instace through which operations for this batch will be performed.
        /// </summary>
        protected Client client { get; private set; } = null;

        /// <summary>
        /// Initializes a new instance of <see cref="Batch"/>.
        /// </summary>
        public Batch() { }

        /// <summary>
        /// Sets the Nuxeo <see cref="Client"/> instance through which the requests for this batch will be made.
        /// </summary>
        /// <param name="client">The Nuxeo <see cref="Client"/> instance.</param>
        /// <returns>The current <see cref="Batch"/> instance.</returns>
        public Batch SetClient(Client client)
        {
            this.client = client;
            Endpoint = UrlCombiner.Combine(client.RestPath, "upload/", BatchId);
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
            return new BatchOperation(client, this, operationId)
                        .SetParameter("operationId", operationId)
                        .SetParameter("batchId", BatchId);
        }

        /// <summary>
        /// Executes a an <see cref="UploadJob"/>.
        /// </summary>
        /// <param name="job">The <see cref="UploadJob"/> to be executed.</param>
        /// <returns>A new and updated <see cref="Batch"/> instance of the current batch.</returns>
        public async Task<Batch> Upload(UploadJob job)
        {
            if (job.IsChunked)
            {
                int readBytes, currentChunk = 0, chunkCount = (int)Math.Ceiling((double)job.Blob.File.Length / job.ChunkSize);
                byte[] buffer = new byte[job.ChunkSize];
                Batch batch = null;
                using (FileStream fs = job.Blob.File.OpenRead())
                {
                    while ((readBytes = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        batch = (Batch)await client.PostBin(UrlCombiner.Combine(Endpoint, job.FileId.ToString()),
                                                null,
                                                buffer,
                                                new Dictionary<string, string>() {
                                                { "X-Upload-Type", "chunked" },
                                                { "X-Upload-Chunk-Index", currentChunk.ToString() },
                                                { "X-Upload-Chunk-Count", chunkCount.ToString() },
                                                { "X-File-Name", Uri.EscapeDataString(job.Blob.Filename) },
                                                { "X-File-Type", job.Blob.MimeType },
                                                { "X-File-Size", job.Blob.File.Length.ToString() }
                                                });
                        currentChunk++;
                    }
                }
                return batch;
            }
            else
            {
                using (FileStream fs = job.Blob.File.OpenRead())
                {
                    return (Batch)await client.PostBin(UrlCombiner.Combine(Endpoint, job.FileId.ToString()),
                                                null,
                                                fs.ReadToEnd(),
                                                new Dictionary<string, string>() {
                                                    { "X-File-Name", Uri.EscapeDataString(job.Blob.Filename) },
                                                    { "X-File-Type", job.Blob.MimeType }
                                                });
                }
            }
        }

        /// <summary>
        /// Drops the current <see cref="Batch"/> from the server.
        /// </summary>
        /// <returns>A <see cref="BatchInfo"/> instance contaning information regarding the drop operation.</returns>
        public async Task<BatchInfo> Drop()
        {
            return (BatchInfo)await client.Delete(Endpoint);
        }

        /// <summary>
        /// Requests information regarding all the files uploaded so far.
        /// </summary>
        /// <returns>A <see cref="EntityList{T}"/> containing an instance of <see cref="BatchFile"/> per uploaded file.</returns>
        public async Task<EntityList<Entity>> Info()
        {
            return (EntityList<Entity>)await client.Get(Endpoint);
        }

        /// <summary>
        /// Requests information about a particular uploaded file.
        /// </summary>
        /// <param name="fileIndex">The index of the uploaded file.</param>
        /// <returns>An instance of <see cref="BatchFile"/> containing information about the file.</returns>
        public async Task<BatchFile> Info(int fileIndex)
        {
            return (BatchFile)await client.Get(UrlCombiner.Combine(Endpoint, fileIndex.ToString()));
        }
    }
}