using NanoDNA.GitHubManager.Interfaces;
using NanoDNA.GitHubManager.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace NanoDNA.GitHubManager.Events
{
    /// <summary>
    /// Represents a new Workflow Job Event on GitHub
    /// </summary>
    public class WorkflowJobEvent : IGitHubEvent
    {
        /// <inheritdoc/>
        public string EventType => "workflow_job";

        /// <summary>
        /// Workflow Dispatched by GitHub
        /// </summary>
        [JsonProperty("workflow_job")]
        public WorkflowRun Workflow { get; }

        /// <summary>
        /// Extra Data Associated with the Workflow Job Event
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JToken> ExtraData { get; set; }
    }
}