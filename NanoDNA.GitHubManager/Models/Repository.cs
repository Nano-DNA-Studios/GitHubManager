using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace NanoDNA.GitHubManager.Models
{
    /// <summary>
    /// Describes a GitHub Repository's Information
    /// </summary>
    public class Repository : GitHubAPIClient
    {
        /// <summary>
        /// ID of the Repository, Unique to Each Repository
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// Full Name of the Repository (Owner/RepoName)
        /// </summary>
        [JsonProperty("full_name")]
        public string FullName { get; set; }

        /// <summary>
        /// Name of the Repository
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Owner of the Repository
        /// </summary>
        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        /// <summary>
        /// Languages the Repository is Written in
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// HTML URL to the Repository
        /// </summary>
        [JsonProperty("html_url")]
        public string HtmlURL { get; set; }

        /// <summary>
        /// API URL to the Repository
        /// </summary>
        [JsonProperty("url")]
        public string URL { get; set; }

        /// <summary>
        /// Toggle for the Repository being Private or Public
        /// </summary>
        [JsonProperty("private")]
        public bool Private { get; set; }

        /// <summary>
        /// Extra Data Associated with the Repository
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JToken> ExtraData { get; set; }

        /// <summary>
        /// Gets a Repositories Information from the GitHub API using the Owner and Repository Name
        /// </summary>
        /// <param name="ownerName">Name of the Owner of the Repository</param>
        /// <param name="repositoryName">Name of the Repository</param>
        /// <returns>New Initialized instance of a Repository</returns>
        public static Repository GetRepo(string ownerName, string repositoryName)
        {
            using (HttpResponseMessage response = Client.GetAsync($"https://api.github.com/repos/{ownerName}/{repositoryName}").Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to get Repository");

                string responseBody = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<Repository>(responseBody);
            }
        }

        /// <summary>
        /// Gets all Runners Belonging to the Repository 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Runner[] GetRunners()
        {
            using (HttpResponseMessage response = Client.GetAsync($"{URL}/actions/runners").Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to get Runners");

                string responseBody = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(responseBody);

                JObject payload = JObject.Parse(responseBody);
                JToken runners = payload["runners"];

                return JsonConvert.DeserializeObject<Runner[]>(runners.ToString());
            }
        }

        public WorkflowRun[] GetWorkflows()
        {
            using (HttpResponseMessage response = Client.GetAsync($"{URL}/actions/runs").Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to get Runners");

                string responseBody = response.Content.ReadAsStringAsync().Result;

                JObject payload = JObject.Parse(responseBody);

                File.WriteAllText(@"C:\Users\MrDNA\Downloads\GitHubActionWorker\workflows.json", payload.ToString());

                JToken workflowRuns = payload["workflow_runs"];

                return JsonConvert.DeserializeObject<WorkflowRun[]>(workflowRuns.ToString());
            }
        }

        public void GetWorkflowJobs()
        {
            using (HttpResponseMessage response = Client.GetAsync($"{URL}/actions/jobs").Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to get Runners");

                string responseBody = response.Content.ReadAsStringAsync().Result;

                //Console.WriteLine(responseBody);

                JObject payload = JObject.Parse(responseBody);

                Console.WriteLine(payload.ToString());

                // return JsonConvert.DeserializeObject<Runner[]>(runners.ToString());
            }
        }
    }
}
