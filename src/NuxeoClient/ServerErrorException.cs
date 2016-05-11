/*
 * (C) Copyright 2016 Nuxeo SA (http://nuxeo.com/) and others.
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
    /// The exception that is thrown when an instance of the Nuxeo
    /// server throws a server error, with status code between 500 and 599.
    /// </summary>
    public class ServerErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ServerErrorException"/>
        /// with an empty message.
        /// </summary>
        public HttpStatusCode StatusCode { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ServerErrorException"/>
        ///  with its response status code set to <paramref name="statusCode"/>.
        /// </summary>
        /// <param name="statusCode">The server's response status code.</param>
        public ServerErrorException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServerErrorException"/> with
        /// its response /// status code set to <paramref name="statusCode"/> and
        /// its message set to <paramref name="message"/>.
        /// </summary>
        /// <param name="statusCode">The server's response status code.</param>
        /// <param name="message">The exception message.</param>
        public ServerErrorException(HttpStatusCode statusCode, string message)
        : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServerErrorException"/> with
        /// its response status code set to <paramref name="statusCode"/>, its
        /// message set to <paramref name="message"/>, and with its inner exception
        /// set to <paramref name="inner"/>.
        /// </summary>
        /// <param name="statusCode">The server's response status code.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="inner">The inner excepiton.</param>
        public ServerErrorException(HttpStatusCode statusCode, string message, Exception inner)
        : base(message, inner)
        {
            StatusCode = statusCode;
        }
    }
}