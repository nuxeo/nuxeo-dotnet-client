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

using Newtonsoft.Json.Linq;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents a nuxeo <see cref="Entity"/> with an unknown
    /// entity type.
    /// </summary>
    public class UnknowEntity : Entity
    {
        /// <summary>
        /// Gets the enclosed json object.
        /// </summary>
        public JToken Json { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="UnknowEntity"/>.
        /// </summary>
        /// <param name="json">The unknown object's JSON representation.</param>
        public UnknowEntity(JToken json)
        {
            Json = json;
        }
    }
}