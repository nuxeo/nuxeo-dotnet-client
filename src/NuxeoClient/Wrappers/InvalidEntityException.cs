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

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// The exception that is thrown when a JSON object does
    /// not represent a valid <see cref="Entity"/> instance.
    /// </summary>
    public class InvalidEntityException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="InvalidEntityException"/> with
        /// an empty message.
        /// </summary>
        public InvalidEntityException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="InvalidEntityException"/> with
        /// its message string set to <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public InvalidEntityException(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="InvalidEntityException"/> with
        /// its message string set to <paramref name="message"/> and with its inner
        /// exceptio set to <paramref name="inner"/>
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="inner">The inner excepiton.</param>
        public InvalidEntityException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}