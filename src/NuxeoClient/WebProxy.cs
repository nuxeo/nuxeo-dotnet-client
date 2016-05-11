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