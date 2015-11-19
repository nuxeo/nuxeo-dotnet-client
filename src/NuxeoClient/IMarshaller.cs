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

using Newtonsoft.Json.Linq;
using NuxeoClient.Wrappers;
using System;

namespace NuxeoClient
{
    /// <summary>
    /// Defines methods to marshal and unmarshal nuxeo object to and from JSON.
    /// </summary>
    public interface IMarshaller
    {
        /// <summary>
        /// Registers a business object type in the marshaller.
        /// </summary>
        /// <remarks>Registering a business object type in the marshaller will allow
        /// objects of this type to be marshalled and unmarshalled.</remarks>
        /// <param name="adapterType">A string with the type of the adapter for the business object</param>
        /// <param name="objectType">The type of the business object.</param>
        /// <returns>The current <see cref="IMarshaller"/> instance.</returns>
        IMarshaller RegisterBO(string adapterType, Type objectType);

        /// <summary>
        /// Registers a new entity type in the marshaller.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="objectType">The type extending <see cref="Entity"/>.</param>
        /// <returns>The current <see cref="IMarshaller"/> instance.</returns>
        IMarshaller RegisterEntity(string entityType, Type objectType);

        /// <summary>
        /// Marshals a nuxeo <see cref="Entity"/> to JSON.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to be marshalled.</param>
        /// <returns>The marshalled JSON object.</returns>
        JObject Marshal(Entity entity);

        /// <summary>
        /// Unmarshals a JSON object into a nuxeo <see cref="Entity"/>.
        /// </summary>
        /// <param name="json">The JSON object to be unmarshalled.</param>
        /// <returns>The unmarshalled nuxeo <see cref="Entity"/>.</returns>
        Entity UnMarshal(JToken json);
    }
}