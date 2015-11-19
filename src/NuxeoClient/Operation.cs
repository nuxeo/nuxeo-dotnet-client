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
using System.Collections.Generic;
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
        public JToken Input { get; private set; }

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
        /// Gets whether or not the operation request is multipart or not.
        /// </summary>
        public bool IsMultipart { get; private set; } = false;

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
            IsMultipart = false;
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
            IsMultipart = false;
            return this;
        }

        /// <summary>
        /// Sets the operation input.
        /// </summary>
        /// <param name="blob">The operation input.</param>
        /// <returns>The current <see cref="Operation"/> instance.</returns>
        public Operation SetInput(Blob blob)
        {
            Input = blob.ToJObject();
            IsMultipart = true;
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
            Entity result = null;

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
            if (Input != null && !IsMultipart)
            {
                data.Add("input", Input);
            }

            if (IsMultipart)
            {
                await client.RequestMultipart(Endpoint, data, Input, HttpMethod.Post, headers).ContinueWith(output =>
                {
                    result = output.Result;
                });
            }
            else
            {
                await client.RequestJson(Endpoint, null, data, HttpMethod.Post, headers).ContinueWith(output =>
                {
                    result = output.Result;
                });
            }

            return result;
        }
    }
}