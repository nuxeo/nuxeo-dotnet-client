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

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents a Business Object that is retrieved and created through a
    /// <see cref="Adapters.BusinessAdapter"/>.
    /// </summary>
    /// <remarks>For more details about Business Objects, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC/Web+Adapters+for+the+REST+API#WebAdaptersfortheRESTAPI-CustomAdapters">Nuxeo Documentation Page</a>.</remarks>
    public abstract class BusinessObject : Entity
    {
        /// <summary>
        /// Gets the Business Object's name.
        /// </summary>
        /// <remarks>The Business Object's name is only used in the creation process.
        /// It is not required for updating or fetching operations.</remarks>
        [JsonIgnore]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of <see cref="BusinessObject"/>.
        /// </summary>
        /// <param name="adapter"></param>
        public BusinessObject(string adapter)
        {
            EntityType = adapter;
        }

        /// <summary>
        /// Sets the name of the Business Object.
        /// </summary>
        /// <param name="name">The name of the Business Object.</param>
        /// <returns>The curreint <see cref="BusinessObject"/> instance.</returns>
        /// /// <remarks>The Business Object's name is only used in the creation process.
        /// It is not required for updating or fetching operations.</remarks>
        public BusinessObject SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}