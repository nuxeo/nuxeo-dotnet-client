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

using System;
using System.IO;

namespace NuxeoClient
{
    /// <summary>
    /// Extension methods used by NuxeoClient.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns a subarray of the supplied array.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="data">The input array.</param>
        /// <param name="index">A zero-based starting position of the subarray.</param>
        /// <param name="length">The number of elements to be included in the subarray.</param>
        /// <returns>A a copy of the subarray.</returns>
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Adds a path to an <see cref="UriBuilder"/>.
        /// </summary>
        /// <param name="builder">The instance of <see cref="UriBuilder"/> to which the path should be added.</param>
        /// <param name="pathValue">The path to be added.</param>
        /// <returns>The current <see cref="UriBuilder"/> instance.</returns>
        public static UriBuilder AddPath(this UriBuilder builder, string pathValue)
        {
            if (string.IsNullOrEmpty(pathValue))
            {
                return builder;
            }

            string path = builder.Path;

            if (path.EndsWith("/") == false)
            {
                path = path + "/";
            }

            int i = (pathValue[0] == '/' ? 1 : 0);
            int j = pathValue.Length - (pathValue[pathValue.Length - 1] == '/' ? 1 : 0);
            path += (j > 0 ? pathValue.Substring(i, j-i) : string.Empty);

            builder.Path = path;

            return builder;
        }

        /// <summary>
        /// Reads all data from a stream into a byte array.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to be read.</param>
        /// <returns>A byte array containing the data.</returns>
        public static byte[] ReadToEnd(this Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}