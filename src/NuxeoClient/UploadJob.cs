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

namespace NuxeoClient
{
    /// <summary>
    /// Represents an upload job, used either by the <see cref="Uploader"/>
    /// and the <see cref="Batch"/> classes. And upload job might refer to
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
        /// Gets the index of the chunk to upload.
        /// </summary>
        public int ChunkIndex { get; protected set; } = 0;

        /// <summary>
        /// Gets the total amount of chunks to upload;
        /// </summary>
        public int ChunkCount { get; protected set; } = 1;

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
        /// Sets the index of the chunk to upload.
        /// </summary>
        /// <param name="chunkIndex"></param>
        /// <returns>The current <see cref="UploadJob"/> instance.</returns>
        public UploadJob SetChunkIndex(int chunkIndex)
        {
            ChunkIndex = chunkIndex;
            return this;
        }

        /// <summary>
        /// Sets the number of total chunks to upload.
        /// </summary>
        /// <param name="chunkCount"></param>
        /// <returns>The current <see cref="UploadJob"/> instance.</returns>
        public UploadJob SetChunkCount(int chunkCount)
        {
            ChunkCount = chunkCount;
            return this;
        }

        /// <summary>
        /// Creates and returns a string representation of the current <see cref="UploadJob"/> object.
        /// </summary>
        /// <returns>A string representation of the current <see cref="UploadJob"/> object.</returns>
        public override string ToString()
        {
            return "Filename: " + Blob.Filename + ", FileId: " + FileId + ", ChunkIndex: " + ChunkIndex + ", ChunkCount: " + ChunkCount;
        }
    }
}