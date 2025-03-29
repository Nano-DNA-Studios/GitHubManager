using NanoDNA.GitHubManager.Interfaces;
using NanoDNA.GitHubManager.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace NanoDNA.GitHubManager.Events
{
    /// <summary>
    /// Represents a new Workflow Run Event on GitHub
    /// </summary>
    public class WorkflowRunEvent : IGitHubEvent
    {
        /// <inheritdoc/>
        public string EventType => "workflow_run";

        /// <summary>
        /// Workflow Run Dispatched by GitHub
        /// </summary>
        [JsonProperty("workflow_run")]
        public WorkflowRun WorkflowRun { get; set; }

        /// <summary>
        /// Extra Data Associated with the Workflow Run Event
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JToken> ExtraData { get; set; }
    }
}