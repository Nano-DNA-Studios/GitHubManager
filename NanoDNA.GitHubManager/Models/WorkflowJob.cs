using Newtonsoft.Json;

namespace NanoDNA.GitHubManager.Models
{
    /// <summary>
    /// Represents a Workflow Job from the GitHub API
    /// </summary>
    public class WorkflowJob
    {
        /// <summary>
        /// ID of the Workflow Job
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; private set; }

        /// <summary>
        /// Workflow Run ID
        /// </summary>
        [JsonProperty("run_id")]
        public long RunID { get; private set; }

        /// <summary>
        /// Workflow Run URL
        /// </summary>
        [JsonProperty("run_url")]
        public string RunURL { get; private set; }

        /// <summary>
        /// Attempt number of the Workflow Run
        /// </summary>
        [JsonProperty("run_attempt")]
        public long RunAttempt { get; private set; }

        /// <summary>
        /// Node ID of the Workflow Job
        /// </summary>
        [JsonProperty("node_id")]
        public string NodeID { get; private set; }

        /// <summary>
        /// Head SHA of the Workflow Job
        /// </summary>
        [JsonProperty("head_sha")]
        public string HeadSHA { get; private set; }

        /// <summary>
        /// API URL of the Workflow Job
        /// </summary>
        [JsonProperty("url")]
        public string URL { get; private set; }

        /// <summary>
        /// HTML URL of the Workflow Job
        /// </summary>
        [JsonProperty("html_url")]
        public string HtmlURL { get; private set; }

        /// <summary>
        /// Name of the Workflow Run the Job is associated with
        /// </summary>
        [JsonProperty("workflow_name")]
        public string WorkflowName { get; private set; }

        /// <summary>
        /// Name of the Workflow Job
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Status of the Workflow Job (Queued, In Progress, Completed)
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; private set; }

        /// <summary>
        /// Conclusion of the Workflow Job (Success, Failure, Cancelled)
        /// </summary>
        [JsonProperty("conclusion")]
        public string Conclusion { get; private set; }

        /// <summary>
        /// Start Time of the Workflow Job
        /// </summary>
        [JsonProperty("started_at")]
        public string StartedAt { get; private set; }

        /// <summary>
        /// Completion Time of the Workflow Job
        /// </summary>
        [JsonProperty("completed_at")]
        public string CompletedAt { get; private set; }

        /// <summary>
        /// Creation Time of the Workflow Job
        /// </summary>
        [JsonProperty("created_at")]
        public string CreatedAt { get; private set; }

        /// <summary>
        /// Steps within the Workflow Job
        /// </summary>
        [JsonProperty("steps")]
        public WorkflowJobStep[] Steps { get; private set; }

        /// <summary>
        /// URL to request Checkup info about the run such as Status and Conclusion
        /// </summary>
        [JsonProperty("check_run_url")]
        public string CheckRunURL { get; private set; }

        /// <summary>
        /// Labels associated with the Workflow Job
        /// </summary>
        [JsonProperty("labels")]
        public string[] Labels { get; private set; }

        /// <summary>
        /// ID of the Runner that executed the Workflow Job
        /// </summary>
        [JsonProperty("runner_id")]
        public long RunnerID { get; private set; }

        /// <summary>
        /// Name of the Runner that executed the Workflow Job
        /// </summary>
        [JsonProperty("runner_name")]
        public string RunnerName { get; private set; }

        /// <summary>
        /// Group ID of the Runner that executed the Workflow Job
        /// </summary>
        [JsonProperty("runner_group_id")]
        public long RunnerGroupID { get; private set; }

        /// <summary>
        /// Group Name of the Runner that executed the Workflow Job
        /// </summary>
        [JsonProperty("runner_group_name")]
        public string RunnerGroupName { get; private set; }
    }
}
