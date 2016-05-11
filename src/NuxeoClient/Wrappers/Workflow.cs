/*
 * (C) Copyright 2015-2016 Nuxeo SA (http://nuxeo.com/) and others.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Contributors:
 *     Gabriel Barata <gbarata@nuxeo.com>
 */

using Newtonsoft.Json;
using System.ComponentModel;

namespace NuxeoClient.Wrappers
{
    /// <summary>
    /// Represents a Workflow entity.
    /// </summary>
    /// <remarks>For more information about workflows, check Nuxeo Documentation Center
    /// <a href="https://doc.nuxeo.com/display/NXDOC/Workflow">here</a> and
    /// <a href="https://doc.nuxeo.com/display/USERDOC/Workflows">here</a>.
    /// </remarks>
    public class Workflow : Entity
    {
        /// <summary>
        /// Gets and sets the workflow's model name.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "workflowModelName")]
        public string WorkflowModelName { get; set; } = string.Empty;

        /// <summary>
        /// Gets and sets the ids of the documents attached to this workflow.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty(PropertyName = "attachedDocumentIds")]
        public dynamic AttachedDocumentIds { get; set; } = null;

        /// <summary>
        /// Gets and sets the workflow variables.
        /// </summary>
        [DefaultValue(null)]
        [JsonProperty(PropertyName = "worflowVariables")]
        public Properties WorflowVariables { get; set; } = null;

        /// <summary>
        /// Gets and sets the workflow's Graph Resource.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "graphResource")]
        public string GraphResource { get; set; } = string.Empty;

        /// <summary>
        /// Gets and sets the workflow's ID.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets and sets the worflow's initiator.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "initiator")]
        public string Initiator { get; set; } = string.Empty;

        /// <summary>
        /// Gets and sets the worflow's name.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets and sets the worflow's state.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Gets and sets the worflow's title.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Initializes a new instance of <see cref="Workflow"/>.
        /// </summary>
        public Workflow()
        {
            EntityType = "workflow";
        }
    }
}