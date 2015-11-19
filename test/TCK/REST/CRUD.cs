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
using NuxeoClient.Wrappers;
using System;
using Xunit;

namespace TCK.REST
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
        public void TestRestCRUD()
        {
            FetchDomain();
            FetchNonExistent();
            CreateFolder();
            UpdateFolder();
            SetProperties();
            SetAndSave();
            DeleteFolder();
        }

        public void FetchDomain()
        {
            Document document = (Document)client.DocumentFromPath("/default-domain").Get().Result;
            Assert.NotNull(document);
            Assert.Equal("/default-domain", document.Path);
        }

        public void FetchNonExistent()
        {
            Document document = (Document)client.DocumentFromPath("/non-existing").Get().Result;
            Assert.Null(document);
        }

        public void CreateFolder()
        {
            Document document = (Document)client.DocumentFromPath("/").Post(new Document
            {
                Type = "Folder",
                Name = "folder",
                Properties = new Properties { { "dc:title", "A Folder" } }
            }).Result;
            Assert.NotNull(document);
            Assert.Equal("/folder", document.Path);
            Assert.Equal("A Folder", document.Title);
        }

        public void UpdateFolder()
        {
            Document document = (Document)client.DocumentFromPath("/folder").Put(new Document
            {
                Properties = new Properties { { "dc:title", "new title" } }
            }).Result;
            Assert.NotNull(document);
            Assert.Equal("new title", document.Title);
        }

        public void DeleteFolder()
        {
            Document shouldBeNull = (Document)client.DocumentFromPath("/folder").Delete().Result;
            Assert.Null(shouldBeNull);
        }

        public void SetProperties()
        {
            Document document = (Document)client.DocumentFromPath("/folder").Get().Result;
            Assert.NotNull(document);
            string newSourceValue = "new-source-value-" + DateTime.Now;
            document.Set("dc:source", newSourceValue);
            Assert.NotNull(document.DirtyProperties["dc:source"]);
            Assert.Equal(newSourceValue, document.DirtyProperties["dc:source"].ToObject<string>());
        }

        public void SetAndSave()
        {
            Document document = (Document)client.DocumentFromPath("/folder").Get().Result;
            Assert.NotNull(document);
            string newSourceValue = "new-source-value-" + DateTime.Now;
            document.Set("dc:source", newSourceValue);
            document = (Document)document.Save().Result;
            Assert.NotNull(document);
            Assert.NotNull(document.Properties["dc:source"]);
            Assert.Equal(newSourceValue, document.Properties["dc:source"].ToObject<string>());
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}