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
using System.Text;
using Xunit;

namespace TCK.REST
{
    public class BatchUpload : IDisposable
    {
        private Client client;

        private Batch normalBatch;

        private Batch chunkedBatch;

        private Uploader uploader;

        private Document testFolder;

        public BatchUpload()
        {
            client = new Client(Config.ServerUrl());
            client.AddDefaultSchema("*");

            // populate
            testFolder = (Document)client.DocumentFromPath("/").Post(new Document
            {
                Type = "Folder",
                Name = "TestFolder3",
                Properties = new Properties { { "dc:title", "Upload Test Folder" } }
            }).Result;
        }

        [Fact]
        public void TestBatchUpload()
        {
            HandShake();
            UploadFile();
            UploadAnotherFile();
            GetBachInfo();
            GetBatchFileInfo();
            DropBatch();
            AnotherHandShake();
            UploadFileChuncked();
            CreateDocumentFromUpload();
            BatchUploadWithUploader();
            BatchOperation();
        }

        public void HandShake()
        {
            normalBatch = client.Batch().Result;
            Assert.NotNull(normalBatch);
            Assert.NotNull(normalBatch.BatchId);
        }

        public void UploadFile()
        {
            Blob blob = new Blob(IOHelper.CreateTempFile("Just the content.")).SetFilename("myFile.doc");
            UploadJob job = new UploadJob(blob);
            normalBatch = normalBatch.Upload(job).Result;
            Assert.NotNull(normalBatch);
            Assert.NotNull(normalBatch.FileIndex);
            Assert.NotNull(normalBatch.UploadType);
            Assert.NotNull(normalBatch.UploadSize);
            Assert.Equal(0, normalBatch.FileIndex);
            Assert.Equal("normal", normalBatch.UploadType);
            Assert.Equal(17, normalBatch.UploadSize);
        }

        public void UploadAnotherFile()
        {
            // test uploading a second with name that needs escaping
            Blob blob = new Blob(IOHelper.CreateTempFile("Yet more content.")).SetFilename("行动计划 + test.pdf");
            UploadJob job = new UploadJob(blob).SetFileId(1);
            normalBatch = normalBatch.Upload(job).Result;
            Assert.NotNull(normalBatch);
            Assert.NotNull(normalBatch.FileIndex);
            Assert.NotNull(normalBatch.UploadType);
            Assert.NotNull(normalBatch.UploadSize);
            Assert.Equal(1, normalBatch.FileIndex);
            Assert.Equal("normal", normalBatch.UploadType);
            Assert.Equal(17, normalBatch.UploadSize);
        }

        public void GetBachInfo()
        {
            EntityList<Entity> files = normalBatch.Info().Result;
            Assert.NotNull(files);
            Assert.Equal(2, files.Count);
            Assert.Equal("myFile.doc", ((BatchFile)files[0]).Name);
            Assert.Equal("行动计划 + test.pdf", ((BatchFile)files[1]).Name);
        }

        public void GetBatchFileInfo()
        {
            BatchFile file = normalBatch.Info(0).Result;
            Assert.NotNull(file);
            Assert.Equal("myFile.doc", file.Name);
        }

        public void DropBatch()
        {
            BatchInfo batchInfo = normalBatch.Drop().Result;
            // in 7.10 a {"batchId": batchId, "dropped": "true"} is returned
            // from 7.10HF3 and 8.1 onwards a 204 is returned instead, which is translated into null
            Assert.True(batchInfo == null/* || batchInfo.Dropped*/);
        }

        public void AnotherHandShake()
        {
            chunkedBatch = client.Batch().Result;
            Assert.NotNull(normalBatch);
            Assert.NotNull(normalBatch.BatchId);
        }

        public void UploadFileChuncked()
        {
            Blob blob = new Blob(IOHelper.CreateTempFile("This content is chunked. Seriously, really chunked!"))
                .SetFilename("chunked.txt").SetMimeType("text/plain");
            int chunkSize = (int)Math.Ceiling((double)blob.File.Length / 5);
            UploadJob job = new UploadJob(blob);
            job.SetChunked(true);
            job.SetChunkSize(chunkSize);
            chunkedBatch = chunkedBatch.Upload(job).Result;
            BatchFile info = chunkedBatch.Info(chunkedBatch.FileIndex).Result;
            Assert.NotNull(info);
            Assert.Equal(5, info.ChunkCount);
            Assert.Equal(5, info.UploadedChunkIds.Length);
        }

        public void CreateDocumentFromUpload()
        {
            Document document = (Document)client.DocumentFromPath(testFolder.Path).Post(new Document
            {
                Type = "File",
                Name = "TestDocument",
                Properties = new Properties
                                {
                                    { "dc:title", "Upload Test File"},
                                    { "file:content", new JObject
                                                        {
                                                            { "upload-batch", chunkedBatch.BatchId },
                                                            { "upload-fileId", chunkedBatch.FileIndex.ToString() }
                                                        }
                                    }
                                }
            }).Result;
            Assert.NotNull(document);
            Assert.Equal("text/plain", (string)document.Properties["file:content"]["mime-type"]);
        }

        public void BatchUploadWithUploader()
        {
            uploader = client.Uploader().SetChunked(true).SetChunkSize(1024); // 1KB = 1024B
            Entity result = uploader.AddFile("Test.txt")
                                      .AddFile("Puppy.docx")
                                      .UploadFiles().Result;
            Assert.NotNull(result);
            Assert.True(result is EntityList<Entity>);
            Assert.Equal(2, ((EntityList<Entity>)result).Count);
        }

        public void BatchOperation()
        {
            Documents documents = (Documents)uploader.Operation("FileManager.Import")
                                                     .SetContext("currentDocument", "/TestFolder3")
                                                     .Execute().Result;
            Assert.NotNull(documents);
            Assert.Equal(2, documents.Entries.Count);
        }

        public void Dispose()
        {
            client.DocumentFromPath("/TestFolder3").Delete().Wait();
            client.Dispose();
        }
    }
}