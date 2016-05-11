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

using System.IO;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents a BLOB enclosing a file.
    /// </summary>
    public class Blob : Entity
    {
        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// Gets the <see cref="FileInfo"/> of the file.
        /// </summary>
        public FileInfo File { get; private set; }

        /// <summary>
        /// Gets the file's mime type.
        /// </summary>
        public string MimeType { get; private set; }

        /// <summary>
        /// Gets whether or not this blob is a chunk or the whole file.
        /// </summary>
        public bool IsChunk { get; private set; } = false;

        /// <summary>
        /// Initializes a new instance of <see cref="Blob"/>.
        /// </summary>
        /// <param name="filename">The name of the file it represents.</param>
        public Blob(string filename) :
            this(filename, null)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="Blob"/>.
        /// </summary>
        /// <param name="file">The file's <see cref="FileInfo"/>.</param>
        public Blob(FileInfo file) :
            this(file.Name, file)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="Blob"/>.
        /// </summary>
        /// <param name="filename">The name of the file it represents.</param>
        /// <param name="file">The file's <see cref="FileInfo"/>.</param>
        public Blob(string filename, FileInfo file) :
            this(filename, file, MimeTypeMap.GetMimeType(Path.GetExtension(filename)))
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="Blob"/>.
        /// </summary>
        /// <param name="filename">The name of the file it represents.</param>
        /// <param name="file">The file's <see cref="FileInfo"/>.</param>
        /// <param name="mime">The file's mime type.</param>
        public Blob(string filename, FileInfo file, string mime)
        {
            Filename = filename;
            File = file;
            MimeType = mime;
        }

        /// <summary>
        /// Sets the name of the file represented by the blob.
        /// </summary>
        /// <param name="filename">The file's name.</param>
        /// <returns>The current <see cref="Blob"/> instance.</returns>
        public Blob SetFilename(string filename)
        {
            Filename = filename;
            return this;
        }

        /// <summary>
        /// Sets the file object.
        /// </summary>
        /// <param name="file">The file's <see cref="FileInfo"/>.</param>
        /// <returns>The current <see cref="Blob"/> instance.</returns>
        public Blob SetFile(FileInfo file)
        {
            File = file;
            return this;
        }

        /// <summary>
        /// Set's the file's mime type.
        /// </summary>
        /// <param name="mime">The file's mime type.</param>
        /// <returns>The current <see cref="Blob"/> instance.</returns>
        public Blob SetMimeType(string mime)
        {
            MimeType = mime;
            return this;
        }

        /// <summary>
        /// Creates a new <see cref="Blob"/> instance from a path to a file.
        /// </summary>
        /// <param name="path">The path to a file</param>
        /// <returns>A new <see cref="Blob"/> instance.</returns>
        public static Blob FromFile(string path)
        {
            return new Blob(Path.GetFileName(path),
                            new FileInfo(path),
                            MimeTypeMap.GetMimeType(Path.GetExtension(path)));
        }
    }
}