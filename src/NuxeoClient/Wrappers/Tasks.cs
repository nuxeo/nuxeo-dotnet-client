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

using System.Collections.Generic;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents a Tasks entity, which contains a collection of <see cref="Task"/>.
    /// </summary>
    /// <remarks>For more information about tasks, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC60/About+Tasks">Nuxeo Documentation Center</a>.
    /// </remarks>
    public class Tasks : EntityList<Task>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Tasks"/>.
        /// </summary>
        public Tasks() : base()
        {
            EntityType = "tasks";
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Tasks"/>.
        /// </summary>
        /// <param name="tasks">A list of <see cref="Task"/> to be included.</param>
        public Tasks(List<Task> tasks) : base(tasks)
        {
            EntityType = "tasks";
        }
    }
}