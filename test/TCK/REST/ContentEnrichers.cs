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
    public class ContentEnrichers : IDisposable
    {
        private Client client;

        public ContentEnrichers()
        {
            client = new Client(Config.Instance.GetServerUrl());
            client.AddDefaultSchema("dublincore");
        }

        [Fact]
        public void TestContentEnrichers()
        {
            CreateFolder();
            TestThumbnailEnricher();
            TestAclEnricher();
            TestPreviewEnricher();
            TestBreadcrumbEnricher();
            DeleteFolder();
        }

        public void CreateFolder()
        {
            Document document = (Document)client.DocumentFromPath("/").Post(new Document
            {
                Type = "Folder",
                Name = "folder2",
                Properties = new Properties { { "dc:title", "A Folder 2" } }
            }).Result;
            Assert.NotNull(document);
            Assert.Equal("/folder2", document.Path);
            Assert.Equal("A Folder 2", document.Title);
        }

        public void TestThumbnailEnricher()
        {
            Document document = (Document)client.DocumentFromPath("/folder2")
                                            .AddContentEnricher("thumbnail")
                                            .Get().Result;
            Assert.NotNull(document);
            Assert.NotNull(document.ContextParameters);
            Assert.NotNull(document.ContextParameters["thumbnail"]);
            Assert.NotNull(document.ContextParameters["thumbnail"]["url"]);
        }

        public void TestAclEnricher()
        {
            Document document = (Document)client.DocumentFromPath("/folder2")
                                            .AddContentEnricher("acls")
                                            .Get().Result;
            Assert.NotNull(document);
            Assert.NotNull(document.ContextParameters);
            Assert.NotNull(document.ContextParameters["acls"]);
        }

        public void TestPreviewEnricher()
        {
            Document document = (Document)client.DocumentFromPath("/folder2")
                                            .AddContentEnricher("preview")
                                            .Get().Result;
            Assert.NotNull(document);
            Assert.NotNull(document.ContextParameters);
            Assert.NotNull(document.ContextParameters["preview"]);
            Assert.NotNull(document.ContextParameters["preview"]["url"]);
        }

        public void TestBreadcrumbEnricher()
        {
            Document document = (Document)client.DocumentFromPath("/folder2")
                                            .AddContentEnricher("breadcrumb")
                                            .Get().Result;
            Assert.NotNull(document);
            Assert.NotNull(document.ContextParameters);
            Assert.NotNull(document.ContextParameters["breadcrumb"]);
            Assert.NotNull(document.ContextParameters["breadcrumb"]["entity-type"]);
            Assert.Equal("documents", document.ContextParameters["breadcrumb"]["entity-type"].ToObject<string>());
        }

        public void DeleteFolder()
        {
            Document shouldBeNull = (Document)client.DocumentFromPath("/folder2").Delete().Result;
            Assert.Null(shouldBeNull);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}