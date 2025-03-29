using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace NanoDNA.GitHubManager.Events
{
    public class WorkflowRunEvent : IGitHubEvent
    {
        public string EventType => "workflow_run";

        [JsonProperty("workflow_run")]
        public WorkflowRun WorkflowRun { get; set; }

        /// <summary>
        /// Extra Data Associated with the Repository
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JToken> ExtraData { get; set; }
    }
}