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
    /// Represents a Workflows entity, which contains a collection of <see cref="Workflow"/>.
    /// </summary>
    /// <remarks>For more information about workflows, check Nuxeo Documentation Center
    /// <a href="https://doc.nuxeo.com/display/NXDOC/Workflow">here</a> and
    /// <a href="https://doc.nuxeo.com/display/USERDOC/Workflows">here</a>.
    /// </remarks>
    public class Workflows : EntityList<Workflow>
    {

        /// <summary>
        /// Initializes a new instance of <see cref="Workflows"/>.
        /// </summary>
        public Workflows() : base()
        {
            EntityType = "worflows";
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Workflows"/>.
        /// </summary>
        /// <param name="workflows">A list of <see cref="Workflow"/> to be included.</param>
        public Workflows(List<Workflow> workflows) : base(workflows)
        {
            EntityType = "workflows";
        }
    }
}