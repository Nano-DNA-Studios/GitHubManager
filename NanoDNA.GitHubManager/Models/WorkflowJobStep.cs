using Newtonsoft.Json;

namespace NanoDNA.GitHubManager.Models
{
    public class WorkflowJobStep
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("status")]
        public string Status { get; private set; }


        [JsonProperty("conclusion")]
        public string Conclusion { get; private set; }

        [JsonProperty("number")]
        public long Number { get; private set; }

        [JsonProperty("started_at")]
        public string StartedAt { get; private set; }

        [JsonProperty("completed_at")]
        public string CompletedAt { get; private set; }
    }
}
