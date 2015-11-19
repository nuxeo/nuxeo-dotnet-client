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

using System;

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
    }
}