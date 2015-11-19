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
using System.Collections.Generic;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents a Tasks entity, which contains a collection of <see cref="Task"/>.
    /// </summary>
    /// <remarks>For more information about tasks, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC60/About+Tasks">Nuxeo Documentation Center</a>.
    public class Tasks : Entity
    {
        /// <summary>
        /// Gets the list of tasks.
        /// </summary>
        [JsonProperty(PropertyName = "entries")]
        public List<Task> Entries { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Tasks"/>.
        /// </summary>
        public Tasks()
        {
            EntityType = "tasks";
        }
    }
}