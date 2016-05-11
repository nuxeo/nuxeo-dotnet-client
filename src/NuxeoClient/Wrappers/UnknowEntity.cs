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