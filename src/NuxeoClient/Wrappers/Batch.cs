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

using Newtonsoft.Json;
using System.Collections.Generic;
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
        /// Executes a non-chnked <see cref="UploadJob"/>.
        /// </summary>
        /// <param name="job">The <see cref="UploadJob"/> to be executed.</param>
        /// <returns>A new and updated <see cref="Batch"/> instance of the current batch.</returns>
        public async Task<Batch> Upload(UploadJob job)
        {
            return (Batch)await client.PostBin(UrlCombiner.Combine(Endpoint, job.FileId.ToString()),
                                                null,
                                                job.Blob.Content,
                                                new Dictionary<string, string>() {
                                                    { "X-File-Name", job.Blob.Filename },
                                                    { "X-File-Type", job.Blob.MimeType }
                                                });
        }

        /// <summary>
        /// Executes a chunked <see cref="UploadJob"/>.
        /// </summary>
        /// <param name="job">The <see cref="UploadJob"/> to be executed.</param>
        /// <returns>A new and updated <see cref="Batch"/> instance of the current batch.</returns>
        public async Task<Batch> UploadChunk(UploadJob job)
        {
            return (Batch)await client.PostBin(UrlCombiner.Combine(Endpoint, job.FileId.ToString()),
                                            null,
                                            job.Blob.Content,
                                            new Dictionary<string, string>() {
                                                { "X-Upload-Type", "chunked" },
                                                { "X-Upload-Chunk-Index", job.ChunkIndex.ToString() },
                                                { "X-Upload-Chunk-Count", job.ChunkCount.ToString() },
                                                { "X-File-Name", job.Blob.Filename },
                                                { "X-File-Type", job.Blob.MimeType },
                                                { "X-File-Size", job.Blob.Content.Length.ToString() }
                                            });
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
        /// <returns>A <see cref="EntityList"/> containing an instance of <see cref="BatchFile"/> per uploaded file.</returns>
        public async Task<EntityList> Info()
        {
            return (EntityList)await client.Get(Endpoint);
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