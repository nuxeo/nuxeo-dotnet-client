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
    /// Represents a Documents entity, which contains a collection of
    /// <see cref="Document"/>.
    /// </summary>
    public class Documents : EntityList<Document>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Documents"/>.
        /// </summary>
        public Documents()
        {
            EntityType = "documents";
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Documents"/>.
        /// </summary>
        /// <param name="documents">A list of <see cref="Document"/> to be included.</param>
        public Documents(List<Document> documents) : base(documents)
        {
            EntityType = "documents";
        }
    }
}