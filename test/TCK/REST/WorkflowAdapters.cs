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
        private Document node;
        private Task task;

        public WorkflowAdapters()
        {
            client = new Client(Config.Instance.GetServerUrl());
            client.AddDefaultSchema("dublincore");
        }

        [Fact]
        public void TestSerialReview()
        {
            CreateDocument();
            StartSerialReviewWorkflow();
            GetTaskFromWorkflow();
            GetTaskFromDocument();
            CompleteTask();
            DeleteDocument();
        }

        public void CreateDocument()
        {
            document = (Document)client.DocumentFromPath("/").Post(new Document
            {
                Type = "File",
                Name = "JustAfile",
                Properties = new Properties { { "dc:title", "Just a file" } }
            }).Result;
            Assert.NotNull(document);
        }

        public void StartSerialReviewWorkflow()
        {
            node = (Document)document.SetAdapter(new NuxeoClient.Adapters.WorkflowAdapter()).Post(new Workflow
            {
                WorkflowModelName = "SerialDocumentReview",
                AttachedDocumentIds = new JArray() { document.Uid }
            }).Result;

            Assert.NotNull(node);
            Assert.Equal("wf.serialDocumentReview.SerialDocumentReview", node.Title);
            Assert.Equal("running", node.State);
        }

        public void GetTaskFromWorkflow()
        {
            Tasks tasks = (Tasks)document.Get("/" + node.Uid + "/task").Result;
            Assert.NotNull(tasks);
            Assert.Equal(1, tasks.Entries.Count);
            task = tasks.Entries[0];
        }

        public void GetTaskFromDocument()
        {
            Tasks tasks = (Tasks)document.SetAdapter(new TaskAdapter()).Get().Result;
            Assert.NotNull(tasks);
            Assert.Equal(1, tasks.Entries.Count);
            Assert.Equal(task.Id, tasks.Entries[0].Id);
        }

        public void CompleteTask()
        {
            Task completedTask = (Task)document.Put(new Task
            {
                Id = task.Id,
                Comment = "a comment",
                Variables = new Properties
                {
                    { "end_date", DateTime.Now.Date.ToString("yyyy-MM-dd") },
                    { "participants", "[\"user:Administrator\"]" },
                    { "assignees", "[\"user:Administrator\"]" }
                }
            }, "/" + task.Id + "/start_review").Result;
            Assert.NotNull(completedTask);
        }

        public void DeleteDocument()
        {
            Entity result = document.SetAdapter(null).Delete().Result;
            Assert.Null(result);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}