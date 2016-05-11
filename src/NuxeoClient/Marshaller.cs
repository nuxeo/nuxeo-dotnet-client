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
using Newtonsoft.Json.Linq;
using NuxeoClient.Wrappers;
using System;
using System.Collections.Generic;

namespace NuxeoClient
{
    /// <summary>
    /// This object is responsible for the conversions between JSON objects
    /// received from the Nuxeo server and C# Nuxeo Objects.
    /// </summary>
    public class Marshaller : IMarshaller
    {
        private Client client;
        private Dictionary<string, Type> businessObjects;
        private Dictionary<string, Type> entityMap;

        /// <summary>
        /// Initializes a new instance of <see cref="Marshaller"/>.
        /// </summary>
        /// <param name="client">The client throw which objects are received and sent through.</param>
        public Marshaller(Client client)
        {
            this.client = client;
            businessObjects = new Dictionary<string, Type>();
            entityMap = new Dictionary<string, Type>();
        }

        /// <summary>
        /// Registers a business object type in the marshaller.
        /// </summary>
        /// <remarks>Registering a business object type in the marshaller will allow
        /// objects of this type to be marshalled and unmarshalled.</remarks>
        /// <param name="adapterType">A string with the type of the adapter for the business object</param>
        /// <param name="objectType">The type of the business object.</param>
        /// <returns>The current <see cref="Marshaller"/> intance.</returns>
        public IMarshaller RegisterBO(string adapterType, Type objectType)
        {
            businessObjects.Add(adapterType, objectType);
            return this;
        }

        /// <summary>
        /// Registers a new entity type in the marshaller.
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        /// <param name="objectType">The type extending <see cref="Entity"/>.</param>
        /// <returns>The current <see cref="Marshaller"/> intance.</returns>
        public IMarshaller RegisterEntity(string entityType, Type objectType)
        {
            entityMap.Add(entityType, objectType);
            return this;
        }

        /// <summary>
        /// Unmarshals a JSON object into a nuxeo <see cref="Entity"/>.
        /// </summary>
        /// <param name="json">The JSON object to be unmarshalled.</param>
        /// <returns>The unmarshalled nuxeo <see cref="Entity"/>.</returns>
        public Entity UnMarshal(JToken json)
        {
            if (json == null)
            {
                return null;
            }
            else
            {
                if (json is JArray)
                {
                    return UnMarshalJArray((JArray)json);
                }
                else if (json is JObject)
                {
                    return UnMarshalJObject((JObject)json);
                }
                else
                {
                    throw new InvalidEntityException(json.ToString());
                }
            }
        }

        /// <summary>
        /// Marshals a nuxeo <see cref="Entity"/> to JSON.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to be marshalled.</param>
        /// <returns>The marshalled JSON object.</returns>
        public JObject Marshal(Entity entity)
        {
            JObject json = JObject.Parse(JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            }));
            if (businessObjects.ContainsKey(entity.EntityType))
            {
                JObject value = new JObject();
                List<string> toRemove = new List<string>();
                foreach (KeyValuePair<string, JToken> pair in json)
                {
                    if (pair.Key == "entity-type")
                        continue;
                    value.Add(pair.Key, pair.Value);
                    toRemove.Add(pair.Key);
                }
                foreach (string property in toRemove)
                {
                    json.Remove(property);
                }
                json.Add("value", value);
            }
            return json;
        }

        private Entity UnMarshalJArray(JArray jArr)
        {
            List<Entity> entities = new List<Entity>();
            foreach (JToken token in jArr)
            {
                if (token is JArray)
                {
                    entities.Add(UnMarshalJArray((JArray)token));
                }
                else if (token is JObject)
                {
                    entities.Add(UnMarshalJObject((JObject)token));
                }
            }
            return new EntityList<Entity>(entities);
        }

        private Entity UnMarshalJObject(JObject jObj)
        {
            Entity result = null;
            if (jObj["entity-type"] != null)
            {
                string entityType = jObj["entity-type"].ToObject<string>();
                if (entityType == "documents")
                {
                    if (jObj["isPaginable"] != null &&
                        jObj["isPaginable"].ToObject<bool>() == true)
                    {
                        Pageable page = JsonConvert.DeserializeObject<Pageable>(jObj.ToString());
                        foreach (Document document in page.Entries)
                        {
                            document.SetClient(client);
                        }
                        result = page;
                    }
                    else
                    {
                        Documents docs = JsonConvert.DeserializeObject<Documents>(jObj.ToString());
                        foreach (Document document in docs.Entries)
                        {
                            document.SetClient(client);
                        }
                        result = docs;
                    }
                }
                else if (entityType == "document")
                {
                    result = JsonConvert.DeserializeObject<Document>(jObj.ToString()).SetClient(client);
                }
                else if (entityType == "null")
                {
                    result = null;
                }
                else if (entityMap.ContainsKey(entityType))
                {
                    result = (Entity)JsonConvert.DeserializeObject(jObj.ToString(), entityMap[entityType]);
                }
                else if (businessObjects.ContainsKey(entityType))
                {
                    if (jObj["value"] != null && jObj["value"] is JObject)
                    {
                        JObject boJobj = new JObject();
                        boJobj.Add("entity-type", entityType);
                        foreach (KeyValuePair<string, JToken> pair in jObj["value"].ToObject<JObject>())
                        {
                            boJobj.Add(pair.Key, pair.Value);
                        }
                        result = (Entity)JsonConvert.DeserializeObject(boJobj.ToString(), businessObjects[entityType]);
                    }
                    else
                    {
                        throw new InvalidEntityException(jObj.ToString());
                    }
                }
                else
                {
                    result = new UnknowEntity(jObj);
                    result.EntityType = entityType;
                }
            }
            else if (jObj["batchId"] != null)
            {
                result = JsonConvert.DeserializeObject<Batch>(jObj.ToString()).SetClient(client).SetClient(client);
            }
            else if (jObj["name"] != null &&
                     jObj["size"] != null &&
                     jObj["uploadType"] != null)
            {
                result = JsonConvert.DeserializeObject<BatchFile>(jObj.ToString());
            }
            else
            {
                result = new UnknowEntity(jObj);
            }
            return result;
        }
    }
}