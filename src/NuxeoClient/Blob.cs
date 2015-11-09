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

using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace NuxeoClient
{
    /// <summary>
    /// Represents a blob, that might enclose a whole file or a chunk.
    /// </summary>
    public class Blob
    {
        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// Gets the blob's content.
        /// </summary>
        public byte[] Content { get; private set; }

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
            this(filename, new byte[0])
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="Blob"/>.
        /// </summary>
        /// <param name="filename">The name of the file it represents.</param>
        /// <param name="content">The blob's content.</param>
        public Blob(string filename, byte[] content) :
            this(filename, content, MimeTypeMap.GetMimeType(Path.GetExtension(filename)))
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="Blob"/>.
        /// </summary>
        /// <param name="filename">The name of the file it represents.</param>
        /// <param name="content">The blob's content.</param>
        /// <param name="mime">The file's mime type.</param>
        public Blob(string filename, byte[] content, string mime)
        {
            Filename = filename;
            Content = content;
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
        /// Sets the blob's content.
        /// </summary>
        /// <param name="content">The blob's content.</param>
        /// <returns>The current <see cref="Blob"/> instance.</returns>
        public Blob SetContent(byte[] content)
        {
            Content = content;
            return this;
        }

        /// <summary>
        /// Sets the blob's content.
        /// </summary>
        /// <param name="content">The blob's content.</param>
        /// <returns>The current <see cref="Blob"/> instance.</returns>
        public Blob SetContent(string content)
        {
            Content = content.Select(c => (byte)c).ToArray();
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
        /// Splits a blob into <paramref name="numChunks"/> chunks.
        /// </summary>
        /// <param name="numChunks">The number of chunks to split the blob into.</param>
        /// <returns>An array with <paramref name="numChunks"/> chunk blobs.</returns>
        public Blob[] Split(int numChunks)
        {
            Blob[] result = new Blob[numChunks];
            int parts = (int)Math.Ceiling(Content.Length / (double)numChunks);
            for (int i = 0; i < numChunks; i++)
            {
                result[i] = new Blob(Filename,
                                     Content.SubArray<byte>(i * parts, (int)Math.Min(parts, Content.Length - (i * parts))),
                                     MimeType);
                result[i].IsChunk = true;
            }
            return result;
        }

        /// <summary>
        /// Creates and returns a <see cref="JObject"/> representation of the current object.
        /// </summary>
        /// <returns>A <see cref="JObject"/> representation of the current object.</returns>
        public JObject ToJObject()
        {
            return new JObject
            {
                {"filename", Filename },
                {"content", Content },
                {"mimetype", MimeType }
            };
        }

        /// <summary>
        /// Creates and returns a new instance of <see cref="Blob"/> build from an instance of <see cref="JObject"/>.
        /// </summary>
        /// <param name="obj">The base <see cref="JObject"/> object.</param>
        /// <returns>A new instance of <see cref="Blob"/>.</returns>
        public static Blob FromJObject(JObject obj)
        {
            if (obj["filename"] != null &&
                obj["content"] != null &&
                obj["mimetype"] != null)
            {
                return new Blob(obj["filename"].ToObject<string>(),
                                obj["content"].ToObject<byte[]>(),
                                obj["mimetype"].ToObject<string>());
            }
            else
            {
                return null;
            }
        }
    }
}