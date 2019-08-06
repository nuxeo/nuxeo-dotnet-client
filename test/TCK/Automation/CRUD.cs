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
using System.Collections.Generic;
using Xunit;

namespace TCK.Automation
{
    public class CRUD : IDisposable
    {
        private Client client;

        private Document testFolder;

        private Document firstChild;

        private Document secondChild;

        private Document section;

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
            UpdateAllChildren();
            GetChildren();
            CreateSection();
            PublishSecondChild();
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
                               .SetInput(testFolder)
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
                               .SetInput(testFolder)
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
                               .SetInput(secondChild)
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

        public void UpdateAllChildren()
        {
            Entity output = client.Operation("Document.Update")
                                  .SetInput(new List<Document> { firstChild, secondChild })
                                  .SetParameter("properties", new ParamProperties { { "dc:subjects", "comics" } })
                                  .SetParameter("save", "true")
                                  .Execute()
                                  .Result;
            Assert.NotNull(output);
            Assert.True(output is Documents);
            Documents docs = (Documents)output;
            Assert.Equal(2, docs.Entries.Count);

            Document child = docs.Entries[0];
            Assert.Equal("Test File 1", child.Title);
            Assert.NotNull(child.Properties["dc:description"]);
            Assert.Null(child.Properties["dc:description"].ToObject<string>());
            Assert.NotNull(child.Properties["dc:subjects"]);
            JArray subjects = child.Properties["dc:subjects"].ToObject<JArray>();
            Assert.Equal(1, subjects.Count);
            Assert.Contains("comics", subjects);

            child = docs.Entries[1];
            Assert.Equal("Test File 2", child.Title);
            Assert.NotNull(child.Properties["dc:description"]);
            Assert.Equal("Simple File", child.Properties["dc:description"].ToObject<string>());
            Assert.NotNull(child.Properties["dc:subjects"]);
            subjects = child.Properties["dc:subjects"].ToObject<JArray>();
            Assert.Equal(1, subjects.Count);
            Assert.Contains("comics", subjects);
        }

        public void GetChildren()
        {
            Entity doc = (Documents)client.Operation("Document.GetChildren")
                                          .SetInput(testFolder)
                                          .Execute()
                                          .Result;
            Assert.NotNull(doc);
            Assert.True(doc is Documents);
            Documents documents = (Documents)doc;
            Assert.Equal(2, documents.Entries.Count);
        }


        public void CreateSection()
        {
            Entity doc = client.Operation("Document.Create")
                               .SetInput(testFolder)
                               .SetParameter("type", "Section")
                               .SetParameter("name", "section")
                               .SetParameter("properties", new ParamProperties { { "dc:title", "Section" } })
                               .Execute()
                               .Result;

            Assert.NotNull(doc);
            Assert.True(doc is Document);
            section = (Document)doc;
            Assert.Equal(testFolder.Uid, section.ParentRef);
            Assert.Equal("Section", section.Title);
        }

        public void PublishSecondChild()
        {
            Document doc = (Document)client.Operation("Document.PublishToSection")
                                           .SetInput(secondChild)
                                           .SetParameter("target", section)
                                           .Execute()
                                           .Result;
            Assert.NotNull(doc);
            Assert.Equal($"{section.Path}/TestFile2", doc.Path);
            Assert.True(doc.IsProxy);
        }

        public void DeleteParent()
        {
            Entity shouldBeNull = client.Operation("Document.Delete")
                                        .SetInput(testFolder)
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