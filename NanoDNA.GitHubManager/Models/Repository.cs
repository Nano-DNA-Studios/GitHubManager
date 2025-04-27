using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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
        public long ID { get; private set; }

        /// <summary>
        /// Full Name of the Repository (Owner/RepoName)
        /// </summary>
        [JsonProperty("full_name")]
        public string FullName { get; private set; }

        /// <summary>
        /// Name of the Repository
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Owner of the Repository
        /// </summary>
        [JsonProperty("owner")]
        public Owner Owner { get; private set; }

        /// <summary>
        /// Languages the Repository is Written in
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; private set; }

        /// <summary>
        /// HTML URL to the Repository
        /// </summary>
        [JsonProperty("html_url")]
        public string HtmlURL { get; private set; }

        /// <summary>
        /// API URL to the Repository
        /// </summary>
        [JsonProperty("url")]
        public string URL { get; private set; }

        /// <summary>
        /// Toggle for the Repository being Private or Public
        /// </summary>
        [JsonProperty("private")]
        public bool Private { get; private set; }

        /// <summary>
        /// Extra Data Associated with the Repository
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JToken> ExtraData { get; private set; }

        /// <summary>
        /// Gets a Repositories Information from the GitHub API using the Owner and Repository Name
        /// </summary>
        /// <param name="ownerName">Name of the Owner of the Repository</param>
        /// <param name="repositoryName">Name of the Repository</param>
        /// <returns>New Initialized instance of a Repository</returns>
        public static Repository GetRepository(string ownerName, string repositoryName)
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
        /// <returns>Array of Runners belonging to the Repository</returns>
        /// <exception cref="Exception">Thrown if the HttpResponse failed</exception>
        public Runner[] GetRunners()
        {
            using (HttpResponseMessage response = Client.GetAsync($"{URL}/actions/runners").Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to get Runners");

                string responseBody = response.Content.ReadAsStringAsync().Result;

                JObject payload = JObject.Parse(responseBody);

                if ((long)payload["total_count"] == 0)
                    return null;

                return JsonConvert.DeserializeObject<Runner[]>(payload["runners"].ToString());
            }
        }

        /// <summary>
        /// Gets all Workflows Belonging to the Repository
        /// </summary>
        /// <returns>Array of the Workflow Runs</returns>
        /// <exception cref="Exception">Thrown if the HttpResponse failed</exception>
        public WorkflowRun[] GetWorkflows()
        {
            using (HttpResponseMessage response = Client.GetAsync($"{URL}/actions/runs").Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Failed to get Workflows : {response.Content.ReadAsStringAsync()}");

                string responseBody = response.Content.ReadAsStringAsync().Result;

                JObject payload = JObject.Parse(responseBody);

                if ((long)payload["total_count"] == 0)
                    return null;

                return JsonConvert.DeserializeObject<WorkflowRun[]>(payload["workflow_runs"].ToString());
            }
        }

        /// <summary>
        /// Removes and Unregisters a Runner from the Repository
        /// </summary>
        /// <param name="id">ID of the Runner to remove</param>
        /// <exception cref="Exception">Thrown if the Status Code returned has failed</exception>
        public void RemoveRunner (long id)
        {
            using (HttpResponseMessage response = Client.DeleteAsync($"{URL}/actions/runners/{id}").Result)
            {
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Failed to remove Runner : {response.Content.ReadAsStringAsync()}");
            }
        }

        /// <summary>
        /// Tries to Remove a Runner from the Repository
        /// </summary>
        /// <param name="id">ID of the Runner to Remove</param>
        /// <returns>True if the Runner was Removed Successfully or was already Removed, False if an Exception occured</returns>
        public bool TryRemoveRunner (long id)
        {
            try
            {
                RemoveRunner(id);
                return true;
            } catch (Exception e)
            {
                Console.WriteLine($"Failed to remove Runner : {e.Message}");
                return false;
            }
        }
    }
}