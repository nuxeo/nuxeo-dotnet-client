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

        public CRUD()
        {
            client = new Client(Config.Instance.GetServerUrl());
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
            Document testFolder1 = (Document)client.Operation("Document.Create")
                                                   .SetInput("doc:/")
                                                   .SetParameter("type", "Folder")
                                                   .SetParameter("name", "TestFolder1")
                                                   .SetParameter("properties", new ParamProperties { { "dc:title", "Test Folder 1" } })
                                                   .Execute()
                                                   .Result;
            Assert.NotNull(testFolder1);
            Assert.Equal("/TestFolder1", testFolder1.Path);
            Assert.Equal("Test Folder 1", testFolder1.Title);
        }

        public void CreateFirstChild()
        {
            string parentPath = "/TestFolder1";
            Document firstChild = (Document)client.Operation("Document.Create")
                                                  .SetInput("doc:" + parentPath)
                                                  .SetParameter("type", "File")
                                                  .SetParameter("name", "TestFile1")
                                                  .SetParameter("properties", new ParamProperties { { "dc:title", "Test File 1" } })
                                                  .Execute()
                                                  .Result;
            Assert.NotNull(firstChild);
            Assert.Equal(parentPath + "/TestFile1", firstChild.Path);
            Assert.Equal("Test File 1", firstChild.Title);
        }

        public void CreateSecondChild()
        {
            string parentPath = "/TestFolder1";
            Document secondChild = (Document)client.Operation("Document.Create")
                                                   .SetInput("doc:" + parentPath)
                                                   .SetParameter("type", "File")
                                                   .SetParameter("name", "TestFile2")
                                                   .SetParameter("properties", new ParamProperties { { "dc:title", "Test File 2" } })
                                                   .Execute()
                                                   .Result;
            Assert.NotNull(secondChild);
            Assert.Equal(parentPath + "/TestFile2", secondChild.Path);
            Assert.Equal("Test File 2", secondChild.Title);
        }

        public void UpdateSecondChild()
        {
            string secondChildPath = "/TestFolder1/TestFile2"; ;
            Document secondChild = (Document)client.Operation("Document.Update")
                                                   .SetInput("doc:" + secondChildPath)
                                                   .SetParameter("properties", new ParamProperties { { "dc:description", "Simple File" },
                                                                                                     { "dc:subjects", "art,sciences" } })
                                                   .SetParameter("save", "true")
                                                   .Execute()
                                                   .Result;
            Assert.NotNull(secondChild);
            Assert.NotNull(secondChild.Properties["dc:description"]);
            Assert.Equal("Simple File", secondChild.Properties["dc:description"].ToObject<string>());
            Assert.NotNull(secondChild.Properties["dc:subjects"]);
            Assert.Equal(2, secondChild.Properties["dc:subjects"].ToObject<JArray>().Count);
        }

        public void GetChildren()
        {
            string parentPath = "/TestFolder1";
            Documents documents = (Documents)client.Operation("Document.GetChildren")
                                          .SetInput("doc:" + parentPath)
                                          .Execute()
                                          .Result;
            Assert.NotNull(documents);
            Assert.Equal(2, documents.Entries.Count);
        }

        public void DeleteParent()
        {
            string parentPath = "/TestFolder1";
            Entity shouldBeNull = client.Operation("Document.Delete")
                                       .SetInput("doc:" + parentPath)
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