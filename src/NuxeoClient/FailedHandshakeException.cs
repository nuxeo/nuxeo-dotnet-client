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
    /// The exception that is thrown when the handshake between the Nuxeo
    /// client and the server fails. The handshake is performed whenever a
    /// new batch is requested from the server.
    /// </summary>
    public class FailedHandshakeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="FailedHandshakeException"/> with
        /// an empty message.
        /// </summary>
        public FailedHandshakeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FailedHandshakeException"/> with
        /// its message string set to <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public FailedHandshakeException(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FailedHandshakeException"/> with
        /// its message string set to <paramref name="message"/> and with its inner
        /// exceptio set to <paramref name="inner"/>
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="inner">The inner excepiton.</param>
        public FailedHandshakeException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}