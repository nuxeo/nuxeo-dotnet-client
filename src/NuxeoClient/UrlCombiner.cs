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
