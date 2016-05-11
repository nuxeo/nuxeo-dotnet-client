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

namespace NuxeoClient.Adapters
{
    /// <summary>
    /// Represents a Custom Document adapter.
    /// </summary>
    /// <remarks>For more details about Custom Document Adapters, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC/Web+Adapters+for+the+REST+API#WebAdaptersfortheRESTAPI-CustomAdapters">Nuxeo Documentation Page</a>.</remarks>
    public abstract class BusinessAdapter : Adapter
    {
        /// <summary>
        /// Gets the document adapter type.
        /// </summary>
        /// <remarks>The document adapter's type is refected by the JSON object's "entity-type" property.</remarks>
        public string Type { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="BusinessAdapter"/>.
        /// </summary>
        /// <param name="type">The document adapter type.</param>
        public BusinessAdapter(string type) :
            base("bo")
        {
            Type = type;
            Parameters = type;
        }
    }
}