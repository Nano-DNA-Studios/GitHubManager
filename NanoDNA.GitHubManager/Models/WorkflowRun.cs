using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace NanoDNA.GitHubManager.Models
{
    /// <summary>
    /// Represents a Workflow Run on GitHub
    /// </summary>
    public class WorkflowRun : GitHubAPIClient
    {
        /// <summary>
        /// ID of the Workflow Run
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; private set; }

        /// <summary>
        /// Name of the Workflow being run
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Event that Triggered the Workflow
        /// </summary>
        [JsonProperty("event")]
        public string Event { get; private set; }

        /// <summary>
        /// Status of the Workflow Run (Queued, In Progress, Completed)
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; private set; }

        /// <summary>
        /// Ending Conclusion of the Workflow Run (Success, Failure, Cancelled)
        /// </summary>
        [JsonProperty("conclusion")]
        public string Conclusion { get; private set; }

        /// <summary>
        /// ID of the Workflow File that was Run
        /// </summary>
        [JsonProperty("workflow_id")]
        public long WorkflowID { get; private set; }

        /// <summary>
        /// API URL of the Workflow Run
        /// </summary>
        [JsonProperty("url")]
        public string URL { get; private set; }

        /// <summary>
        /// HTML URL of the Workflow Run
        /// </summary>
        [JsonProperty("html_url")]
        public string HtmlURL { get; private set; }

        /// <summary>
        /// Repository the Workflow Run is Associated with
        /// </summary>
        [JsonProperty("repository")]
        public Repository Repository { get; private set; }

        /// <summary>
        /// URL to the Jobs Associated with the Workflow Run
        /// </summary>
        [JsonProperty("jobs_url")]
        public string JobsURL { get; private set; }

        /// <summary>
        /// URL to the Logs Associated with the Workflow Run
        /// </summary>
        [JsonProperty("logs_url")]
        public string LogsURL { get; private set; }

        /// <summary>
        /// Name of the Branch / Pull Request the Workflow Run was Triggered on
        /// </summary>
        [JsonProperty("display_title")]
        public string DisplayTitle { get; private set; }

        /// <summary>
        /// Extra Data Associated with the Repository
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JToken> ExtraData { get; private set; }

        /// <summary>
        /// Gets the Workflows Jobs Logs Files
        /// </summary>
        public byte[] GetLogs()
        {
            using (HttpResponseMessage response = Client.GetAsync(LogsURL).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to get Logs");
                    Console.ResetColor();
                    return null;
                }

                return response.Content.ReadAsByteArrayAsync().Result;
            }
        }

        /// <summary>
        /// Gets the Jobs Associated with the Workflow Run
        /// </summary>
        public WorkflowJob[] GetJobs()
        {
            using (HttpResponseMessage response = Client.GetAsync(JobsURL).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to get Logs");
                    Console.ResetColor();
                    return null;
                }

                JObject keyValuePairs = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                if (keyValuePairs == null)
                    return null;

                WorkflowJob[] jobs = JsonConvert.DeserializeObject<WorkflowJob[]>(keyValuePairs["jobs"].ToString());

                return jobs;
            }
        }
    }
}
