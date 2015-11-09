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
using System.Net;

namespace NuxeoClient
{
    /// <summary>
    /// A simple class representing the WebProxy class, which
    /// is not yet implemented on the <a href="https://github.com/dotnet/corefx">dotnet/corefx</a> project.
    /// </summary>
    public class WebProxy : System.Net.IWebProxy
    {
        private readonly Uri proxyUri;

        /// <summary>
        /// Initializes an empty instance of the <see cref="WebProxy"/> class.
        /// </summary>
        /// <param name="proxyUri"></param>
        public WebProxy(Uri proxyUri)
        {
            this.proxyUri = proxyUri;
        }

        /// <summary>
        /// Gets or sets the credentials to submit to the proxy server for authentication.
        /// </summary>
        public ICredentials Credentials { get; set; }

        /// <summary>
        /// Returns the proxied URI for a request.
        /// </summary>
        /// <param name="destination">The <see cref="Uri"/> instance of the requested Internet resource.</param>
        /// <returns>The <see cref="Uri"/> instance of the Internet resource, if the resource is on the bypass
        /// list; otherwise, the Uri instance of the proxy.
        /// </returns>
        public Uri GetProxy(Uri destination)
        {
            return proxyUri;
        }

        /// <summary>
        /// Indicates whether to use the proxy server for the specified host.
        /// </summary>
        /// <param name="host">The <see cref="Uri"/> instance of the host to check for proxy use.</param>
        /// <returns><c>true</c> if the proxy server should not be used for host; otherwise, <c>false</c>.</returns>
        public bool IsBypassed(Uri host)
        {
            return false;
        }
    }
}