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
using System.Collections.Generic;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents a <see cref="Dictionary{struct, JToken}"/> structure to hold parameters operation parameters.
    /// </summary>
    public class ParamProperties : Dictionary<string, JToken>
    {
        /// <summary>
        /// Creates and returns a string representation of the current <see cref="ParamProperties"/> object.
        /// </summary>
        /// <returns>A string representation of the current <see cref="ParamProperties"/> object.</returns>
        public override string ToString()
        {
            string result = string.Empty;
            foreach (KeyValuePair<string, JToken> pair in this)
            {
                result += pair.Key + "=" + pair.Value.ToString().Replace("\r\n", "").Replace("\n", "") + "\n";
            }
            return result;
        }
    }
}