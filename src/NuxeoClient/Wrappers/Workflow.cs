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