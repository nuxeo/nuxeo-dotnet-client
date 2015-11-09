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
    /// The exception that is thrown when an instance of the Nuxeo
    /// server throws an exception.
    /// </summary>
    public class ServerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ServerException"/>
        /// with an empty message.
        /// </summary>
        public HttpStatusCode StatusCode { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ServerException"/>
        ///  with its response status code set to <paramref name="statusCode"/>.
        /// </summary>
        /// <param name="statusCode">The server's response status code.</param>
        public ServerException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServerException"/> with
        /// its response /// status code set to <paramref name="statusCode"/> and
        /// its message set to <paramref name="message"/>.
        /// </summary>
        /// <param name="statusCode">The server's response status code.</param>
        /// <param name="message">The exception message.</param>
        public ServerException(HttpStatusCode statusCode, string message)
        : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServerException"/> with
        /// its response status code set to <paramref name="statusCode"/>, its
        /// message set to <paramref name="message"/>, and with its inner exception
        /// set to <paramref name="inner"/>.
        /// </summary>
        /// <param name="statusCode">The server's response status code.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="inner">The inner excepiton.</param>
        public ServerException(HttpStatusCode statusCode, string message, Exception inner)
        : base(message, inner)
        {
            StatusCode = statusCode;
        }
    }
}