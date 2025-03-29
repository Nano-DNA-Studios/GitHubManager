using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace NanoDNA.GitHubManager
{
    public class WorkflowRun : GitHubAPIClient
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Name of the Workflow Run
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("conclusion")]
        public string Conclusion { get; set; }

        [JsonProperty("workflow_id")]
        public long WorkflowID { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }

        [JsonProperty("html_url")]
        public string HtmlURL { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("jobs_url")]
        public string JobsURL { get; set; }

        [JsonProperty("logs_url")]
        public string LogsURL { get; set; }

        /// <summary>
        /// Name of the Event Owner that Triggered the Workflow
        /// </summary>
        [JsonProperty("display_title")]
        public string DisplayTitle { get; set; }


        /// <summary>
        /// Extra Data Associated with the Repository
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JToken> ExtraData { get; set; }


        /// <summary>
        /// Gets the Workflows Jobs Logs Files
        /// </summary>
        public void GetLogs ()
        {
            Console.WriteLine("Getting Logs");
            Console.WriteLine(LogsURL);

            using (HttpResponseMessage response = Client.GetAsync(LogsURL).Result)
            {
                Console.WriteLine(response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to get Logs");
                    Console.ResetColor();
                    return;
                }

                Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                Directory.CreateDirectory("Logs");

                File.WriteAllBytes(@$"Logs\GitHubLogs-{ID}.zip", response.Content.ReadAsByteArrayAsync().Result);
            }
        }

        public void GetJobs ()
        {
            Console.WriteLine("Getting Jobs");
            Console.WriteLine(JobsURL);

            using (HttpResponseMessage response = Client.GetAsync(JobsURL).Result)
            {
                Console.WriteLine(response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to get Logs");
                    Console.ResetColor();
                    return;
                }

                Console.WriteLine(response.Content.ReadAsStringAsync().Result);

                Directory.CreateDirectory("Logs");

                File.WriteAllBytes(@$"Logs\GitHubLogs-{ID}.zip", response.Content.ReadAsByteArrayAsync().Result);
            }
        }

    }
}
