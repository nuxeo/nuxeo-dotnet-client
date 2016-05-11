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
using System.ComponentModel;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents a pageable version of the <see cref="Documents"/> object.
    /// </summary>
    public class Pageable : Documents
    {
        /// <summary>
        /// Gets the total number of documents.
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "totalSize")]
        public int TotalSize { get; set; } = 0;

        /// <summary>
        /// Gets the current page index.
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "currentPageIndex")]
        public int CurrentPageIndex { get; set; } = 0;

        /// <summary>
        /// Gets the current page size.
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "currentPageSize")]
        public int CurrentPageSize { get; set; } = 0;

        /// <summary>
        /// Gets the maximum page size.
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "maxPageSize")]
        public int MaxPageSize { get; set; } = 0;

        /// <summary>
        /// Gets the number of pages in the remote object.
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "numberOfPages")]
        public int NumberOfPages { get; set; } = 0;

        /// <summary>
        /// Gets the page size.
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; } = 0;

        /// <summary>
        /// Gets the error message, if any.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "errorMessage")]
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets whether there was an error or not. The error message is retrieved by <see cref="ErrorMessage"/>.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "hasError")]
        public bool HasError { get; set; } = false;

        /// <summary>
        /// Gets whether the last page is available or not.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "isLastPageAvailable")]
        public bool IsLastPageAvailable { get; set; } = false;

        /// <summary>
        /// Gets whether the next page is available or not.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "isNextPageAvailable")]
        public bool IsNextPageAvailable { get; set; } = false;

        /// <summary>
        /// Gets whether the remote object is paginable or not.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "isPaginable")]
        public bool IsPaginable { get; set; } = false;

        /// <summary>
        /// Gets whether the previous page is available or not.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "isPreviousPageAvailable")]
        public bool IsPreviousPageAvailable { get; set; } = false;

        /// <summary>
        /// Gets whether the remote object is sortable or not.
        /// </summary>
        [DefaultValue(false)]
        [JsonProperty(PropertyName = "isSortable")]
        public bool IsSortable { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of <see cref="Pageable"/>.
        /// </summary>
        public Pageable()
        { }
    }
}