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
using System.Collections;
using System.Collections.Generic;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents a list of <see cref="Entity"/>.
    /// </summary>
    public class EntityList<T> : Entity where T : Entity
    {
        /// <summary>
        /// Gets or sets the list of entries.
        /// </summary>
        [JsonProperty(PropertyName = "entries")]
        public List<T> Entries { get; set; }

        /// <summary>
        /// Gets the number of items in the list.
        /// </summary>
        [JsonIgnore]
        public int Count
        {
            get
            {
                return Entries.Count;
            }
        }

        /// <summary>
        /// Returns the <see cref="Entity"/> object in the position specified by index in the list.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The <see cref="Entity"/> in the position specified by index.</returns>
        [JsonIgnore]
        public T this[int index]
        {
            get
            {
                return Entries[index];
            }

            set
            {
                Entries[index] = value;
            }
        }

        /// <summary>
        /// Initializes an empty instance of <see cref="EntityList{T}"/>.
        /// </summary>
        public EntityList()
        {
            Entries = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EntityList{T}"/>.
        /// </summary>
        /// <param name="entities">A list of <see cref="EntityList{T}"/> to be included.</param>
        public EntityList(List<T> entities)
        {
            Entries = entities;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the list.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific <paramref name="item"/> in the list.
        /// </summary>
        /// <param name="item">The item to be found.</param>
        /// <returns>The index of <paramref name="item"/> in the list; otherwise, -1</returns>
        public int IndexOf(T item)
        {
            return Entries.IndexOf(item);
        }

        /// <summary>
        /// Inserts an intem in the list.
        /// </summary>
        /// <param name="index">The index in which the item should be inserted.</param>
        /// <param name="item">The item to be inserted.</param>
        public void Insert(int index, T item)
        {
            Entries.Insert(index, item);
        }

        /// <summary>
        /// Removes the item from the list in a given index.
        /// </summary>
        /// <param name="index">The index of the item to be removed.</param>
        public void RemoveAt(int index)
        {
            Entries.RemoveAt(index);
        }

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        public void Add(T item)
        {
            Entries.Add(item);
        }

        /// <summary>
        /// Removes all items from the list.
        /// </summary>
        public void Clear()
        {
            Entries.Clear();
        }
        
        /// <summary>
        /// Determines whether the list contains a specific item.
        /// </summary>
        /// <param name="item">The item to be found.</param>
        /// <returns><c>true</c> if the item is in the list; false otherwise.</returns>
        public bool Contains(T item)
        {
            return Entries.Contains(item);
        }

        /// <summary>
        /// Copies the items of the list to an array, starting at a given index.
        /// </summary>
        /// <param name="array">The array to which the items will be copied.</param>
        /// <param name="arrayIndex">The starting index.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Entries.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes a specific item from the list.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <returns><c>true</c> if the item was removed; false otherwise.</returns>
        public bool Remove(T item)
        {
            return Entries.Remove(item);
        }
    }
}