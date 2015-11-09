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
    /// Encloses information about a file that was uploaded into a batch.
    /// </summary>
    public class BatchFile : Entity
    {
        /// <summary>
        /// Gets the file name.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets the file size.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; } = string.Empty;

        /// <summary>
        /// Gets the type of upload.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "uploadType")]
        public string UploadType { get; set; } = string.Empty;

        /// <summary>
        /// Gets the IDs of the uploaded chunks.
        /// </summary>
        [DefaultValue(new int[0])]
        [JsonProperty(PropertyName = "uploadedChunkIds")]
        public int[] UploadedChunkIds { get; set; } = new int[0];

        /// <summary>
        /// Gets the number of uploaded chunks.
        /// </summary>
        [DefaultValue(1)]
        [JsonProperty(PropertyName = "chunkCount")]
        public int ChunkCount { get; set; } = 1;

        /// <summary>
        /// Initializes a new instance of <see cref="BatchFile"/>.
        /// </summary>
        internal BatchFile()
        { }
    }
}