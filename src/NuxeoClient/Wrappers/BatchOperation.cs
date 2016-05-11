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

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents an Automation Operation to be performed on a batch.
    /// </summary>
    public class BatchOperation : Operation
    {
        /// <summary>
        /// Gets the <see cref="Batch"/> over which the operation will be applied.
        /// </summary>
        public Batch Batch { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="BatchOperation"/>.
        /// </summary>
        /// <param name="client">The client over which the operation is to be executed.</param>
        /// <param name="batch">The <see cref="Batch"/> over which the operation is to be executed.</param>
        /// <param name="id">The operation id.</param>
        public BatchOperation(Client client, Batch batch, string id) :
            base(client, id)
        {
            Batch = batch;
            Endpoint = UrlCombiner.Combine(client.RestPath, "upload/", Batch.BatchId, "execute/", Id);
        }
    }
}