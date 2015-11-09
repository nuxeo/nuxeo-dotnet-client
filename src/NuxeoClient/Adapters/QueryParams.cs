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

using System.Collections.Generic;
using System.Linq;

namespace NuxeoClient.Adapters
{
    /// <summary>
    /// Represents URL the parameters to be send in a query.
    /// </summary>
    public class QueryParams : Dictionary<string, string>
    {
        /// <summary>
        /// Creates and returns a string representation of the current <see cref="QueryParams"/> object.
        /// </summary>
        /// <returns>A string representation of the current <see cref="QueryParams"/> object.</returns>
        public override string ToString()
        {
            return @"?" + string.Join(@"&", (from pair in this select pair.Key + @"=" + pair.Value));
        }
    }
}