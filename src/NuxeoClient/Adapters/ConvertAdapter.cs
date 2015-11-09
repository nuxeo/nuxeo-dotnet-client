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
    /// Represents a Convert Adapter, which converts target document blobs int other formats.
    /// </summary>
    /// <remarks>For more details about Adapters, check
    /// <a href="https://doc.nuxeo.com/display/NXDOC/Web+Adapters+for+the+REST+API">Nuxeo Documentation Page</a>.</remarks>
    public class ConvertAdapter : Adapter
    {
        /// <summary>
        /// Gets the target conversion format.
        /// </summary>
        public string Format { get; protected set; } = string.Empty;

        /// <summary>
        /// Gets the parameters of the <see cref="ConvertAdapter"/>.
        /// </summary>
        protected QueryParams QParams { get; set; } = new QueryParams();

        /// <summary>
        /// Initializes a new instance of <see cref="ConvertAdapter"/>.
        /// </summary>
        public ConvertAdapter() :
            base("convert")
        { }

        /// <summary>
        /// Sets the target conversion format.
        /// </summary>
        /// <param name="format">The target conversion format.</param>
        /// <returns>The current <see cref="ConvertAdapter"/> instance.</returns>
        public ConvertAdapter SetFormat(string format)
        {
            Format = format;
            QParams["format"] = format;
            Parameters = QParams.ToString();
            return this;
        }
    }
}