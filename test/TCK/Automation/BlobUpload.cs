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
    public class BlobUpload : IDisposable
    {
        private Client client;

        public BlobUpload()
        {
            client = new Client(Config.Instance.GetServerUrl());
            client.AddDefaultSchema("dublincore");
        }

        [Fact]
        public void TestBlobUpload()
        {
            CreateFolderOnRoot();
            UploadBlobTxt();
            UploadBlobBin();
            VerifyChildren();
            CreateDocument();
            AttachBlob();
            DeleteParent();
        }

        public void CreateFolderOnRoot()
        {
            Document blobsFolder = (Document)client.Operation("Document.Create")
                                                   .SetInput("doc:/")
                                                   .SetParameter("type", "Folder")
                                                   .SetParameter("name", "TestBlobs")
                                                   .SetParameter("properties", new ParamProperties { { "dc:title", "Test Blobs" } })
                                                   .Execute()
                                                   .Result;
            Assert.NotNull(blobsFolder);
            Assert.Equal("/TestBlobs", blobsFolder.Path);
        }

        public void UploadBlobTxt()
        {
            Blob blob = new Blob("testText.txt").SetContent("Some content in play text.");
            Document document = (Document)client.Operation("FileManager.Import")
                                                .SetInput(blob)
                                                .SetContext("currentDocument", "/TestBlobs")
                                                .Execute()
                                                .Result;
            Assert.NotNull(document);
            Assert.Equal("Note", document.Type);
        }

        public void UploadBlobBin()
        {
            Blob blob = new Blob("testBin.dll").SetContent("Some content that should be binary.");
            Document document = (Document)client.Operation("FileManager.Import")
                                                .SetInput(blob)
                                                .SetContext("currentDocument", "/TestBlobs")
                                                .Execute()
                                                .Result;
            Assert.NotNull(document);
            Assert.Equal("File", document.Type);
        }

        public void VerifyChildren()
        {
            string parentPath = "/TestBlobs";
            Documents documents = (Documents)client.Operation("Document.GetChildren")
                                           .SetInput("doc:" + parentPath)
                                           .Execute()
                                           .Result;
            Assert.NotNull(documents);
            Assert.Equal(2, documents.Entries.Count);
        }

        public void CreateDocument()
        {
            string parentPath = "/TestBlobs";
            Document document = (Document)client.Operation("Document.Create")
                                                .SetInput("doc:" + parentPath)
                                                .SetParameter("type", "File")
                                                .SetParameter("name", "TestDoc")
                                                .SetParameter("properties", new ParamProperties { { "dc:title", "Document with Attachment" } })
                                                .Execute()
                                                .Result;
            Assert.NotNull(document);
            Assert.Equal(parentPath + "/TestDoc", document.Path);
            Assert.Equal("Document with Attachment", document.Title);
        }

        public void AttachBlob()
        {
            string parentPath = "/TestBlobs/TestDoc";
            Blob blob = new Blob("attachment.txt").SetContent("Just an attachment.");
            Entity content = client.Operation("Blob.Attach")
                                     .SetInput(blob)
                                     .SetParameter("document", parentPath)
                                     .SetParameter("save", "true")
                                     .SetParameter("xpath", "file:content")
                                     .Execute()
                                     .Result;
            Assert.Null(content);
        }

        public void DeleteParent()
        {
            string parentPath = "/TestBlobs";
            Entity shouldBeNull = client.Operation("Document.Delete")
                                       .SetInput(parentPath)
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