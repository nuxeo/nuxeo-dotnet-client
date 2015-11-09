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
            Endpoint = client.ServerURL + client.RestPath + "upload/" + Batch.BatchId + "/execute/" + Id;
        }
    }
}