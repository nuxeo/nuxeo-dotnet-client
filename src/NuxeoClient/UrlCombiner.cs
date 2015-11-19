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
    /// A simplete URL segment combiner.
    /// </summary>
    public static class UrlCombiner
    {
        /// <summary>
        /// Combines URL segments.
        /// </summary>
        /// <param name="toCombine">The segments to combine.</param>
        /// <returns>The combined URL.</returns>
        public static string Combine(params string[] toCombine)
        {
            string url = string.Empty;
            string token;
            foreach (string segment in toCombine)
            {
                token = segment.Trim();
                if (string.IsNullOrEmpty(token))
                {
                    continue;
                }
                
                if (!string.IsNullOrEmpty(url) &&
                    url.EndsWith("/") == false)
                {
                    url += "/";
                }

                int i = (token.StartsWith("/") ? 1 : 0);
                int j = token.Length - (token.EndsWith("/") ? 1 : 0);

                if (j > 0)
                {
                    url += token.Substring(i, j - i);
                }
            }
            return url;
        }
    }
}
