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
using System.Text;

namespace NuxeoClient
{
    /// <summary>
    /// Represents the Athorization credentials to be sent on every request to
    /// the nuxeo server.
    /// </summary>
    public class Authorization
    {
        /// <summary>
        /// The supported authorization methods.
        /// </summary>
        public enum Methods
        {
            Basic/*,
            Proxy,
            Portal*/
        }

        /// <summary>
        /// Gets the authorization method.
        /// </summary>
        public Methods Method { get; protected set; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string Username { get; protected set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Authorization"/>.
        /// </summary>
        /// <param name="method">The authorization method.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        private Authorization(Methods method, string username, string password)
        {
            Method = method;
            Username = username;
            Password = password;
        }

        /// <summary>
        /// Initializes a new instance of basic Authorization.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public Authorization(string username, string password) : this(Methods.Basic, username, password)
        {
        }

        /// <summary>
        /// Intializes a new instance of basic Authorization with the default
        /// credentials of the Nuxeo server (Administrator // Administrator).
        /// </summary>
        public Authorization() : this("Administrator", "Administrator")
        {
        }

        /// <summary>
        /// Generates a string with the contents for the HTTP authorization header.
        /// </summary>
        /// <returns>A string with the authorization header's content.</returns>
        public string GenerateAuthorizationParameter()
        {
            switch (Method)
            {
                case Methods.Basic:
                    string unencodedUsernameAndPassword = string.Format("{0}:{1}", Username, Password);
                    byte[] unencodedBytes = UTF8Encoding.UTF8.GetBytes(unencodedUsernameAndPassword);
                    string base64UsernameAndPassword = Convert.ToBase64String(unencodedBytes);
                    return string.Format("Basic {0}", base64UsernameAndPassword);
                /*case Methods.Portal:
                    return string.Empty;

                case Methods.Proxy:
                    return string.Empty;*/
                default:
                    return string.Empty;
            }
        }
    }
}