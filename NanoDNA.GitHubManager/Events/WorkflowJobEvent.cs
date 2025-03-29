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
        /// <summary>
        /// Event Type sent by GitHub
        /// </summary>
        public string EventType => "workflow_job";

        /// <summary>
        /// Workflow Dispatched by GitHub
        /// </summary>
        [JsonProperty("workflow_job")]
        public WorkflowRun Workflow { get; }


        /// <summary>
        /// Extra Data Associated with the Repository
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JToken> ExtraData { get; set; }
    }
}