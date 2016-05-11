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

using Newtonsoft.Json;
using System.ComponentModel;

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
        /// Gets the document's repository.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "repository")]
        public string Repository { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of <see cref="Entity"/>.
        /// </summary>
        public Entity()
        {
        }
    }
}