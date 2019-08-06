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
using NuxeoClient.Adapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents a remote Document in a Nuxeo server.
    /// </summary>
    /// <remarks>For more information about documents, check
    /// <a href="https://doc.nuxeo.com/display/Studio/Documents">Nuxeo Documentation Center</a>.
    /// </remarks>
    public class Document : Entity
    {
        private bool? _isTrashed = null;

        /// <summary>
        /// The <see cref="Client"/> instance through which operations to this document will be performed.
        /// </summary>
        protected Client client { get; private set; } = null;

        /// <summary>
        /// The type of endpoint used to reference the document.
        /// </summary>
        protected enum EndpointType
        {
            /// <summary>
            /// UID
            /// </summary>
            UID,
            /// <summary>
            /// Path
            /// </summary>
            PATH
        }

        /// <summary>
        /// Gets the document's UID.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "uid")]
        public string Uid { get; set; } = string.Empty;

        /// <summary>
        /// Gets the document's remote path.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Gets the document's name.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets the reference to the document's parent document.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "parentRef")]
        public string ParentRef { get; set; } = string.Empty;

        /// <summary>
        /// Gets the document's type.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets the document's state.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Gets the document's version label.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "versionLabel")]
        public string VerisonLabel { get; set; } = string.Empty;

        /// <summary>
        /// Gets whether the document is checked out or not.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "isCheckedOut")]
        public bool IsCheckedOut { get; set; } = false;

        /// <summary>
        /// Gets whether the document is checked out or not.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "isProxy")]
        public bool IsProxy { get; set; } = false;

        /// <summary>
        /// Gets whether the document is trashed out or not.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "isTrashed")]
        public bool IsTrashed {
            get { return _isTrashed ?? State == "deleted"; }
            set { _isTrashed = value; }
        }

        /// <summary>
        /// Gets the document's title.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets the date of when the document was last modified.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty(PropertyName = "lastModified")]
        public DateTime? LastModified { get; set; } = null;

        /// <summary>
        /// Gets the list of facets exhibited by the document.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty(PropertyName = "facets")]
        public List<string> Facets { get; set; } = null;

        /// <summary>
        /// Gets the document's properties.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty(PropertyName = "properties")]
        public Properties Properties { get; set; } = null;

        /// <summary>
        /// Gets the context parameters to be used on the next request.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty(PropertyName = "contextParameters")]
        public Properties ContextParameters { get; set; } = null;

        /// <summary>
        /// Gets the current document adapter.
        /// </summary>
        [JsonIgnore]
        public Adapter Adapter { get; protected set; } = null;

        /// <summary>
        /// Gets the document's dirty properties.
        /// </summary>
        [JsonIgnore]
        public Properties DirtyProperties { get; protected set; } = null;

        /// <summary>
        /// Gets the document's schemas, to be used on the next request.
        /// </summary>
        [JsonIgnore]
        public List<string> Schemas { get; protected set; }

        /// <summary>
        /// Gets the current content enrichers, to be used on the next request.
        /// </summary>
        [JsonIgnore]
        public List<string> ContentEnrichers { get; protected set; }

        /// <summary>
        /// Gets the additional custom headers to be used on the next request.
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, string> AdditionalHeaders { get; protected set; }

        /// <summary>
        /// Gets the request timeout-
        /// </summary>
        [JsonIgnore]
        public int Timeout { get; protected set; } = 30;

        /// <summary>
        /// Gets the document's endpoint.
        /// </summary>
        [JsonIgnore]
        public string Endpoint
        {
            get
            {
                return GenerateEndpoint();
            }
        }

        /// <summary>
        /// Gets the document's endpoint with the adapter.
        /// </summary>
        [JsonIgnore]
        public string EndpointWithAdapter
        {
            get
            {
                return GenerateEndpoint(true);
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Document"/>.
        /// </summary>
        public Document()
        {
            EntityType = "document";
        }

        /// <summary>
        /// Adds a custom header to be used in the next request.
        /// </summary>
        /// <param name="name">The header's name.</param>
        /// <param name="value">The header's value.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document AddHeader(string name, string value)
        {
            AdditionalHeaders = AdditionalHeaders ?? new Dictionary<string, string>();
            AdditionalHeaders.Add(name, value);
            return this;
        }

        /// <summary>
        /// Removes a custom header, which will no longer be sent in the next request.
        /// </summary>
        /// <param name="name">The header's name.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document RemoveHeader(string name)
        {
            AdditionalHeaders.Remove(name);
            return this;
        }

        /// <summary>
        /// Sets a list document schemas to be sent in the next request.
        /// </summary>
        /// <remarks>For more details about schemas, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC60/How+to+Override+Existing+Document+Types">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="schemas">A list of document schema names.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetSchemas(string[] schemas)
        {
            Schemas?.Clear();
            Schemas = Schemas ?? new List<string>();
            foreach (string schema in schemas)
            {
                Schemas.Add(schema);
            }
            return this;
        }

        /// <summary>
        /// Adds a document schema to be sent in the next request.
        /// </summary>
        /// <remarks>For more details about schemas, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC60/How+to+Override+Existing+Document+Types">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="schema">The schema's name.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document AddSchema(string schema)
        {
            Schemas = Schemas ?? new List<string>();
            Schemas.Add(schema);
            return this;
        }

        /// <summary>
        /// Removes a document schema, which will no longer be sent in the next request.
        /// </summary>
        /// <remarks>For more details about schemas, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC60/How+to+Override+Existing+Document+Types">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="schema">The schema's name</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document RemoveSchema(string schema)
        {
            Schemas.Remove(schema);
            return this;
        }

        /// <summary>
        /// Sets the list of content enrichers to be used in the next request.
        /// </summary>
        /// <remarks>For more details about document enrichers, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Content+Enricher">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="enrichers">The list of content enricher names.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetContentEnrichers(string[] enrichers)
        {
            ContentEnrichers?.Clear();
            ContentEnrichers = ContentEnrichers ?? new List<string>();
            foreach (string enricher in enrichers)
            {
                ContentEnrichers.Add(enricher);
            }
            return this;
        }

        /// <summary>
        /// Adds a content enricher to be used in the next request.
        /// </summary>
        /// <remarks>For more details about document enrichers, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Content+Enricher">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="enricher">The content enricher's name.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document AddContentEnricher(string enricher)
        {
            ContentEnrichers = ContentEnrichers ?? new List<string>();
            ContentEnrichers.Add(enricher);
            return this;
        }

        /// <summary>
        /// Removes a content enricher, which will no longer be used in the next request.
        /// </summary>
        /// <remarks>For more details about document enrichers, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Content+Enricher">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="enricher"></param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document RemoveContentEnricher(string enricher)
        {
            ContentEnrichers.Remove(enricher);
            return this;
        }

        /// <summary>
        /// Sets the default reques timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The current <see cref="Client"/> instance.</returns>
        public Document SetTimemout(int timeout)
        {
            Timeout = timeout;
            return this;
        }

        /// <summary>
        /// Sets the Nuxeo <see cref="Client"/> through which operations to this document can be performed.
        /// </summary>
        /// <remarks>The client is used for operations like <see cref="Get(string)"/>, <see cref="Post(Entity, string)"/>,
        /// <see cref="Put(Entity, string)"/> and <see cref="Save"/>.</remarks>
        /// <param name="client">The Nuxeo <see cref="Client"/> through which operations to this document can be performed.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetClient(Client client)
        {
            this.client = client;
            return this;
        }

        /// <summary>
        /// Sets the document's repository.
        /// </summary>
        /// <param name="repository">The path to the document's repository.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetRepository(string repository)
        {
            Repository = repository;
            return this;
        }

        /// <summary>
        /// Sets the document's uid.
        /// </summary>
        /// <param name="uid">The document's uid.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetUid(string uid)
        {
            Uid = uid;
            return this;
        }

        /// <summary>
        /// Sets the document's path.
        /// </summary>
        /// <param name="path">The document's path.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetPath(string path)
        {
            Path = path;
            return this;
        }

        /// <summary>
        /// Sets the document's name.
        /// </summary>
        /// <param name="name">The document's name.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetName(string name)
        {
            Name = name;
            return this;
        }

        /// <summary>
        /// Sets the document's type.
        /// </summary>
        /// <param name="type">The document's type.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetType(string type)
        {
            Type = type;
            return this;
        }

        /// <summary>
        /// Sets the document's title.
        /// </summary>
        /// <param name="title">The document's title.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        /// Sets the document's properties.
        /// </summary>
        /// <param name="properties">The document's properties.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetProperties(Properties properties)
        {
            Properties = properties;
            return this;
        }

        /// <summary>
        /// Sets the document's context parameters.
        /// </summary>
        /// <param name="contextParameters">The document's context parameters.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetContextParameters(Properties contextParameters)
        {
            ContextParameters = contextParameters;
            return this;
        }

        /// <summary>
        /// Performs a GET request to this document's endpoint. A <see cref="Document"/> will be
        /// returned unless an <see cref="Adapter"/> is specified. In this case, different <see cref="Entity"/>
        /// objects may be returned.
        /// </summary>
        /// <remarks>For more details about RESTful Document CRUD, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Document+Resources+Endpoints">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="extendEndpoint">A path suffix to be apeended to the document's enpoint, including the adapter.</param>
        /// <returns>A <see cref="Task"/> that will return the respective <see cref="Entity"/>.</returns>
        public async Task<Entity> Get(string extendEndpoint = "")
        {
            return await client.Get(GenerateEndpoint(true, extendEndpoint), null, GenerateHeaders());
        }

        /// <summary>
        /// Performs a post request to this documents endpoint.
        /// </summary>
        /// <remarks>For more details about RESTful Document CRUD, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Document+Resources+Endpoints">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="entity">The <see cref="Entity"/> to be posted.</param>
        /// <param name="extendEndpoint">A path suffix to be apeended to the document's enpoint, including the adapter.</param>
        /// <returns>A <see cref="Task"/> that will return the resulting <see cref="Entity"/>.</returns>
        public async Task<Entity> Post(Entity entity, string extendEndpoint = "")
        {
            return await client.Post(GenerateEndpoint(true, UrlCombiner.Combine((entity is BusinessObject ? ((BusinessObject)entity).Name : string.Empty), extendEndpoint)),
                null,
                client.Marshaller.Marshal(entity),
                GenerateHeaders());
        }

        /// <summary>
        /// Performs a POST request to this document's endpoint.
        /// </summary>
        /// <remarks>For more details about RESTful Document CRUD, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Document+Resources+Endpoints">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="data">The JSON object to be sent.</param>
        /// <param name="extendEndpoint">A path suffix to be apeended to the document's enpoint, including the adapter.</param>
        /// <returns>A <see cref="Task"/> that will return the resulting <see cref="Entity"/>.</returns>
        public async Task<Entity> Post(JToken data, string extendEndpoint = "")
        {
            return await client.Post( GenerateEndpoint(true, extendEndpoint),
                null,
                data,
                GenerateHeaders());
        }

        /// <summary>
        /// Performs a PUT request to this document's endpoint.
        /// </summary>
        /// <remarks>For more details about RESTful Document CRUD, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Document+Resources+Endpoints">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="entity">The <see cref="Entity"/> to be updated.</param>
        /// <param name="extendEndpoint">A path suffix to be apeended to the document's enpoint, including the adapter.</param>
        /// <returns>A <see cref="Task"/> that will return the resulting <see cref="Entity"/>.</returns>
        public async Task<Entity> Put(Entity entity, string extendEndpoint = "")
        {
            return await client.Put(GenerateEndpoint(true, extendEndpoint),
                null,
                client.Marshaller.Marshal(entity),
                GenerateHeaders());
        }

        /// <summary>
        /// Performs a PUT request to this document's endpoint.
        /// </summary>
        /// <remarks>For more details about RESTful Document CRUD, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Document+Resources+Endpoints">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="data">The JSON object to be sent.</param>
        /// <param name="extendEndpoint">A path suffix to be apeended to the document's enpoint, including the adapter.</param>
        /// <returns>A <see cref="Task"/> that will return the resulting <see cref="Entity"/>.</returns>
        public async Task<Entity> Put(JToken data, string extendEndpoint = "")
        {
            return await client.Put(GenerateEndpoint(true, extendEndpoint),
                null,
                data,
                GenerateHeaders());
        }

        /// <summary>
        /// Performs a DELETE request to this document's endpoint.
        /// </summary>
        /// <remarks>For more details about RESTful Document CRUD, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Document+Resources+Endpoints">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="extendEndpoint">A path suffix to be apeended to the document's enpoint, including the adapter.</param>
        /// <returns>A <see cref="Task"/> that will return <c>null</c> if the deletion of the remote document
        /// was successful.</returns>
        public async Task<Entity> Delete(string extendEndpoint = "")
        {
            return await client.Delete(GenerateEndpoint(true, extendEndpoint), null, GenerateHeaders());
        }

        /// <summary>
        /// Sets a (dirty) property of the document. No changes will be made to the remote object.
        /// </summary>
        /// <param name="name">The property's name.</param>
        /// <param name="value">The new property's value.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document Set(string name, JToken value)
        {
            DirtyProperties = DirtyProperties ?? new Properties();
            DirtyProperties[name] = value;
            return this;
        }

        /// <summary>
        /// Saves the changes made by <see cref="Set(string, JToken)"/> to this document.
        /// </summary>
        /// <returns>A <see cref="Task"/> that will return a new and updated instance of this <see cref="Document"/>.</returns>
        public async Task<Document> Save()
        {
            return (Document)await Put(new Document { Properties = DirtyProperties });
        }

        /// <summary>
        /// Sets a Web Adapter for the current document.
        /// </summary>
        /// <remarks>For more details about adapters, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Web+Adapters+for+the+REST+API">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="adapter">The <see cref="Adapter"/> to be set.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetAdapter(Adapter adapter)
        {
            Adapter = adapter;
            return this;
        }

        /// <summary>
        /// Trash the current document.
        /// </summary>
        /// <returns>The current document.</returns>
        public async Task<Document> Trash()
        {
            return (Document)await client.Operation("Document.Trash").SetInput(this).Execute();
        }

        /// <summary>
        /// Untrash the current document.
        /// </summary>
        /// <returns>The current document.</returns>
        public async Task<Document> Untrash()
        {
            return (Document)await client.Operation("Document.Untrash").SetInput(this).Execute();
        }

        /// <summary>
        /// Returns a dictionary with the name and value of the custom headers to be used
        /// on the next request for this document.
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, string> GenerateHeaders()
        {
            Dictionary<string, string> headers;
            if (AdditionalHeaders != null)
            {
                headers = new Dictionary<string, string>(AdditionalHeaders);
            }
            else
            {
                headers = new Dictionary<string, string>();
            }
            headers["Nuxeo-Transaction-Timeout"] = Timeout.ToString();
            if (Schemas != null && Schemas.Count > 0)
            {
                headers["X-NXDocumentProperties"] = string.Join(",", Schemas);
            }
            if (ContentEnrichers != null && ContentEnrichers.Count > 0)
            {
                headers["X-NXenrichers.document"] = string.Join(",", ContentEnrichers);
            }
            if (Repository != null && Repository != string.Empty)
            {
                headers["X-NXRepository"] = Repository;
            }
            return headers;
        }

        private string GenerateEndpoint(bool withAdapter = false, string extension = "")
        {
            bool noUid = string.IsNullOrEmpty(Uid);
            bool noPath = string.IsNullOrEmpty(Path);
            if (noUid && noPath)
            {
                throw new InvalidEntityException("No remote document identifier was especified.");
            }
            else
            {
                return BuildEndpointForDoc((noPath ? Uid : Path), extension, (noPath ? EndpointType.UID : EndpointType.PATH), withAdapter);
            }
        }

        private string BuildEndpointForDoc(string doc, string extension = "", EndpointType type = EndpointType.UID, bool withAdapter = false)
        {
            List<string> segments = new List<string>();
            
            segments.Add(client.RestPath);
            if (!string.IsNullOrEmpty(Repository))
            {
                segments.Add("repo");
                segments.Add(Repository);
            }
            segments.Add(type == EndpointType.UID ? "id" : "path");
            segments.Add(doc);
            if (withAdapter && Adapter != null)
            {
                segments.Add(Adapter.GetEndpointSuffix());
            }
            if (!string.IsNullOrEmpty(extension))
            {
                segments.Add(extension);
            }
            
            return UrlCombiner.Combine(segments.ToArray());
        }
    }
}