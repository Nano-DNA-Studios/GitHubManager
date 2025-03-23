using Newtonsoft.Json;

namespace NanoDNA.GitHubActionsManager
{
    /// <summary>
    /// Describes a GitHub Runner Label
    /// </summary>
    public class RunnerLabel
    {
        /// <summary>
        /// ID of the Label
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// Name of the Label
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Type of the Label (read-only, etc.)
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        internal RunnerLabel(string name)
        {
            ID = 0;
            Name = Name;
            Type = string.Empty;
        }
    }
}
