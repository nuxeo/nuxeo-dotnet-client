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
    }
}