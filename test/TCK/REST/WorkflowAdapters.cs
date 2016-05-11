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
using NuxeoClient.Adapters;
using NuxeoClient.Wrappers;
using System;
using Xunit;

namespace TCK.REST
{
    public class WorkflowAdapters : IDisposable
    {
        private Client client;
        private Document document;
        private Workflow node;
        private Task task;

        public WorkflowAdapters()
        {
            client = new Client(Config.ServerUrl());
            client.AddDefaultSchema("dublincore");

            // populate
            document = (Document)client.DocumentFromPath("/").Post(new Document
            {
                Type = "File",
                Name = "JustAfile",
                Properties = new Properties { { "dc:title", "Just a file" } }
            }).Result;
        }

        [Fact]
        public void TestSerialReview()
        {
            StartSerialReviewWorkflow();
            GetTaskFromWorkflow();
            GetTaskFromDocument();
            CompleteTask();
        }

        public void StartSerialReviewWorkflow()
        {
            Entity entity = document.SetAdapter(new WorkflowAdapter()).Post(new Workflow
            {
                WorkflowModelName = "SerialDocumentReview",
                AttachedDocumentIds = new JArray() { document.Uid }
            }).Result;

            Assert.NotNull(entity);
            // in 7.10 a document of entity-type = "document" is returned
            // from 7.10HF06 and 8.2 onwards "task" is returned instead
            Assert.IsType<Workflow>(entity);
            node = (Workflow)entity;
            Assert.Equal("wf.serialDocumentReview.SerialDocumentReview", node.Title);
            Assert.Equal("running", node.State);
        }

        public void GetTaskFromWorkflow()
        {
            Entity entity = document.Get("/" + node.Id + "/task").Result;
            Assert.NotNull(entity);
            Assert.IsType<Tasks>(entity);
            Tasks tasks = (Tasks)entity;
            Assert.Equal(1, tasks.Entries.Count);
            task = tasks.Entries[0];
        }

        public void GetTaskFromDocument()
        {
            Entity entity = document.SetAdapter(new TaskAdapter()).Get().Result;
            Assert.NotNull(entity);
            Assert.IsType<Tasks>(entity);
            Tasks tasks = (Tasks)entity;
            Assert.Equal(1, tasks.Entries.Count);
            Assert.Equal(task.Id, tasks.Entries[0].Id);
        }

        public void CompleteTask()
        {
            Entity entity = document.Put(new Task
            {
                Id = task.Id,
                Comment = "a comment",
                Variables = new Properties
                {
                    { "end_date", DateTime.Now.Date.ToString("yyyy-MM-dd") },
                    { "participants", new JArray { "\"user:Administrator\"" } },
                    { "assignees", new JArray { "\"user:Administrator\"" } }
                }
            }, "/" + task.Id + "/start_review").Result;
            Assert.NotNull(entity);
            Assert.IsType<Task>(entity);
            Assert.Equal(((Task)entity).State, "ended");
        }

        public void Dispose()
        {
            document.SetAdapter(null).Delete().Wait();
            client.Dispose();
        }
    }
}