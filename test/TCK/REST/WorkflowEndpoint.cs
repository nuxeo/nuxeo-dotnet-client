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
    public class WorkflowEndoint : IDisposable
    {
        private Client client;
        private Document document;
        private Workflow workFlow;
        private Task task;

        public WorkflowEndoint()
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
        public void TestParallelReview()
        {
            StartParallelReviewWorflow();
            GetCurrentTask();
            StartReview();
        }

        public void StartParallelReviewWorflow()
        {
            workFlow = (Workflow)client.Request(Client.RequestType.POST, "workflow", data: new JObject
            {
                { "entity-type", "workflow" },
                { "workflowModelName", "ParallelDocumentReview" },
                { "attachedDocumentIds", new JArray { document.Uid } }
            }).Result;

            Assert.NotNull(workFlow);
            Assert.Equal("wf.parallelDocumentReview.ParallelDocumentReview", workFlow.Title);
            Assert.Equal("running", workFlow.State);
            Assert.NotNull(workFlow.WorflowVariables);
        }

        public void GetCurrentTask()
        {
            Tasks tasks = (Tasks)client.Request(Client.RequestType.GET, "task", new QueryParams
            {
                { "workflowInstanceId", workFlow.Id }
            }).Result;
            Assert.NotNull(tasks);
            Assert.Equal(1, tasks.Entries.Count);
            task = tasks.Entries[0];
        }

        public void StartReview()
        {
            Task updatedTask = new Task
            {
                Id = task.Id,
                Comment = "a comment",
                Variables = new Properties
                {
                    { "end_date", DateTime.Now.Date.ToString("yyyy-MM-dd") },
                    { "participants", new JArray { "user:Administrator" } },
                    { "assignees", new JArray { "user:Administrator" } }
                }
            };
            task = (Task)client.Request(Client.RequestType.PUT, "task/" + task.Id + "/start_review", data: client.Marshaller.Marshal(updatedTask)).Result;
            Assert.NotNull(task);
        }

        public void Dispose()
        {
            document.SetAdapter(null).Delete().Wait();
            client.Dispose();
        }
    }
}