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

using Newtonsoft.Json;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents an entity obtained from the responses from the
    /// Nuxeo server. An <see cref="Entity"/> is represented by
    /// a JSON object, obtained from the server's response to a
    /// requests. Conversion between JSON and Native types is made by
    /// <see cref="IMarshaller"/>. A class extending <see cref="Entity"/>
    /// must should annotate with <see cref="JsonPropertyAttribute"/> every
    /// property that should be serialized.
    /// </summary>
    /// <remarks>For more details about Nuxeo entities, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC/REST+API+Entity+Types">Nuxeo Documentation Center</a>.
    /// For more details about <see cref="JsonPropertyAttribute"/>, check
    /// <a href="http://www.newtonsoft.com/json/help/html/JsonPropertyName.htm">Json .NET Document</a>.
    /// </remarks>
    public abstract class Entity
    {
        /// <summary>
        /// Gets the entity type.
        /// </summary>
        [JsonProperty(PropertyName = "entity-type")]
        public string EntityType { get; set; } = null;

        /// <summary>
        /// Initializes a new instance of <see cref="Entity"/>.
        /// </summary>
        public Entity()
        {
        }
    }
}