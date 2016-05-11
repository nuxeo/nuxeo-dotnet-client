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

namespace NuxeoClient.Adapters
{
    /// <summary>
    /// Represents a WebAdapter, which transforms an input resource in another resource.
    /// An <see cref="Adapter"/> is applied to an NxEntity, and a different NxEntity is
    /// returned when a request is made to the server.
    /// </summary>
    /// <remarks>For more details about Adapters, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC/Web+Adapters+for+the+REST+API">Nuxeo Documentation Page</a>.</remarks>
    public class Adapter
    {
        /// <summary>
        /// Gets the Adapter's ID.
        /// </summary>
        public String Id { get; protected set; }

        /// <summary>
        /// Gets the adapter parameters to be sent on the request.
        /// </summary>
        protected String Parameters { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of <see cref="Adapter"/>.
        /// </summary>
        /// <param name="id">The adapter's ID.</param>
        public Adapter(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Returns the endpoint suffix to the current <see cref="Adapter"/>, to be appended to other requests.
        /// </summary>
        /// <returns>A string with the endpoint suffix.</returns>
        public string GetEndpointSuffix()
        {
            return "@" + UrlCombiner.Combine(Id, (Parameters != string.Empty ? (Parameters[0] != '?' ? "/" : string.Empty) + Parameters : string.Empty));
        }
    }
}