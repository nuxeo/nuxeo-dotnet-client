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
            return "@" + Id + (Parameters != string.Empty ? (Parameters[0] != '?' ? "/" : string.Empty) + Parameters : string.Empty);
        }
    }
}