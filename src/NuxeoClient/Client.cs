﻿/*
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
using NuxeoClient.Wrappers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NuxeoClient
{
    /// <summary>
    /// A client that provides all the methods required to perform Automation
    /// operations and REST CRUD operations on an instance of the Nuxeo Server.
    /// </summary>
    public class Client : IDisposable
    {
        private HttpClient http;

        /// <summary>
        /// Type of content of a  requests.
        /// </summary>
        protected sealed class ContentType
        {
            public const string JSON = "application/json";
            public const string NXREQUEST = "application/json+nxrequest";
            public const string OCTETSTREAM = "application/octet-stream";
            public const string ALL = "*/*";
        }

        /// <summary>
        /// Gets the URL to the Nuxeo Server.
        /// </summary>
        public string ServerURL { get; private set; }

        /// <summary>
        /// Gets the path to the Automation API endpoint.
        /// </summary>
        public string AutomationPath { get; private set; }

        /// <summary>
        /// Gets the path to the REST API endpoint.
        /// </summary>
        public string RestPath { get; private set; }

        /// <summary>
        /// Gets the Authorization information to be sent in all requests.
        /// </summary>
        public Authorization Authorization { get; private set; }

        /// <summary>
        /// Gets the default document schemas to be sent in all request.
        /// </summary>
        public List<string> DefaultSchemas { get; private set; }

        /// <summary>
        /// Gets or sets the object marshaller for the client.
        /// </summary>
        public IMarshaller Marshaller { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Nuxeo <see cref="Client"/>.
        /// </summary>
        /// <param name="serverURL">The URL of the Nuxeo server.</param>
        /// <param name="authorization"></param>
        /// <param name="automationPath"></param>
        /// <param name="restPath"></param>
        /// <param name="schemas"></param>
        /// <param name="timeout"></param>
        public Client(string serverURL = "http://localhost:8080/nuxeo/",
                      Authorization authorization = null,
                      string automationPath = "api/v1/automation/",
                      string restPath = "api/v1/",
                      string[] schemas = null,
                      int timeout = 30)
        {
            http = new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.None,
                AllowAutoRedirect = false/*,
                Proxy = new WebProxy(new Uri("http://127.0.0.1:8888")),
                UseProxy = true*/ // used to debug with Fiddler and Charles Proxy
            });
            SetServerURL(serverURL);
            SetAuthorization(authorization);
            SetAuthomationPath(automationPath);
            SetRestPath(restPath);
            SetAuthorization(authorization);
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.ALL));
            Marshaller = new Marshaller(this);
        }

        /// <summary>
        /// Sets the URL of the Nuxeo server.
        /// </summary>
        /// <param name="url">The Nuxeo server's URL.</param>
        /// <returns>The current <see cref="Client"/> instace.</returns>
        public Client SetServerURL(string url)
        {
            if (url == null || url == string.Empty)
            {
                ServerURL = "http://localhost:8080/nuxeo/";
            }
            else
            {
                ServerURL = url;
            }
            http.BaseAddress = new Uri(ServerURL);
            return this;
        }

        /// <summary>
        /// Sets the path to the Automation endpoint.
        /// </summary>
        /// <param name="path">The path to the Automation endpoint.</param>
        /// <returns>The current <see cref="Client"/> instace.</returns>
        public Client SetAuthomationPath(string path)
        {
            AutomationPath = path;
            return this;
        }

        /// <summary>
        /// Sets the path to the REST API endpoint.
        /// </summary>
        /// <param name="path">The path to the REST API endpoint.</param>
        /// <returns>The current <see cref="Client"/> instace.</returns>
        public Client SetRestPath(string path)
        {
            RestPath = path;
            return this;
        }

        /// <summary>
        /// Sets the authorization information to be sent in all requests.
        /// </summary>
        /// <param name="auth">The authorization data.</param>
        /// <returns>The current <see cref="Client"/> instace.</returns>
        public Client SetAuthorization(Authorization auth)
        {
            Authorization = auth ?? new Authorization();
            string authorizationHeaderValue = Authorization.GenerateAuthorizationParameter();
            if (http.DefaultRequestHeaders.Contains("Authorization"))
            {
                http.DefaultRequestHeaders.Remove("Authorization");
            }
            http.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);
            return this;
        }

        /// <summary>
        /// Sets the default document schemas to be sent in all requests.
        /// </summary>
        /// <param name="schemas">One or more strings representing schema names.</param>
        /// <remarks>For more details about schemas, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC60/How+to+Override+Existing+Document+Types">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <returns>The current <see cref="Client"/> instace.</returns>
        public Client SetDefaultSchemas(string[] schemas)
        {
            DefaultSchemas?.Clear();
            DefaultSchemas = DefaultSchemas ?? new List<string>();
            foreach (string schema in schemas)
            {
                DefaultSchemas.Add(schema);
            }
            if (http.DefaultRequestHeaders.Contains("X-NXDocumentProperties"))
            {
                http.DefaultRequestHeaders.Remove("X-NXDocumentProperties");
            }
            http.DefaultRequestHeaders.Add("X-NXDocumentProperties", string.Join(",", DefaultSchemas));
            return this;
        }

        /// <summary>
        /// Adds a default document schema to be sent in all requests.
        /// </summary>
        /// <remarks>For more details about schemas, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC60/How+to+Override+Existing+Document+Types">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="schema">A string containing the schema's name.</param>
        /// <returns>The current <see cref="Client"/> instace.</returns>
        public Client AddDefaultSchema(string schema)
        {
            DefaultSchemas = DefaultSchemas ?? new List<string>();
            DefaultSchemas.Add(schema);
            if (http.DefaultRequestHeaders.Contains("X-NXDocumentProperties"))
            {
                http.DefaultRequestHeaders.Remove("X-NXDocumentProperties");
            }
            http.DefaultRequestHeaders.Add("X-NXDocumentProperties", string.Join(",", DefaultSchemas));
            return this;
        }

        /// <summary>
        /// Removes a default schema to be sent in all requests.
        /// </summary>
        /// <remarks>For more details about schemas, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC60/How+to+Override+Existing+Document+Types">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <param name="schema">A string containing the schema's name.</param>
        /// <returns>The current <see cref="Client"/> instance.</returns>
        public Client RemoveDefaultSchema(string schema)
        {
            DefaultSchemas?.Remove(schema);
            if (http.DefaultRequestHeaders.Contains("X-NXDocumentProperties"))
            {
                http.DefaultRequestHeaders.Remove("X-NXDocumentProperties");
            }
            http.DefaultRequestHeaders.Add("X-NXDocumentProperties", string.Join(",", DefaultSchemas));
            return this;
        }

        /// <summary>
        /// Removes all default schemas to be sent in all requests.
        /// </summary>
        /// <remarks>For more details about schemas, check
        /// <a href="https://doc.nuxeo.com/display/NXDOC60/How+to+Override+Existing+Document+Types">Nuxeo Documentation Center</a>.
        /// </remarks>
        /// <returns>The current <see cref="Client"/> instance.</returns>
        public Client ClearDefaultSchemas()
        {
            DefaultSchemas?.Clear();
            return this;
        }

        /// <summary>
        /// Sets the object marshaller for this client.
        /// </summary>
        /// <param name="marshaller">The object marshaller implementing the <see cref="IMarshaller"/> interface.</param>
        /// <returns>The current <see cref="Client"/> instance.</returns>
        public Client SetMarshaller(IMarshaller marshaller)
        {
            Marshaller = marshaller;
            return this;
        }

        /// <summary>
        /// Creates a new instance of an Automation <see cref="Operation"/>.
        /// </summary>
        /// <param name="id">The operation's id.</param>
        /// <returns>A new <see cref="Operation"/> instance.</returns>
        public Operation Operation(string id)
        {
            return new Operation(this, id);
        }

        /// <summary>
        /// Creates a new instance of a Nuxeo Document.
        /// </summary>
        /// <param name="path">The remote path to the document.</param>
        /// <returns>A <see cref="Document"/> object representing the remote document.</returns>
        public Document DocumentFromPath(string path)
        {
            Document doc = new Document().SetClient(this);
            doc.Path = path;
            return doc;
        }

        /// <summary>
        /// Creates a new instance of a Nuxeo Document.
        /// </summary>
        /// <param name="uid">The remote document's UID.</param>
        /// <returns>A <see cref="Document"/> object representing the remote document.</returns>
        public Document DocumentFromUid(string uid)
        {
            Document doc = new Document().SetClient(this);
            doc.Uid = uid;
            return doc;
        }

        /// <summary>
        /// Returns a new instance of Uploader.
        /// </summary>
        /// <returns>A new <see cref="Uploader"/> instance.</returns>
        public Uploader Uploader()
        {
            return new Uploader(this);
        }

        /// <summary>
        /// Requests a new Batch from the server.
        /// </summary>
        /// <returns>A new <see cref="Task"/> that will return the <see cref="Batch"/> object
        /// representing the remote batch.</returns>
        public async Task<Batch> Batch()
        {
            return (Batch)await Post(ServerURL + RestPath + "upload/");
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Client"/> and optionally disposes of the managed resources.
        /// </summary>
        public void Dispose()
        {
            http.Dispose();
        }

        internal async Task<Entity> Get(string endpoint,
                                       Dictionary<string, string> additionalHeaders = null,
                                       string contentType = ContentType.JSON)
        {
            return await RequestJson(endpoint, null, HttpMethod.Get, additionalHeaders, contentType);
        }

        internal async Task<Entity> Post(string endpoint,
                                         JToken data = null,
                                         Dictionary<string, string> additionalHeaders = null,
                                         string contentType = ContentType.JSON)
        {
            return await RequestJson(endpoint, data, HttpMethod.Post, additionalHeaders, contentType);
        }

        internal async Task<Entity> PostBin(string endpoint,
                                            byte[] data = null,
                                            Dictionary<string, string> additionalHeaders = null)
        {
            return await RequestBin(endpoint, data, HttpMethod.Post, additionalHeaders);
        }

        internal async Task<Entity> Put(string endpoint,
                                        JToken data = null,
                                        Dictionary<string, string> additionalHeaders = null,
                                        string contentType = ContentType.JSON)
        {
            return await RequestJson(endpoint, data, HttpMethod.Put, additionalHeaders, contentType);
        }

        internal async Task<Entity> Delete(string endpoint,
                                           Dictionary<string, string> additionalHeaders = null,
                                           string contentType = ContentType.JSON)
        {
            // TODO: review the DELETE problem
            // On Mac OS and Linux, DELETE requests will be sent as GET, which make it impossible for now
            // to delete documents and drop batches using the REST API.
            // This issue is documented here: https://github.com/dotnet/corefx/issues/4134
            return await RequestJson(endpoint, null, HttpMethod.Delete, additionalHeaders, contentType);
        }

        internal async Task<Entity> RequestJson(string endpoint,
                                                  JToken data = null,
                                                  HttpMethod httpMethod = null,
                                                  Dictionary<string, string> additionalHeaders = null,
                                                  string contentType = ContentType.JSON)
        {
            HttpRequestMessage request = new HttpRequestMessage(httpMethod ?? HttpMethod.Get, endpoint);
            if (data != null)
            {
                string serializedData = JsonConvert.SerializeObject(data);
                request.Content = new StringContent(serializedData, Encoding.UTF8, contentType);
            }

            return await ProcessRequest(request, additionalHeaders);
        }

        internal async Task<Entity> RequestBin(string endpoint,
                                                 byte[] data = null,
                                                 HttpMethod httpMethod = null,
                                                 Dictionary<string, string> additionalHeaders = null,
                                                 string contentType = ContentType.OCTETSTREAM)
        {
            HttpRequestMessage request = new HttpRequestMessage(httpMethod ?? HttpMethod.Get, endpoint);
            if (data != null)
            {
                request.Content = new ByteArrayContent(data);
            }
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType.OCTETSTREAM);
            return await ProcessRequest(request, additionalHeaders);
        }

        internal async Task<Entity> RequestMultipart(string endpoint,
                                                       JToken data,
                                                       JToken input,
                                                       HttpMethod htttpMethod = null,
                                                       Dictionary<string, string> additionalHeaders = null,
                                                       string contentType = ContentType.NXREQUEST)
        {
            if (data == null)
            {
                throw new NullReferenceException("data parameter was null.");
            }
            if (input == null)
            {
                throw new NullReferenceException("input parameter was null.");
            }

            HttpRequestMessage request = new HttpRequestMessage(htttpMethod ?? HttpMethod.Post, endpoint);
            MultipartContent requestContent = new MultipartContent("related");

            string firstPartStr = JsonConvert.SerializeObject(data);
            HttpContent firstPart = new StringContent(firstPartStr);
            firstPart.Headers.ContentType = new MediaTypeHeaderValue(Client.ContentType.NXREQUEST);
            firstPart.Headers.Add("Content-Transfer-Encoding", "8bit");
            firstPart.Headers.Add("Content-ID", "request");
            requestContent.Add(new StringContent(firstPartStr));
            JArray inputObjs = (input is JArray ? input.ToObject<JArray>() : new JArray { input });
            foreach (JToken obj in inputObjs)
            {
                if (obj["filename"] == null ||
                    obj["content"] == null ||
                    obj["mimetype"] == null)
                {
                    throw new HttpRequestException("Invalid blob file.");
                }
                HttpContent part = new ByteArrayContent(obj["content"].ToObject<byte[]>());
                part.Headers.ContentType = new MediaTypeHeaderValue(obj["mimetype"].ToObject<string>());
                part.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = obj["filename"].ToObject<string>()
                };
                part.Headers.Add("Content-Transfer-Encoding", "binary");
                part.Headers.Add("Content-ID", "input");
                requestContent.Add(part);
            }
            request.Content = requestContent;

            return await ProcessRequest(request, additionalHeaders);
        }

        internal async Task<Entity> ProcessRequest(HttpRequestMessage request, Dictionary<string, string> additionalHeaders = null)
        {
            if (additionalHeaders != null)
            {
                foreach (KeyValuePair<string, string> pair in additionalHeaders)
                {
                    request.Headers.Add(pair.Key, pair.Value);
                }
            }
            Entity result = null;
            await ProcessResponse(await http.SendAsync(request)).ContinueWith(output =>
            {
                JToken json = output.Result;
                result = Marshaller.UnMarshal(json);
            });
            return result;
        }

        private async Task<JToken> ProcessResponse(HttpResponseMessage response)
        {
            JToken json = null;
            await response.Content.ReadAsStringAsync().ContinueWith(output =>
            {
                string result = output.Result;
                if (result != string.Empty)
                {
                    try
                    {
                        json = JToken.Parse(result);
                    }
                    catch (JsonReaderException)
                    {
                        json = response.IsSuccessStatusCode ? null : new JObject { { "message", result } };
                    }
                    if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 599)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound &&
                            json["code"] != null &&
                            json["code"].ToObject<string>().Contains("DocumentNotFoundException"))
                        {
                            json = null;
                        }
                        else
                        {
                            if (json["message"] != null)
                            {
                                throw new ServerException(response.StatusCode, json["message"].ToObject<string>());
                            }
                            else
                            {
                                throw new ServerException(response.StatusCode, json.ToObject<string>());
                            }
                        }
                    }
                }
            });
            return json;
        }
    }
}