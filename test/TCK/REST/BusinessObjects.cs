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
using TCK.REST.BO;
using Xunit;

namespace TCK.REST
{
    public class BusinessObjects : IDisposable
    {
        private Client client;
        private Document document;
        private BusinessBeanAdapter adapter;
        private BusinessBean note;

        public BusinessObjects()
        {
            client = new Client(Config.ServerUrl());
            client.AddDefaultSchema("dublincore");
        }

        [Fact]
        public void TestBusinessObjects()
        {
            CreateFile();
            CreateBO();
            UpdateBO();
            DeleteFile();
        }

        public void CreateFile()
        {
            document = (Document)client.DocumentFromPath("/").Post(new Document
            {
                Type = "Folder",
                Name = "bofolder",
                Properties = new Properties { { "dc:title", "Just a folder" } }
            }).Result;
            Assert.NotNull(document);
            Assert.Equal("/bofolder", document.Path);
            Assert.Equal("Just a folder", document.Title);
        }

        public void CreateBO()
        {
            adapter = new BusinessBeanAdapter();
            client.Marshaller.RegisterBO(adapter.Type, typeof(BusinessBean));
            note = new BusinessBean().SetType("Note")
                                                  .SetTitle("Note")
                                                  .SetNote("Note content");
            note = (BusinessBean)document.SetAdapter(adapter).Post(note.SetName("Note")).Result;
            Assert.NotNull(note);
            Assert.Equal("Note content", note.Note);
        }

        public void UpdateBO()
        {
            note = (BusinessBean)document.Put(note.SetDescription("My new description")).Result;
            Assert.NotNull(note);
            Assert.Equal("My new description", note.Description);
        }

        public void DeleteFile()
        {
            Document result = (Document)document.SetAdapter(null).Delete().Result;
            Assert.Null(result);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}