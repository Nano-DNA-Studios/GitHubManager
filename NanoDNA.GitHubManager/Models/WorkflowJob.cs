using Newtonsoft.Json;

namespace NanoDNA.GitHubManager.Models
{
    public class WorkflowJob
    {
        [JsonProperty("id")]
        public long ID { get; private set; }

        [JsonProperty("run_id")]
        public long RunID { get; private set; }

        [JsonProperty("run_url")]
        public string RunURL { get; private set; }

        [JsonProperty("run_attempt")]
        public long RunAttempt { get; private set; }

        [JsonProperty("node_id")]
        public string NodeID { get; private set; }

        [JsonProperty("head_sha")]
        public string HeadSHA { get; private set; }

        [JsonProperty("url")]
        public string URL { get; private set; }

        [JsonProperty("html_url")]
        public string HtmlURL { get; private set; }

        [JsonProperty("workflow_name")]
        public string WorkflowName { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("status")]
        public string Status { get; private set; }

        [JsonProperty("conclusion")]
        public string Conclusion { get; private set; }

        [JsonProperty("started_at")]
        public string StartedAt { get; private set; }

        [JsonProperty("completed_at")]
        public string CompletedAt { get; private set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; private set; }

        [JsonProperty("steps")]
        public WorkflowJobStep[] Steps { get; private set; }

        [JsonProperty("check_run_url")]
        public string CheckRunURL { get; private set; }

        [JsonProperty("labels")]
        public string[] Labels { get; private set; }

        [JsonProperty("runner_id")]
        public long RunnerID { get; private set; }

        [JsonProperty("runner_name")]
        public string RunnerName { get; private set; }

        [JsonProperty("runner_group_id")]
        public long RunnerGroupID { get; private set; }

        [JsonProperty("runner_group_name")]
        public string RunnerGroupName { get; private set; }
    }
}
