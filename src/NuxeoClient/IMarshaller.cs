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