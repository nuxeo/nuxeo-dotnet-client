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