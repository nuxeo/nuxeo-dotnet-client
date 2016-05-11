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
using System.IO;
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
        public sealed class ContentType
        {
            /// <summary>
            /// The "application/json" content type.
            /// </summary>
            public const string JSON = "application/json";
            /// <summary>
            /// The "application/json+nxrequest" content type.
            /// </summary>
            public const string NXREQUEST = "application/json+nxrequest";
            /// <summary>
            /// The "application/json+nxentity" content type.
            /// </summary>
            public const string NXENTITY = "application/json+nxentity";
            /// <summary>
            /// The "application/octet-stream" content type.
            /// </summary>
            public const string OCTETSTREAM = "application/octet-stream";
            /// <summary>
            /// The "*/*" content type.
            /// </summary>
            public const string ALL = "*/*";
        }

        /// <summary>
        /// Type of requests that can be made to the server.
        /// </summary>
        public sealed class RequestType
        {
            /// <summary>
            /// The HTTP GET method.
            /// </summary>
            public static RequestType GET = new RequestType(HttpMethod.Get);
            /// <summary>
            /// The HTTP POST method.
            /// </summary>
            public static RequestType POST = new RequestType(HttpMethod.Post);
            /// <summary>
            /// The HTTP PUT method.
            /// </summary>
            public static RequestType PUT = new RequestType(HttpMethod.Put);
            /// <summary>
            /// The HTTP DELETE method.
            /// </summary>
            public static RequestType DELETE = new RequestType(HttpMethod.Delete);

            /// <summary>
            /// Gets the corresponding <see cref="HttpMethod"/> object.
            /// </summary>
            public HttpMethod Method { get; private set; }

            private RequestType(HttpMethod method)
            {
                Method = method;
            }
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
        /// Gets the default request timeout.
        /// </summary>
        public int Timeout { get; private set; }

        /// <summary>
        /// Gets or sets the object marshaller for the client.
        /// </summary>
        public IMarshaller Marshaller { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Nuxeo <see cref="Client"/>.
        /// </summary>
        /// <param name="serverURL">The URL of the Nuxeo server.</param>
        /// <param name="authorization">The authorization credentials.</param>
        /// <param name="automationPath">The Automation API path.</param>
        /// <param name="restPath">The REST API path.</param>
        /// <param name="schemas">The default schemas to be used in the requests.</param>
        /// <param name="timeout">The default request timeout.</param>
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
                AllowAutoRedirect = false
            });
            SetTimemout(timeout);
            SetServerURL(serverURL);
            SetAuthorization(authorization);
            SetAuthomationPath(automationPath);
            SetRestPath(restPath);
            SetAuthorization(authorization);
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.ALL));
            Marshaller = new Marshaller(this).RegisterEntity("workflow", typeof(Workflow))
                                             .RegisterEntity("worflows", typeof(Workflows))
                                             .RegisterEntity("task", typeof(NuxeoClient.Wrappers.Task))
                                             .RegisterEntity("tasks", typeof(Tasks));
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
        /// Sets the default reques timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns>The current <see cref="Client"/> instance.</returns>
        public Client SetTimemout(int timeout)
        {
            Timeout = timeout;
            if (http.DefaultRequestHeaders.Contains("Nuxeo-Transaction-Timeout"))
            {
                http.DefaultRequestHeaders.Remove("Nuxeo - Transaction - Timeout");
            }
            http.DefaultRequestHeaders.Add("Nuxeo-Transaction-Timeout", Timeout.ToString());
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
            return (Batch)await Post(RestPath + "upload/");
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Client"/> and optionally disposes of the managed resources.
        /// </summary>
        public void Dispose()
        {
            http.Dispose();
        }

        /// <summary>
        /// Performs a RESTful request to the Nuxeo Server.
        /// </summary>
        /// <param name="type">The type of the request.</param>
        /// <param name="endpoint">The end point, following "api/v1/".</param>
        /// <param name="parameters">The query parameters to follow the url.</param>
        /// <param name="data">The JSON data to be send.</param>
        /// <param name="additionalHeaders">The additional request headers, besides those already specified in the client.</param>
        /// <param name="contentType">The type of the content to be sent.</param>
        /// <returns>Returns an <see cref="Entity"/> with the result from the request.</returns>
        public async Task<Entity> Request(RequestType type,
                                          string endpoint,
                                          QueryParams parameters = null,
                                          JToken data = null,
                                          Dictionary<string, string> additionalHeaders = null,
                                          string contentType = ContentType.JSON)
        {
            if (type == RequestType.GET)
            {
                return await Get(UrlCombiner.Combine(RestPath, endpoint), parameters, additionalHeaders, contentType);
            }
            else if (type == RequestType.POST)
            {
                return await Post(UrlCombiner.Combine(RestPath, endpoint), parameters, data, additionalHeaders, contentType);
            }
            else if (type == RequestType.PUT)
            {
                return await Put(UrlCombiner.Combine(RestPath, endpoint), parameters, data, additionalHeaders, contentType);
            }
            else if (type == RequestType.DELETE)
            {
                return await Delete(UrlCombiner.Combine(RestPath, endpoint), parameters, additionalHeaders, contentType);
            }
            else
            {
                throw new Exception("Invalid request type.");
            }
        }

        internal async Task<Entity> Get(string endpoint,
                                       QueryParams parameters = null,
                                       Dictionary<string, string> additionalHeaders = null,
                                       string contentType = ContentType.JSON)
        {
            return await RequestJson(endpoint, parameters, null, HttpMethod.Get, additionalHeaders, contentType);
        }

        internal async Task<Entity> Post(string endpoint,
                                         QueryParams parameters = null,
                                         JToken data = null,
                                         Dictionary<string, string> additionalHeaders = null,
                                         string contentType = ContentType.JSON)
        {
            return await RequestJson(endpoint, parameters, data, HttpMethod.Post, additionalHeaders, contentType);
        }

        internal async Task<Entity> PostBin(string endpoint,
                                            QueryParams parameters = null,
                                            byte[] data = null,
                                            Dictionary<string, string> additionalHeaders = null)
        {
            return await RequestBin(endpoint, parameters, data, HttpMethod.Post, additionalHeaders);
        }

        internal async Task<Entity> Put(string endpoint,
                                        QueryParams parameters = null,
                                        JToken data = null,
                                        Dictionary<string, string> additionalHeaders = null,
                                        string contentType = ContentType.JSON)
        {
            return await RequestJson(endpoint, parameters, data, HttpMethod.Put, additionalHeaders, contentType);
        }

        internal async Task<Entity> Delete(string endpoint,
                                           QueryParams parameters = null,
                                           Dictionary<string, string> additionalHeaders = null,
                                           string contentType = ContentType.JSON)
        {
            // TODO: review the DELETE problem
            // On Mac OS and Linux, DELETE requests will be sent as GET, which make it impossible for now
            // to delete documents and drop batches using the REST API.
            // This issue is documented here: https://github.com/dotnet/corefx/issues/4134
            return await RequestJson(endpoint, parameters, null, HttpMethod.Delete, additionalHeaders, contentType);
        }

        internal async Task<Entity> RequestJson(string endpoint,
                                                QueryParams parameters = null,
                                                JToken data = null,
                                                HttpMethod httpMethod = null,
                                                Dictionary<string, string> additionalHeaders = null,
                                                string contentType = ContentType.JSON)
        {
            HttpRequestMessage request = new HttpRequestMessage(httpMethod ?? HttpMethod.Get, (endpoint.StartsWith("/") ? endpoint.Substring(1) : endpoint) + (parameters?.ToString() ?? string.Empty));
            if (data != null)
            {
                string serializedData = JsonConvert.SerializeObject(data);
                request.Content = new StringContent(serializedData, Encoding.UTF8, contentType);
            }

            return await ProcessRequest(request, additionalHeaders);
        }

        internal async Task<Entity> RequestBin(string endpoint,
                                                 QueryParams parameters = null,
                                                 byte[] data = null,
                                                 HttpMethod httpMethod = null,
                                                 Dictionary<string, string> additionalHeaders = null,
                                                 string contentType = ContentType.OCTETSTREAM)
        {
            HttpRequestMessage request = new HttpRequestMessage(httpMethod ?? HttpMethod.Get, (endpoint.StartsWith("/") ? endpoint.Substring(1) : endpoint) + (parameters?.ToString() ?? string.Empty));
            if (data != null)
            {
                request.Content = new ByteArrayContent(data);
            }
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType.OCTETSTREAM);
            return await ProcessRequest(request, additionalHeaders);
        }

        internal async Task<Entity> RequestMultipart(string endpoint,
                                                       JToken data,
                                                       BlobList input,
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
                throw new NullReferenceException("input was null.");
            }

            HttpRequestMessage request = new HttpRequestMessage(htttpMethod ?? HttpMethod.Post, (endpoint.StartsWith("/") ? endpoint.Substring(1) : endpoint));
            MultipartContent requestContent = BuildMultipartContent(data);

            foreach (Blob blob in input)
            {
                AddBlobToMultipartContent(requestContent, blob);
            }
            request.Content = requestContent;

            return await ProcessRequest(request, additionalHeaders);
        }

        internal async Task<Entity> RequestMultipart(string endpoint,
                                                       JToken data,
                                                       Blob input,
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
                throw new NullReferenceException("input was null.");
            }

            HttpRequestMessage request = new HttpRequestMessage(htttpMethod ?? HttpMethod.Post, (endpoint.StartsWith("/") ? endpoint.Substring(1) : endpoint));
            MultipartContent requestContent = BuildMultipartContent(data);
            AddBlobToMultipartContent(requestContent, input);
            request.Content = requestContent;

            return await ProcessRequest(request, additionalHeaders);
        }


        internal MultipartContent BuildMultipartContent(JToken data)
        {
            MultipartContent requestContent = new MultipartContent("related");
            string firstPartStr = JsonConvert.SerializeObject(data);
            HttpContent firstPart = new StringContent(firstPartStr);
            firstPart.Headers.ContentType = new MediaTypeHeaderValue(Client.ContentType.NXREQUEST);
            firstPart.Headers.Add("Content-Transfer-Encoding", "8bit");
            firstPart.Headers.Add("Content-ID", "request");
            requestContent.Add(new StringContent(firstPartStr));
            return requestContent;
        }

        private void AddBlobToMultipartContent(MultipartContent content, Blob blob)
        {
            HttpContent part = new StreamContent(blob.File.OpenRead());
            part.Headers.ContentType = new MediaTypeHeaderValue(blob.MimeType);
            part.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = blob.Filename
            };
            part.Headers.Add("Content-Transfer-Encoding", "binary");
            part.Headers.Add("Content-ID", "input");
            content.Add(part);
        }

        private async Task<Entity> ProcessRequest(HttpRequestMessage request, Dictionary<string, string> additionalHeaders = null)
        {
            if (additionalHeaders != null)
            {
                foreach (KeyValuePair<string, string> pair in additionalHeaders)
                {
                    request.Headers.Add(pair.Key, pair.Value);
                }
            }
            return await ProcessResponse(await http.SendAsync(request));
        }

        private async Task<Entity> ProcessResponse(HttpResponseMessage response)
        {
            if ((int)response.StatusCode == 204 || response.Content.Headers.ContentLength == 0)
            {
                return null;
            }

            Entity entity = null;
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            bool isText = contentType.MediaType.Contains("text/");
            bool isJson = contentType.MediaType == ContentType.JSON || contentType.MediaType == ContentType.NXENTITY;
            bool isMultipart = response.Content.IsMimeMultipartContent();

            if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 499)
            {
                throw new ClientErrorException(response.StatusCode,
                    isText || isJson ? await response.Content.ReadAsStringAsync() : string.Empty);
            }
            if ((int)response.StatusCode >= 500 && (int)response.StatusCode <= 599)
            {
                throw new ServerErrorException(response.StatusCode,
                    isText || isJson ? await response.Content.ReadAsStringAsync() : string.Empty);
            }

            if (isText || isJson)
            {
                string result = await response.Content.ReadAsStringAsync();
                if (isJson)
                {
                    entity = Marshaller.UnMarshal(JToken.Parse(result));
                }
                else
                {
                    FileInfo tmpFile = IOHelper.CreateTempFile(result);
                    entity = new Blob(response.Content.Headers.ContentDisposition.FileName).SetFile(tmpFile);
                }
            }
            else if (response.Content.IsMimeMultipartContent())
            {
                MultipartMemoryStreamProvider mp = await response.Content.ReadAsMultipartAsync();
                BlobList blobs = new BlobList();
                foreach (HttpContent part in mp.Contents)
                {
                    blobs.Add(new Blob(IOHelper.CreateTempFile(await part.ReadAsStreamAsync())));
                }
                entity = blobs;
            }
            else
            {
                entity = new Blob(IOHelper.CreateTempFile(await response.Content.ReadAsStreamAsync()));
            }

            return entity;
        }
    }
}