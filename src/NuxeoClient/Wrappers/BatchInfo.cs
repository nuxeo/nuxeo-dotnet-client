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
using System.ComponentModel;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// This class provides information about a batch.
    /// </summary>
    public class BatchInfo : Entity
    {
        /// <summary>
        /// Gets the bach ID.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "batchId")]
        public string BatchId { get; set; } = string.Empty;

        /// <summary>
        /// Gets the file index.
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "fileIdx")]
        public int FileIndex { get; set; } = 0;

        /// <summary>
        /// Gets the type of upload.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "uploadType")]
        public string UploadType { get; set; } = string.Empty;

        /// <summary>
        /// Gets the size of the upload.
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "uploadedSize")]
        public int UploadSize { get; set; } = 0;

        /// <summary>
        /// Gets whether the batch was dropped or not.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "dropped")]
        public bool Dropped { get; set; } = false;

        /// <summary>
        /// Gets the id of the chunk.
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "uploadedChunkId")]
        public int UploadedChunkId { get; set; } = 0;

        /// <summary>
        /// Gets the total amount of chunks.
        /// </summary>
        [DefaultValue(1)]
        [JsonProperty(PropertyName = "chunkCount")]
        public int ChunkCount { get; set; } = 1;

        /// <summary>
        /// Initializes a new instance of <see cref="BatchInfo"/>.
        /// </summary>
        public BatchInfo()
        { }
    }
}