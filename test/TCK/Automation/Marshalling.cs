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
using NuxeoClient;
using NuxeoClient.Wrappers;
using System;
using Xunit;

namespace TCK.Automation
{
    public class Marshalling : IDisposable
    {
        private Client client;
        private string creationFields;
        private string updateFields;

        public Marshalling()
        {
            client = new Client(Config.Instance.GetServerUrl());
            client.AddDefaultSchema("dublincore");
            creationFields = @"[
    {
        ""fieldType"": ""string"",
        ""description"": ""desc field0"",
        ""roles"": [
            ""Decision"",
            ""Score""
        ],
        ""name"": ""field0"",
        ""columnName"": ""col0"",
        ""sqlTypeHint"": ""whatever""
    },
    {
        ""fieldType"": ""string"",
        ""description"": ""desc field1"",
        ""roles"": [
            ""Decision"",
            ""Score""
        ],
        ""name"": ""field1"",
        ""columnName"": ""col1"",
        ""sqlTypeHint"": ""whatever""
    },
    {
        ""fieldType"": ""string"",
        ""description"": ""desc field2"",
        ""roles"": [
            ""Decision"",
            ""Score""
        ],
        ""name"": ""field2"",
        ""columnName"": ""col2"",
        ""sqlTypeHint"": ""whatever""
    },
    {
        ""fieldType"": ""string"",
        ""description"": ""desc field3"",
        ""roles"": [
            ""Decision"",
            ""Score""
        ],
        ""name"": ""field3"",
        ""columnName"": ""col3"",
        ""sqlTypeHint"": ""whatever""
    },
    {
        ""fieldType"": ""string"",
        ""description"": ""desc field4"",
        ""roles"": [
            ""Decision"",
            ""Score""
        ],
        ""name"": ""field4"",
        ""columnName"": ""col4"",
        ""sqlTypeHint"": ""whatever""
    }
]";
            updateFields = @"[
    {
        ""fieldType"":""string"",
        ""description"":""desc fieldA"",
        ""name"":""fieldA"",
        ""columnName"":""colA"",
        ""sqlTypeHint"":""whatever""
    },
    {
        ""fieldType"":""string"",
        ""description"":""desc fieldB"",
        ""name"":""fieldB"",
        ""columnName"":""colB"",
        ""sqlTypeHint"":""whatever""
    }
]";
        }

        [Fact]
        public void TestComplexMarshaling()
        {
            CreateDocument();
            UpdateDocument();
            DeleteDocument();
        }

        public void CreateDocument()
        {
            ParamProperties properties = new ParamProperties
            {
                { "ds:tableName", "MyTable" },
                { "ds:attachments", new JArray {"att1", "att2", "att3"}},
                { "ds:fields", creationFields},
                { "dc:title", "testDoc"}
            };
            //string properties = "ds:tableName=MyTable\ndc:title=testDoc\nds:attachments=[\"att1\",\"att2\",\"att3\"]\nds:fields=" + creationFields.Replace("\r\n", "");
            Document document = (Document)client.Operation("Document.Create")
                                                .SetInput("doc:/")
                                                .SetParameter("type", "DataSet")
                                                .SetParameter("name", "testDoc")
                                                .SetParameter("properties", properties)
                                                .Execute()
                                                .Result;
            Assert.NotNull(document);
            Assert.Equal("/testDoc", document.Path);
        }

        public void UpdateDocument()
        {
            ParamProperties properties = new ParamProperties
            {
                { "ds:fields", updateFields }
            };
            Document document = (Document)client.Operation("Document.Update")
                                                .SetInput("doc:/testDoc")
                                                .SetParameter("properties", properties)
                                                .Execute()
                                                .Result;
            Assert.NotNull(document);
            Assert.NotNull(document.Uid);
        }

        public void DeleteDocument()
        {
            Entity shouldBeNull = client.Operation("Document.Delete")
                                       .SetInput("doc:/testDoc")
                                       .Execute()
                                       .Result;
            Assert.Null(shouldBeNull);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}