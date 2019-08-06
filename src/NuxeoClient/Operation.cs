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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NuxeoClient
{
    /// <summary>
    /// Represents a Nuxeo Automation Operation.
    /// </summary>
    /// <remarks>For more details about Automation Operations, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC60/Automation">Nuxeo Documentation Center</a>.</remarks>
    public class Operation
    {
        /// <summary>
        /// The <see cref="NuxeoClient.Client"/> through which the operation will be executed.
        /// </summary>
        protected Client client { get; private set; }

        /// <summary>
        /// Gets the operation id.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the operation input.
        /// </summary>
        public object Input { get; private set; }

        /// <summary>
        /// Gets the operation parameters.
        /// </summary>
        public Dictionary<string, JToken> Parameters { get; private set; }

        /// <summary>
        /// Gets the operation request context.
        /// </summary>
        public Dictionary<string, JToken> Context { get; private set; }

        /// <summary>
        /// Gets the operation request custom headers.
        /// </summary>
        public Dictionary<string, string> AdditionalHeaders { get; private set; }

        /// <summary>
        /// Gets the operation request schemas.
        /// </summary>
        public List<string> Schemas { get; private set; }

        /// <summary>
        /// Gets the operation request timeout in seconds.
        /// </summary>
        public int Timeout { get; private set; } = 30;

        /// <summary>
        /// Gets the operation request repository.
        /// </summary>
        public string Repository { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the operation endpoint.
        /// </summary>
        public string Endpoint { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Operation"/>.
        /// </summary>
        /// <param name="client">The client through which the operation will be executed.</param>
        /// <param name="id">The operation id.</param>
        public Operation(Client client, string id)
        {
            this.client = client;
            Id = id;
            Input = null;
            Parameters = null;
            Context = null;
            Endpoint = UrlCombiner.Combine(this.client.AutomationPath, Id);
        }

        /// <summary>
        /// Sets the operation input.
        /// </summary>
        /// <param name="input">The operation input.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetInput(string input)
        {
            Input = input;
            return this;
        }

        /// <summary>
        /// Sets the operation input.
        /// </summary>
        /// <param name="input">The operation input.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetInput(JToken input)
        {
            Input = input;
            return this;
        }

        /// <summary>
        /// Sets the operation input.
        /// </summary>
        /// <param name="input">The operation input.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetInput(Blob input)
        {
            Input = input;
            return this;
        }

        /// <summary>
        /// Sets the operation input.
        /// </summary>
        /// <param name="input">The operation input.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetInput(BlobList input)
        {
            Input = input;
            return this;
        }

        /// <summary>
        /// Sets the operation input.
        /// </summary>
        /// <param name="input">The operation input.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetInput(Document input)
        {
            Input = input;
            return this;
        }

        /// <summary>
        /// Sets the operation input.
        /// </summary>
        /// <param name="input">The operation input.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetInput(ICollection<Document> input)
        {
            Input = input;
            return this;
        }

        /// <summary>
        /// Sets the operation input.
        /// </summary>
        /// <param name="input">The operation input.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetInput(Documents input)
        {
            Input = input;
            return this;
        }

        /// <summary>
        /// Sets a context property.
        /// </summary>
        /// <param name="property">The property's name.</param>
        /// <param name="value">The property's value.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetContext(string property, JToken value)
        {
            Context = Context ?? new Dictionary<string, JToken>();
            Context.Add(property, value);
            return this;
        }

        /// <summary>
        /// Clear a context property.
        /// </summary>
        /// <param name="property">The property's name.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation ClearContext(string property)
        {
            Context?.Remove(property);
            return this;
        }

        /// <summary>
        /// Clears a context properties.
        /// </summary>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation ClearContext()
        {
            Context?.Clear();
            return this;
        }

        /// <summary>
        /// Sets a parameter property.
        /// </summary>
        /// <param name="property">The property's name.</param>
        /// <param name="value">The property's value.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetParameter(string property, JToken value)
        {
            Parameters = Parameters ?? new Dictionary<string, JToken>();
            Parameters.Add(property, value);
            return this;
        }

        /// <summary>
        /// Sets a parameter property.
        /// </summary>
        /// <param name="property">The property's name.</param>
        /// <param name="value">The property's value.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetParameter(string property, ParamProperties value)
        {
            Parameters = Parameters ?? new Dictionary<string, JToken>();
            Parameters.Add(property, value.ToString());
            return this;
        }


        /// <summary>
        /// Sets a parameter <see cref="Document"/>
        /// </summary>
        /// <param name="property">The property's name.</param>
        /// <param name="value">The <see cref="Document"/> to use.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetParameter(string property, Document value)
        {
            return SetParameter(property, JToken.Parse(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// Sets a parameter <see cref="Documents"/>
        /// </summary>
        /// <param name="property">The property's name.</param>
        /// <param name="value">The list of <see cref="Document"/> to use.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetParameter(string property, ICollection<Document> value)
        {
            return SetParameter(property, JToken.Parse(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// Sets a parameter <see cref="Documents"/>
        /// </summary>
        /// <param name="property">The property's name.</param>
        /// <param name="value">The <see cref="Documents"/> to use.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetParameter(string property, Documents value)
        {
            return SetParameter(property, JToken.Parse(JsonConvert.SerializeObject(value)));
        }

        /// <summary>
        /// Clears a parameter property
        /// </summary>
        /// <param name="property">The property's name.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation ClearParameter(string property)
        {
            Parameters?.Remove(property);
            return this;
        }

        /// <summary>
        /// Clears all parameter properties.
        /// </summary>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation ClearParameters()
        {
            Parameters?.Clear();
            return this;
        }

        /// <summary>
        /// Adds a custom header to be send in the execution request.
        /// </summary>
        /// <param name="name">The header's name.</param>
        /// <param name="value">The header's value.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation AddHeader(string name, string value)
        {
            AdditionalHeaders = AdditionalHeaders ?? new Dictionary<string, string>();
            AdditionalHeaders.Add(name, value);
            return this;
        }

        /// <summary>
        /// Removes a custom header, which won't be sent in the execution request.
        /// </summary>
        /// <param name="name">The header's name.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation ClearHeader(string name)
        {
            AdditionalHeaders?.Remove(name);
            return this;
        }

        /// <summary>
        /// Sets the document schemas to be used in the execution request.
        /// </summary>
        /// <remarks>For more details about schemas, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC60/How+to+Override+Existing+Document+Types">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="schemas">An array containing the schema names.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetSchemas(string[] schemas)
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
        /// Sets the document schemas to be used in the execution request.
        /// </summary>
        /// <remarks>For more details about schemas, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC60/How+to+Override+Existing+Document+Types">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="schemas">A collection of strings containing the schema names.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetSchemas(ICollection<string> schemas)
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
        /// Adds a document schema to be used in the execution request.
        /// </summary>
        /// <remarks>For more details about schemas, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC60/How+to+Override+Existing+Document+Types">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="schema">The schema name.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation AddSchema(string schema)
        {
            Schemas = Schemas ?? new List<string>();
            Schemas.Add(schema);
            return this;
        }

        /// <summary>
        /// Excludes a document <paramref name="schema"/> from the execution request.
        /// </summary>
        /// <remarks>For more details about schemas, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC60/How+to+Override+Existing+Document+Types">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="schema">The schema name.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation ClearSchema(string schema)
        {
            Schemas?.Remove(schema);
            return this;
        }

        /// <summary>
        /// Clear all document schemas from the execution request.
        /// </summary>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation ClearSchemas()
        {
            Schemas?.Clear();
            return this;
        }

        /// <summary>
        /// Sets the operation request timeout in seconds.
        /// </summary>
        /// <param name="timeout">The timeout in seconds.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetTimeout(int timeout)
        {
            Timeout = timeout;
            return this;
        }

        /// <summary>
        /// Sets the operation request repository.
        /// </summary>
        /// <param name="repository">The repository path.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetRepository(string repository)
        {
            Repository = repository;
            return this;
        }

        /// <summary>
        /// Executes the operation, by sending a request to the server.
        /// </summary>
        /// <returns>A <see cref="Task"/> that will return an instance of <see cref="Entity"/> with the operation result.</returns>
        public async Task<Entity> Execute()
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
            headers["Nuxeo-Transaction-Timeout"] = Timeout.ToString();
            if (Repository != null && Repository != string.Empty)
            {
                headers["X-NXRepository"] = Repository;
            }

            JObject data = new JObject();
            if (Parameters != null)
            {
                JObject jsonParams = new JObject();
                foreach (KeyValuePair<string, JToken> param in Parameters)
                {
                    jsonParams.Add(param.Key, param.Value);
                }
                data.Add("params", jsonParams);
            }
            if (Context != null)
            {
                JObject jsonCotext = new JObject();
                foreach (KeyValuePair<string, JToken> param in Context)
                {
                    jsonCotext.Add(param.Key, param.Value);
                }
                data.Add("context", jsonCotext);
            }

            if (Input is Blob)
            {
                return await client.RequestMultipart(Endpoint, data, (Blob)Input, HttpMethod.Post, headers);
            }
            if (Input is BlobList)
            {
                return await client.RequestMultipart(Endpoint, data, (BlobList)Input, HttpMethod.Post, headers);
            }
            else
            {
                ICollection<Document> docs = Input is Documents ? ((Documents)Input).Entries : Input as ICollection<Document>;
                if (docs != null)
                {
                    data.Add("input", $"docs:{string.Join(",", docs.Select(d => d.Uid).ToArray())}");
                }
                else
                {
                    Document doc = Input as Document;
                    if (!string.IsNullOrWhiteSpace(doc?.Uid))
                    {
                        data.Add("input", $"doc:{doc.Uid}");
                    }
                    else
                    {
                        data.Add("input", (string)Input);
                    }
                }
                return await client.RequestJson(Endpoint, null, data, HttpMethod.Post, headers);
            }
        }
    }
}