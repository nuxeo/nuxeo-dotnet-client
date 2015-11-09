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