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