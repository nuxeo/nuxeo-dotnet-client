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

namespace TCK.REST
{
    public class BatchUpload : IDisposable
    {
        private Client client;
        private Batch normalBatch;
        private Batch chunkedBatch;
        private Uploader uploader;

        public BatchUpload()
        {
            client = new Client(Config.ServerUrl());
            client.AddDefaultSchema("*");
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
            BatchOperation(); // on 7.10 only
            DeleteTestFolder();
        }

        public void HandShake()
        {
            normalBatch = client.Batch().Result;
            Assert.NotNull(normalBatch);
            Assert.NotNull(normalBatch.BatchId);
        }

        public void UploadFile()
        {
            Blob blob = new Blob("myFile.doc").SetContent("Just the content.");
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
            Blob blob = new Blob("anoterFile.pdf").SetContent("Yet more content.");
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
            EntityList files = normalBatch.Info().Result;
            Assert.NotNull(files);
            Assert.Equal(2, files.Count);
            Assert.Equal("myFile.doc", ((BatchFile)files[0]).Name);
            Assert.Equal("anoterFile.pdf", ((BatchFile)files[1]).Name);
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
            Assert.NotNull(batchInfo);
            Assert.True(batchInfo.Dropped);
        }

        public void AnotherHandShake()
        {
            chunkedBatch = client.Batch().Result;
            Assert.NotNull(normalBatch);
            Assert.NotNull(normalBatch.BatchId);
        }

        public void UploadFileChuncked()
        {
            Blob blob = new Blob("chunked.docx").SetContent("This content is chunked. Seriously, really chunked!");
            int nChunks = 5;
            Blob[] chunks = blob.Split(nChunks);
            UploadJob job;
            for (int i = 0; i < nChunks; i++)
            {
                job = new UploadJob(chunks[i]).SetChunkIndex(i).SetChunkCount(nChunks);
                chunkedBatch = chunkedBatch.UploadChunk(job).Result;
                Assert.NotNull(chunkedBatch);
                Assert.NotNull(chunkedBatch.FileIndex);
                Assert.NotNull(chunkedBatch.UploadType);
                Assert.NotNull(chunkedBatch.UploadSize);
                Assert.Equal(0, chunkedBatch.FileIndex);
                Assert.Equal("chunked", chunkedBatch.UploadType);
                Assert.Equal(nChunks, chunkedBatch.ChunkCount);
            }
            BatchFile info = chunkedBatch.Info(chunkedBatch.FileIndex).Result;
            Assert.NotNull(info);
            Assert.Equal(5, info.ChunkCount);
            Assert.Equal(5, info.UploadedChunkIds.Length);
        }

        public void CreateDocumentFromUpload()
        {
            Document testFolder = (Document)client.DocumentFromPath("/").Post(new Document
            {
                Type = "Folder",
                Name = "TestFolder3",
                Properties = new Properties { { "dc:title", "Upload Test Folder" } }
            }).Result;
            Assert.NotNull(testFolder);

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
        }

        public void BatchUploadWithUploader()
        {
            uploader = client.Uploader().SetChunked(true).SetChunkSize(1024); // 1KB = 1024B
            Entity result = uploader.AddFile("Test.txt")
                                      .AddFile("Puppy.docx")
                                      .UploadFiles().Result;
            Assert.NotNull(result);
            Assert.True(result is EntityList);
            Assert.Equal(2, ((EntityList)result).Count);
        }

        public void BatchOperation()
        {
            Documents documents = (Documents)uploader.Operation("FileManager.Import")
                                                     .SetContext("currentDocument", "/TestFolder3")
                                                     .Execute().Result;
            Assert.NotNull(documents);
            Assert.Equal(2, documents.Entries.Count);
        }

        public void DeleteTestFolder()
        {
            Document shouldBeNull = (Document)client.DocumentFromPath("/TestFolder3").Delete().Result;
            Assert.Null(shouldBeNull);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}