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
            /// <summary>
            /// Represents the basic authentication type
            /// </summary>
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