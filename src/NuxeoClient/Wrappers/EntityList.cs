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
using System.Collections;
using System.Collections.Generic;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents a list of <see cref="Entity"/>.
    /// </summary>
    public class EntityList : Entity, IReadOnlyList<Entity>
    {
        private IReadOnlyList<Entity> Entities { get; set; }

        /// <summary>
        /// Gets the number of items in the list.
        /// </summary>
        [JsonIgnore]
        public int Count
        {
            get
            {
                return Entities.Count;
            }
        }

        /// <summary>
        /// Returns the <see cref="Entity"/> object in the position specified by index in the list.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The <see cref="Entity"/> in the position specified by index.</returns>
        [JsonIgnore]
        public Entity this[int index]
        {
            get
            {
                return Entities[index];
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Entity"/>.
        /// </summary>
        /// <param name="entities">A list of <see cref="Entity"/> to be included.</param>
        public EntityList(List<Entity> entities)
        {
            Entities = entities.AsReadOnly();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
        public IEnumerator<Entity> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Entities.GetEnumerator();
        }
    }
}