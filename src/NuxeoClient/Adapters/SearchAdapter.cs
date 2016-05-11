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

namespace NuxeoClient.Adapters
{
    /// <summary>
    /// Represents a Search Adapter, which returns paged results a <see cref="SearchQuery"/>.
    /// </summary>
    /// <remarks>For more details about Adapters, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC/Web+Adapters+for+the+REST+API">Nuxeo Documentation Page</a>.</remarks>
    public class SearchAdapter : Adapter
    {
        /// <summary>
        /// Possible search modes.
        /// </summary>
        public enum SearchMode
        {
            /// <summary>
            /// Full text search.
            /// </summary>
            FULLTEXT,
            /// <summary>
            /// Search via NXQL.
            /// </summary>
            NXQL
        }

        /// <summary>
        /// Gets the current search mode.
        /// </summary>
        public SearchMode Mode { get; protected set; } = SearchMode.FULLTEXT;

        /// <summary>
        /// Gets the search query, which can be full-text or NXQL.
        /// </summary>
        public string SearchQuery { get; protected set; }

        /// <summary>
        /// Gets the property by which results will be ordered.
        /// </summary>
        public string OrderBy { get; protected set; } = "dc:title";

        /// <summary>
        /// Gets the results page to retrieve.
        /// </summary>
        public string Page { get; protected set; } = "0";

        /// <summary>
        /// Gets the results page size.
        /// </summary>
        public string PageSize { get; protected set; } = "50";

        /// <summary>
        /// Gets the maximum number of results to retrieve.
        /// </summary>
        public string MaxResult { get; protected set; } = "nolimit";

        /// <summary>
        /// Gets the query parameters for this <see cref="SearchAdapter"/>.
        /// </summary>
        protected QueryParams QParams { get; set; } = new QueryParams();

        /// <summary>
        /// Initializes a new instance of <see cref="SearchAdapter"/>.
        /// </summary>
        public SearchAdapter() :
            base("search")
        { }

        /// <summary>
        /// Sets the property name by which results should be ordered.
        /// </summary>
        /// <param name="orberby">The property name by which results should be ordered.</param>
        /// <returns>The current <see cref="SearchAdapter"/> instance.</returns>
        public SearchAdapter SetOrberBy(string orberby)
        {
            OrderBy = orberby;
            QParams["orderBy"] = OrderBy;
            Parameters = QParams.ToString();
            return this;
        }

        /// <summary>
        /// Sets the results page to retrieve.
        /// </summary>
        /// <param name="page">The number of the results page.</param>
        /// <returns>The current <see cref="SearchAdapter"/> instance.</returns>
        public SearchAdapter SetPage(string page)
        {
            Page = page;
            QParams["page"] = Page;
            Parameters = QParams.ToString();
            return this;
        }

        /// <summary>
        /// Sets the size of the results page.
        /// </summary>
        /// <param name="pageSize">The size of the results page.</param>
        /// <returns>The current <see cref="SearchAdapter"/> instance.</returns>
        public SearchAdapter SetPageSize(string pageSize)
        {
            PageSize = pageSize;
            QParams["pageSize"] = PageSize;
            Parameters = QParams.ToString();
            return this;
        }

        /// <summary>
        /// Sets the maximum number os results to retrieve.
        /// </summary>
        /// <param name="maxResult">The maximum number os results to retrieve.</param>
        /// <returns>The current <see cref="SearchAdapter"/> instance.</returns>
        public SearchAdapter SetMaxResult(string maxResult)
        {
            MaxResult = maxResult;
            QParams["maxResult"] = MaxResult;
            Parameters = QParams.ToString();
            return this;
        }

        /// <summary>
        /// Sets the current Search <see cref="Mode"/>.
        /// </summary>
        /// <param name="mode">The current search mode.</param>
        /// <returns>The current <see cref="SearchAdapter"/> instance.</returns>
        public SearchAdapter SetSearchMode(SearchMode mode)
        {
            Mode = mode;
            return this;
        }

        /// <summary>
        /// Sets the search query, which can be full-text or NXQL.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns>The current <see cref="SearchAdapter"/> instance.</returns>
        public SearchAdapter SetSearchQuery(string query)
        {
            SearchQuery = query;
            if (Mode == SearchMode.FULLTEXT)
                QParams["fullText"] = SearchQuery;
            else
                QParams["query"] = SearchQuery;
            Parameters = QParams.ToString();
            return this;
        }
    }
}