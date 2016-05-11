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

namespace TCK.Automation
{
    public class Pagination : IDisposable
    {
        private Client client;

        private Document paginationRoot;

        public Pagination()
        {
            client = new Client(Config.ServerUrl());
            client.AddDefaultSchema("dublincore");

            // populate
            paginationRoot = (Document)client.Operation("Document.Create")
                                                   .SetInput("doc:/")
                                                   .SetParameter("type", "Folder")
                                                   .SetParameter("name", "TestFolder2")
                                                   .SetParameter("properties", new ParamProperties { { "dc:title", "Test Folder 2" } })
                                                   .Execute()
                                                   .Result;

            CreateChild("1");
            CreateChild("2");
            CreateChild("3");
        }

        private void CreateChild(string id)
        {
            Document child = (Document)client.Operation("Document.Create")
                                             .SetInput("doc:" + paginationRoot.Path)
                                             .SetParameter("type", "File")
                                             .SetParameter("name", "TestFile" + id)
                                             .SetParameter("properties", new ParamProperties { { "dc:title", "Test File " + id } })
                                             .Execute()
                                             .Result;
        }

        [Fact]
        public void TestPagination()
        {
            QueryPage1();
            QueryPage2();
        }

        public void QueryPage1()
        {
            Pageable page = (Pageable)client.Operation("Document.PageProvider")
                                            .SetParameter("query", "select * from Document where ecm:parentId = ?")
                                            .SetParameter("pageSize", "2")
                                            .SetParameter("page", "0")
                                            .SetParameter("queryParams", paginationRoot.Uid)
                                            .Execute()
                                            .Result;
            Assert.NotNull(page);
            Assert.Equal(2, page.PageSize);
            Assert.Equal(2, page.NumberOfPages);
            Assert.Equal(3, page.TotalSize);
            Assert.Equal(2, page.Entries.Count);
        }

        public void QueryPage2()
        {
            Pageable page = (Pageable)client.Operation("Document.PageProvider")
                                    .SetParameter("query", "select * from Document where ecm:parentId = ?")
                                    .SetParameter("pageSize", "2")
                                    .SetParameter("page", "1")
                                    .SetParameter("queryParams", paginationRoot.Uid)
                                    .Execute()
                                    .Result;
            Assert.NotNull(page);
            Assert.Equal(2, page.PageSize);
            Assert.Equal(2, page.NumberOfPages);
            Assert.Equal(3, page.TotalSize);
            Assert.Equal(1, page.Entries.Count);
        }

        public void Dispose()
        {
            client.Operation("Document.Delete")
                  .SetInput("doc:" + paginationRoot.Path)
                  .Execute()
                  .Wait();
            client.Dispose();
        }
    }
}