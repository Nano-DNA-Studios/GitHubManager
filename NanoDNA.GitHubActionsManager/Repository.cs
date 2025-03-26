using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace NanoDNA.GitHubActionsManager
{
    public class Repository : GitHubAPIClient
    {
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
                    return null;

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
    }
}
