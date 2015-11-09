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

namespace NuxeoClient.Adapters
{
    /// <summary>
    /// Represents a Page Provider (PP) Adapter, which returns the result
    /// of the query corresponding to the named PageProvider.
    /// </summary>
    /// <remarks>For more details about Adapters, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC/Web+Adapters+for+the+REST+API">Nuxeo Documentation Page</a>.</remarks>
    public class PPAdapter : Adapter
    {
        /// <summary>
        /// Gets the name of the page provider.
        /// </summary>
        public string PageProviderName { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="PPAdapter"/>.
        /// </summary>
        public PPAdapter() :
            base("pp")
        { }

        /// <summary>
        /// Sets the name of the page provider.
        /// </summary>
        /// <param name="name">The name of the page provider.</param>
        /// <returns>The current <see cref="PPAdapter"/> instance.</returns>
        public PPAdapter SetPageProviderName(string name)
        {
            PageProviderName = name;
            Parameters = PageProviderName;
            return this;
        }
    }
}