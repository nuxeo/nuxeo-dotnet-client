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
        protected Client client { get; private set; } = null;

        protected enum EndpointType
        {
            UID,
            PATH
        }

        /// <summary>
        /// Gets the document's repository.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "repository")]
        public string Repository { get; set; } = string.Empty;

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
        /// Sets the Nuxeo <see cref="Client"/> through which operations to this document can be performed.
        /// </summary>
        /// <remarks>The client is used for operations like <see cref="Fetch"/>, <see cref="Create(Entity)"/>,
        /// <see cref="Update(Entity)"/> and <see cref="Save"/>.</remarks>
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
        /// <param name="repository">The document's uid.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetUid(string uid)
        {
            Uid = uid;
            return this;
        }

        /// <summary>
        /// Sets the document's path.
        /// </summary>
        /// <param name="repository">The document's path.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetPath(string path)
        {
            Path = path;
            return this;
        }

        /// <summary>
        /// Sets the document's name.
        /// </summary>
        /// <param name="repository">The document's name.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetName(string name)
        {
            Name = name;
            return this;
        }

        /// <summary>
        /// Sets the document's type.
        /// </summary>
        /// <param name="repository">The document's type.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetType(string type)
        {
            Type = type;
            return this;
        }

        /// <summary>
        /// Sets the document's title.
        /// </summary>
        /// <param name="repository">The document's title.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        /// Sets the document's properties.
        /// </summary>
        /// <param name="repository">The document's properties.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetProperties(Properties properties)
        {
            Properties = properties;
            return this;
        }

        /// <summary>
        /// Sets the document's context parameters.
        /// </summary>
        /// <param name="repository">The document's context parameters.</param>
        /// <returns>The current <see cref="Document"/> instance.</returns>
        public Document SetContextParameters(Properties contextParameters)
        {
            ContextParameters = contextParameters;
            return this;
        }

        /// <summary>
        /// Fetches the document from the server. A <see cref="BusinessObject"/> is returned instead if a
        /// <see cref="BusinessAdapter"/> is set via <see cref="SetAdapter(Adapter)"/>.
        /// </summary>
        /// <remarks>For more details about RESTful Document CRUD, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Document+Resources+Endpoints">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <returns>A <see cref="Task"/> that will return a new <see cref="Entity"/>, representing this <see cref="Document"/> or a <see cref="BusinessObject"/>.</returns>
        public async Task<Entity> Fetch()
        {
            return await client.Get(GenerateEndpoint(withAdapter: true), GenerateHeaders());
        }

        /// <summary>
        /// Creates a child of this document.
        /// </summary>
        /// <remarks>For more details about RESTful Document CRUD, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Document+Resources+Endpoints">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="data">The child document.</param>
        /// <returns>A <see cref="Task"/> that will return a new <see cref="Entity"/>, representing the child <see cref="Document"/> or <see cref="BusinessObject"/>.</returns>
        public async Task<Entity> Create(Entity entity)
        {
            return await client.Post(GenerateEndpoint(withAdapter: (entity is BusinessObject ? true : false)) + (entity is BusinessObject ? "/" + ((BusinessObject)entity).Name : string.Empty),
                client.Marshaller.Marshal(entity),
                GenerateHeaders());
        }

        /// <summary>
        /// Updates a child of this document.
        /// </summary>
        /// <remarks>For more details about RESTful Document CRUD, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Document+Resources+Endpoints">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="data">The child document.</param>
        /// <returns>A <see cref="Task"/> that will return a new <see cref="Entity"/>, representing the child <see cref="Document"/> or <see cref="BusinessObject"/>.</returns>
        public async Task<Entity> Update(Entity entity)
        {
            return await client.Put(GenerateEndpoint(withAdapter: (entity is BusinessObject ? true : false)),
                client.Marshaller.Marshal(entity),
                GenerateHeaders());
        }

        /// <summary>
        /// Deletes this document on the remote server.
        /// </summary>
        /// <remarks>For more details about RESTful Document CRUD, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC/Document+Resources+Endpoints">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <returns>A <see cref="Task"/> that will return <c>null</c> if the deletion of the remote document
        /// was successful.</returns>
        public async Task<Entity> Delete()
        {
            return await client.Delete(GenerateEndpoint(), GenerateHeaders());
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
            return (Document)await Update(new Document { Properties = DirtyProperties });
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

        private string GenerateEndpoint(bool withAdapter = false)
        {
            bool noUid = string.IsNullOrEmpty(Uid);
            bool noPath = string.IsNullOrEmpty(Path);
            if (noUid && noPath)
            {
                throw new InvalidEntityException("No remote document identifier was especified.");
            }
            else
            {
                return BuildEndpointForDoc((noPath ? Uid : Path), (noPath ? EndpointType.UID : EndpointType.PATH), withAdapter);
            }
        }

        private string BuildEndpointForDoc(string doc, EndpointType type = EndpointType.UID, bool withAdapter = false)
        {
            string endpoint = client.ServerURL;
            endpoint += client.RestPath;
            endpoint += ((Repository != null && Repository != string.Empty) ? "repo/" + Repository + "/" : string.Empty);
            endpoint += (type == EndpointType.UID ? "id" : "path");
            endpoint += (doc.Length > 0 && doc[0] != '/' ? "/" : string.Empty) + doc;
            if (withAdapter)
            {
                endpoint += (Adapter != null ? "/" + Adapter.GetEndpointSuffix() : string.Empty);
            }
            return endpoint;
        }
    }
}