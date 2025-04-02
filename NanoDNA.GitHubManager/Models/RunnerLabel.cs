using Newtonsoft.Json;

namespace NanoDNA.GitHubManager.Models
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
        /// Name / Value of the Label
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Type of the Label (read-only, etc.)
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Initializes a new instance of a Runner Label
        /// </summary>
        /// <param name="name">Name of the Label</param>
        public RunnerLabel(string name)
        {
            ID = 0;
            Name = name;
            Type = string.Empty;
        }
    }
}
