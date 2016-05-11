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
using System.Text;
using Xunit;

namespace TCK.Automation
{
    public class BlobUpload : IDisposable
    {
        private Client client;

        private Document blobsFolder;

        private Document blobContainer;

        public BlobUpload()
        {
            client = new Client(Config.ServerUrl());
            client.AddDefaultSchema("dublincore");

            // populate
            blobsFolder = (Document)client.Operation("Document.Create")
                                                   .SetInput("doc:/")
                                                   .SetParameter("type", "Folder")
                                                   .SetParameter("name", "TestBlobs")
                                                   .SetParameter("properties", new ParamProperties { { "dc:title", "Blob Operations" } })
                                                   .Execute()
                                                   .Result;

            blobContainer = (Document)client.Operation("Document.Create")
                                            .SetInput("doc:" + blobsFolder.Path)
                                            .SetParameter("type", "File")
                                            .SetParameter("name", "MyFileWithPics")
                                            .SetParameter("properties", new ParamProperties { { "dc:title", "My File With Pics" } })
                                            .Execute()
                                            .Result;
        }

        [Fact]
        public void TestBlobOperations()
        {
            UploadBlobTxt();
            UploadBlobBin();
            VerifyChildren();
            AttachBlobs();
            GetBlobs();
        }

        public void UploadBlobTxt()
        {
            Blob blob = new Blob(IOHelper.CreateTempFile("Some content in play text.")).SetFilename("testText.txt");
            Document document = (Document)client.Operation("FileManager.Import")
                                                .SetInput(blob)
                                                .SetContext("currentDocument", blobsFolder.Path)
                                                .Execute()
                                                .Result;
            Assert.NotNull(document);
            Assert.Equal("Note", document.Type);
            Assert.Equal("testText.txt", document.Title);
        }

        public void UploadBlobBin()
        {
            Blob blob = Blob.FromFile("Puppy.docx");
            Document document = (Document)client.Operation("FileManager.Import")
                                                .SetInput(blob)
                                                .SetContext("currentDocument", blobsFolder.Path)
                                                .Execute()
                                                .Result;
            Assert.NotNull(document);
            Assert.Equal("File", document.Type);
            Assert.Equal("Puppy.docx", document.Title);
        }

        public void VerifyChildren()
        {
            string parentPath = "/TestBlobs";
            Documents documents = (Documents)client.Operation("Document.GetChildren")
                                           .SetInput("doc:" + parentPath)
                                           .Execute()
                                           .Result;
            Assert.NotNull(documents);
            Assert.Equal(3, documents.Entries.Count);
        }

        public void AttachBlobs()
        {
            Blob blob = new Blob(IOHelper.CreateTempFile("This is just a note.")).SetFilename("note1.txt");
            Entity result = client.Operation("Blob.Attach")
                                  .SetInput(blob)
                                  .SetParameter("document", blobContainer.Path)
                                  .Execute()
                                  .Result;
            Assert.True(result is Blob);
            Assert.Equal("This is just a note.", IOHelper.ReadText(((Blob)result).File));

            BlobList blobs = new BlobList();
            blobs.Add(new Blob(IOHelper.CreateTempFile("This is another note.")).SetFilename("note2.txt"));
            blobs.Add(Blob.FromFile("Puppy.docx"));

            result = client.Operation("Blob.Attach")
                           .SetInput(blobs)
                           .SetParameter("document", blobContainer.Path)
                           .SetParameter("xpath", "files:files")
                           .Execute()
                           .Result;
            Assert.True(result is BlobList);
            Assert.Equal(2, blobs.Count);
            Assert.Equal("This is another note.", IOHelper.ReadText(blobs[0].File));
            Assert.True(IOHelper.AreFilesEqual("Puppy.docx", blobs[1].File.FullName));
        }

        public void GetBlobs()
        {
            Entity entity = client.Operation("Blob.Get")
                                  .SetInput("doc:" + blobContainer.Path)
                                  .Execute()
                                  .Result;
            Assert.True(entity is Blob);
            Assert.Equal("This is just a note.", IOHelper.ReadText(((Blob)entity).File));

            entity = client.Operation("Blob.GetList")
                           .SetInput("doc:" + blobContainer.Path)
                           .Execute()
                           .Result;
            Assert.True(entity is BlobList);
            BlobList blobs = (BlobList)entity;
            Assert.Equal(2, blobs.Count);
            Assert.Equal("This is another note.", IOHelper.ReadText(blobs[0].File));
            Assert.True(IOHelper.AreFilesEqual("Puppy.docx", blobs[1].File.FullName));
        }

        public void Dispose()
        {
            client.Operation("Document.Delete").SetInput("doc:" + blobContainer.ParentRef).Execute().Wait();
            client.Dispose();
        }
    }
}