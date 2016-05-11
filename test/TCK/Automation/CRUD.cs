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
    public class CRUD : IDisposable
    {
        private Client client;

        private Document testFolder;

        private Document firstChild;

        private Document secondChild;

        public CRUD()
        {
            client = new Client(Config.ServerUrl());
            client.AddDefaultSchema("dublincore");
        }

        [Fact]
        public void TestBasicCRUD()
        {
            CreateFolderOnRoot();
            CreateFirstChild();
            CreateSecondChild();
            UpdateSecondChild();
            GetChildren();
            DeleteParent();
        }

        public void CreateFolderOnRoot()
        {
            Entity doc = client.Operation("Document.Create")
                               .SetInput("doc:/")
                               .SetParameter("type", "Folder")
                               .SetParameter("name", "TestFolder1")
                               .SetParameter("properties", new ParamProperties { { "dc:title", "Test Folder 1" } })
                               .Execute()
                               .Result;
            Assert.NotNull(doc);
            Assert.True(doc is Document);
            testFolder = (Document)doc;
            Assert.Equal("/TestFolder1", testFolder.Path);
            Assert.Equal("Test Folder 1", testFolder.Title);
        }

        public void CreateFirstChild()
        {
            Entity doc = client.Operation("Document.Create")
                               .SetInput("doc:" + testFolder.Path)
                               .SetParameter("type", "File")
                               .SetParameter("name", "TestFile1")
                               .SetParameter("properties", new ParamProperties { { "dc:title", "Test File 1" } })
                               .Execute()
                               .Result;
            Assert.NotNull(doc);
            Assert.True(doc is Document);
            firstChild = (Document)doc;
            Assert.Equal(testFolder.Path + "/TestFile1", firstChild.Path);
            Assert.Equal("Test File 1", firstChild.Title);
        }

        public void CreateSecondChild()
        {
            Entity doc = client.Operation("Document.Create")
                               .SetInput("doc:" + testFolder.Path)
                               .SetParameter("type", "File")
                               .SetParameter("name", "TestFile2")
                               .SetParameter("properties", new ParamProperties { { "dc:title", "Test File 2" } })
                               .Execute()
                               .Result;
            Assert.NotNull(doc);
            Assert.True(doc is Document);
            secondChild = (Document)doc;
            Assert.Equal(testFolder.Path + "/TestFile2", secondChild.Path);
            Assert.Equal("Test File 2", secondChild.Title);
        }

        public void UpdateSecondChild()
        {
            Entity doc = client.Operation("Document.Update")
                               .SetInput("doc:" + secondChild.Path)
                               .SetParameter("properties", new ParamProperties { { "dc:description", "Simple File" },
                                                                                 { "dc:subjects", "art,sciences" } })
                               .SetParameter("save", "true")
                               .Execute()
                               .Result;
            Assert.NotNull(doc);
            Assert.True(doc is Document);
            secondChild = (Document)doc;
            Assert.NotNull(secondChild.Properties["dc:description"]);
            Assert.Equal("Simple File", secondChild.Properties["dc:description"].ToObject<string>());
            Assert.NotNull(secondChild.Properties["dc:subjects"]);
            Assert.Equal(2, secondChild.Properties["dc:subjects"].ToObject<JArray>().Count);
        }

        public void GetChildren()
        {
            Entity doc = (Documents)client.Operation("Document.GetChildren")
                                          .SetInput("doc:" + testFolder.Path)
                                          .Execute()
                                          .Result;
            Assert.NotNull(doc);
            Assert.True(doc is Documents);
            Documents documents = (Documents)doc;
            Assert.Equal(2, documents.Entries.Count);
        }

        public void DeleteParent()
        {
            Entity shouldBeNull = client.Operation("Document.Delete")
                                        .SetInput("doc:" + testFolder.Path)
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