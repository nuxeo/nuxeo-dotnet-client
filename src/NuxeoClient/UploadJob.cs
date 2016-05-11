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

namespace NuxeoClient
{
    /// <summary>
    /// Represents an upload job, used either by the <see cref="Uploader"/>
    /// and the <see cref="Wrappers.Batch"/> classes. And upload job might refer to
    /// the upload of a whole file or a chunk of a file.
    /// </summary>
    public class UploadJob
    {
        /// <summary>
        /// Gets the <see cref="Blob"/> to upload.
        /// </summary>
        public Blob Blob { get; protected set; }

        /// <summary>
        /// Gets the id of the file to upload.
        /// </summary>
        public int FileId { get; protected set; } = 0;

        /// <summary>
        /// Gets whether the upload should be performed in chunks or not.
        /// </summary>
        public bool IsChunked { get; private set; } = false;

        /// <summary>
        /// Gets the chunk size in bytes.
        /// </summary>
        public int ChunkSize { get; private set; } = 1024;

        /// <summary>
        /// Creates a new <see cref="UploadJob"/> for the supplied <paramref name="blob"/>.
        /// </summary>
        /// <param name="blob">The <see cref="Blob"/> to be uploaded.</param>
        public UploadJob(Blob blob)
        {
            Blob = blob;
        }

        /// <summary>
        /// Sets the id of the file to upload.
        /// </summary>
        /// <param name="fileId">The id of the file.</param>
        /// <returns>The current <see cref="UploadJob"/> instance.</returns>
        public UploadJob SetFileId(int fileId)
        {
            FileId = fileId;
            return this;
        }

        /// <summary>
        /// Sets whether the file should be uploaded in chunks or not.
        /// </summary>
        /// <param name="isChunked">A boolean that should be <c>true</c> if the upload is to be in chunks, or <c>false</c> otherwise.</param>
        /// <returns>The current <see cref="UploadJob"/> instance.</returns>
        public UploadJob SetChunked(bool isChunked)
        {
            IsChunked = isChunked;
            return this;
        }

        /// <summary>
        /// Sets the chunk size.
        /// </summary>
        /// <param name="size">Chunk size in bytes.</param>
        /// <returns>The current <see cref="UploadJob"/> instance.</returns>
        public UploadJob SetChunkSize(int size)
        {
            ChunkSize = size;
            return this;
        }

        /// <summary>
        /// Creates and returns a string representation of the current <see cref="UploadJob"/> object.
        /// </summary>
        /// <returns>A string representation of the current <see cref="UploadJob"/> object.</returns>
        public override string ToString()
        {
            return "Filename: " + Blob.Filename + ", FileId: " + FileId;
        }
    }
}