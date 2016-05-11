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

using NuxeoClient;
using NuxeoClient.Adapters;
using NuxeoClient.Wrappers;
using System;
using Xunit;

namespace TCK.REST
{
    public class Adapters : IDisposable
    {
        private Client client;
        private Document folder;

        public Adapters()
        {
            client = new Client(Config.ServerUrl());
            client.AddDefaultSchema("dublincore");

            // populate
            folder = (Document)client.DocumentFromPath("/").Post(new Document
            {
                Type = "Folder",
                Name = "TestFolder4",
                Properties = new Properties { { "dc:title", "Adapter Tests" } }
            }).Result;
            Assert.NotNull(folder);

            Document document = (Document)client.DocumentFromPath(folder.Path).Post(new Document
            {
                Type = "File",
                Name = "TestFile1",
                Properties = new Properties { { "dc:title", "File 1" } }
            }).Result;
            Assert.NotNull(document);

            document = (Document)client.DocumentFromPath(folder.Path).Post(new Document
            {
                Type = "File",
                Name = "TestFIle2",
                Properties = new Properties { { "dc:title", "File 2" } }
            }).Result;
            Assert.NotNull(document);

            Blob blob = Blob.FromFile("Puppy.docx");
            document = (Document)client.Operation("FileManager.Import")
                                       .SetInput(blob)
                                       .SetContext("currentDocument", folder.Path)
                                       .Execute()
                                       .Result;
        }

        [Fact]
        public void TestAdapters()
        {
            TestChildrenAdapter();
            TestSearchAdapter();
        }

        public void TestChildrenAdapter()
        {
            Documents documents = (Documents)folder.SetAdapter(new ChildrenAdapter()).Get().Result;
            Assert.NotNull(documents);
            Assert.Equal(3, documents.Entries.Count);
        }

        public void TestSearchAdapter()
        {
            Adapter adapter = new SearchAdapter().SetSearchMode(SearchAdapter.SearchMode.NXQL)
                                                 .SetSearchQuery("SELECT * FROM File WHERE ecm:parentId = \"" + folder.Uid + "\"");
            Documents documents = (Documents)folder.SetAdapter(adapter).Get().Result;
            Assert.NotNull(documents);
            Assert.Equal(3, documents.Entries.Count);
        }

        public void TestBlobAdapter()
        {
            Adapter adapter = new BlobAdapter();
            Entity entity = folder.SetAdapter(adapter).Get().Result;
            Assert.NotNull(entity);
            Assert.True(entity is Blob);
            Assert.True(IOHelper.AreFilesEqual("Puppy.docx", ((Blob)entity).File.FullName));
        }

        public void Dispose()
        {
            client.DocumentFromPath(folder.Path).Delete().Wait();
            client.Dispose();
        }
    }
}