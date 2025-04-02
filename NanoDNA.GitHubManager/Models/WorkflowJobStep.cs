using Newtonsoft.Json;

namespace NanoDNA.GitHubManager.Models
{
    /// <summary>
    /// Represents a Step within a GitHub Workflow Job
    /// </summary>
    public class WorkflowJobStep
    {
        /// <summary>
        /// Name of the Step
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Status of the Step (Queued, In Progress, Completed)
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; private set; }

        /// <summary>
        /// How the step concluded (Success, Failure, Cancelled)
        /// </summary>
        [JsonProperty("conclusion")]
        public string Conclusion { get; private set; }

        /// <summary>
        /// Index of the Step within the Job
        /// </summary>
        [JsonProperty("number")]
        public long Number { get; private set; }

        /// <summary>
        /// Time at which the Step Started
        /// </summary>
        [JsonProperty("started_at")]
        public string StartedAt { get; private set; }

        /// <summary>
        /// Time at which the Step Completed
        /// </summary>
        [JsonProperty("completed_at")]
        public string CompletedAt { get; private set; }
    }
}
