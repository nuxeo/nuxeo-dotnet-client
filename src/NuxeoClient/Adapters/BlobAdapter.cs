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
    /// Represents a Blob Adapter that returns blob corresponding to the Document
    /// attribute matching the <see cref="Xpath"/>.
    /// </summary>
    /// <remarks>For more details about Adapters, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC/Web+Adapters+for+the+REST+API">Nuxeo Documentation Page</a>.</remarks>
    public class BlobAdapter : Adapter
    {
        /// <summary>
        /// Gets the blob's xpath.
        /// </summary>
        public string Xpath { get; protected set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of <see cref="BlobAdapter"/>.
        /// </summary>
        public BlobAdapter() :
            base("blob")
        { }

        /// <summary>
        /// Sets the blob's <see cref="Xpath"/>.
        /// </summary>
        /// <param name="xpath">The xpath attribute.</param>
        /// <returns>The current <see cref="BlobAdapter"/> instance.</returns>
        public BlobAdapter SetXpath(string xpath)
        {
            Xpath = xpath;
            Parameters = Xpath;
            return this;
        }
    }
}