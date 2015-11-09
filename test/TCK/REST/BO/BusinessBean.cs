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
using NuxeoClient.Wrappers;
using System.ComponentModel;

namespace TCK.REST.BO
{
    public class BusinessBean : BusinessObject
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;

        [DefaultValue("")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        [DefaultValue(null)]
        [JsonProperty(PropertyName = "object")]
        public object Object { get; set; } = null;

        public BusinessBean() :
            base("BusinessBeanAdapter")
        { }

        public BusinessBean(string title, string description, string note,
                string type, object obj) :
                    base("BusinessBeanAdapter")
        {
            Title = title;
            Description = description;
            Note = note;
            Type = type;
            Object = obj;
        }

        public BusinessBean SetObject(object obj)
        {
            Object = obj;
            return this;
        }

        public BusinessBean SetType(string type)
        {
            Type = type;
            return this;
        }

        public BusinessBean SetNote(string content)
        {
            Note = content;
            return this;
        }

        public BusinessBean SetTitle(string title)
        {
            Title = title;
            return this;
        }

        public BusinessBean SetDescription(string description)
        {
            Description = description;
            return this;
        }

        public BusinessBean SetId(string id)
        {
            Id = id;
            return this;
        }
    }
}