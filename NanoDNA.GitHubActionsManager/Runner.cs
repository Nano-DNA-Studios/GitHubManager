using NanoDNA.DockerManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace NanoDNA.GitHubActionsManager
{
    /// <summary>
    /// Describes a GitHub Runner Instances Information
    /// </summary>
    public class Runner : GitHubAPIClient
    {
        /// <summary>
        /// ID of the Runner Instance
        /// </summary>
        [JsonProperty("id")]
        public long ID { get; set; }

        /// <summary>
        /// Name of the Runner Instance
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Operating System of the Runner Instance
        /// </summary>
        [JsonProperty("os")]
        public string OS { get; set; }

        /// <summary>
        /// Status of the Runner Instance (online , offline, etc.)
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Flag indicating if the Runner Instance is Busy running a Workflow
        /// </summary>
        [JsonProperty("busy")]
        public bool Busy { get; set; }

        /// <summary>
        /// Labels assigned to the Runner Instance
        /// </summary>
        [JsonProperty("labels")]
        public RunnerLabel[] Labels { get; set; }

        /// <summary>
        /// Docker Container Controlling the Runner Instance
        /// </summary>
        public DockerContainer Container { get; private set; }

        /// <summary>
        /// Repository Owner's Name
        /// </summary>
        public string OwnerName { get; private set; }

        /// <summary>
        /// Repository Name
        /// </summary>
        public string RepositoryName { get; private set; }

        /// <summary>
        /// Initializes a new Runner Instance with the specified Name, Owner, Repository and Labels
        /// </summary>
        /// <param name="name">Name of the Runner</param>
        /// <param name="ownerName">Name of the Repositories Owner</param>
        /// <param name="repositoryName">Name of the Repository</param>
        /// <param name="labels">Labels to add to the Runner</param>
        internal Runner(string name, string ownerName, string repositoryName, string[] labels)
        {
            Name = name;
            OwnerName = ownerName;
            RepositoryName = repositoryName;

            List<RunnerLabel> runnerLabels = new List<RunnerLabel>();

            foreach (string label in labels)
                runnerLabels.Add(new RunnerLabel(label));

            Labels = runnerLabels.ToArray();
        }

        /// <summary>
        /// Intializes a new Empty Runner Instance, used for JSON Deserialization
        /// </summary>
        protected Runner() { }

        /// <summary>
        /// Starts a Runner in a Docker Container
        /// </summary>
        public void Start()
        {
            Container = new DockerContainer(Name.ToLower(), "mrdnalex/github-action-worker-container");

            Container.AddEnvironmentVariable("REPO", GetHTMLRepoLink(OwnerName, RepositoryName));
            Container.AddEnvironmentVariable("TOKEN", GetToken());
            Container.AddEnvironmentVariable("RUNNERGROUP", "");
            Container.AddEnvironmentVariable("RUNNERNAME", Name);
            Container.AddEnvironmentVariable("RUNNERLABELS", $"\"{GetRegistrationLabels()}\"");
            Container.AddEnvironmentVariable("RUNNERWORKDIR", "WorkDir");

            Container.Start();

            WaitForRegistered();
            UpdateRunnerInfo();
        }

        /// <summary>
        /// Waits on the GitHub Action Runner to be Registered and Visible on the GitHub API
        /// </summary>
        private void WaitForRegistered()
        {
            int count = 0;

            while (!Registered() && count < 50)
            {
                Thread.Sleep(100);
                count++;
            }
        }

        /// <summary>
        /// Waits on the Runner to be Unregistered and Removed on the GitHub API
        /// </summary>
        private void WaitForUnregistered()
        {
            int count = 0;

            while (Registered() && count < 50)
            {
                Thread.Sleep(100);
                count++;
            }
        }

        /// <summary>
        /// Checks if the Runner Container is Running
        /// </summary>
        /// <returns>True if the Container is Running, False otherwise</returns>
        public bool Running()
        {
            return Container.Running();
        }

        //Split into 2 Versions (Registered for On GitHub API, Running for Container is alive)
        public bool Registered()
        {
            using (HttpResponseMessage response = Client.GetAsync($"https://api.github.com/repos/{OwnerName}/{RepositoryName}/actions/runners?name={Name}").Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to get Runner Status");
                    Console.ResetColor();
                    return false;
                }

                JObject runnerResponse = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                return (long)(runnerResponse["total_count"]) == 1;
            }
        }

        /// <summary>
        /// Gets the Labels that will be added to the Runner in String Format
        /// </summary>
        /// <returns>Labels to add in Appropriate String Format</returns>
        private string GetRegistrationLabels()
        {
            string labels = "";

            for (int i = 0; i < Labels.Length; i++)
            {
                labels += Labels[i].Name;

                if (i < Labels.Length - 1)
                    labels += ",";
            }

            return labels;
        }

        /// <summary>
        /// Stops the Runner
        /// </summary>
        public void Stop()
        {
            Container.Execute($"/home/GitWorker/ActionRunner/config.sh remove --token {Container.EnvironmentVariables["TOKEN"]}");

            WaitForUnregistered();

            Container.Stop();
            Container.Remove();
        }

        /// <summary>
        /// Gets the Registration Token to Register the Runner with the GitHub API
        /// </summary>
        /// <returns>The Registration Token</returns>
        private string GetToken()
        {
            string tokenRegisterURL = $"{GetRepoLink(OwnerName, RepositoryName)}/actions/runners/registration-token";

            using (HttpResponseMessage response = Client.PostAsync(tokenRegisterURL, null).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to get Token");
                    Console.ResetColor();
                    return String.Empty;
                }

                return JObject.Parse(response.Content.ReadAsStringAsync().Result)["token"].ToString();
            }
        }

        /// <summary>
        /// Updates the Runner Information using the Instanciated Runner from the GitHub API
        /// </summary>
        private void UpdateRunnerInfo()
        {
            using (HttpResponseMessage response = Client.GetAsync($"https://api.github.com/repos/{OwnerName}/{RepositoryName}/actions/runners?name={Name}").Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to get Runner Status");
                    Console.ResetColor();
                    return;
                }

                JObject runnerResponse = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                Name = runnerResponse["runners"][0]["name"].ToString();
                ID = (long)runnerResponse["runners"][0]["id"];
                OS = runnerResponse["runners"][0]["os"].ToString();
                Status = runnerResponse["runners"][0]["status"].ToString();
                Busy = (bool)runnerResponse["runners"][0]["busy"];
                Labels = JsonConvert.DeserializeObject<RunnerLabel[]>(runnerResponse["runners"][0]["labels"].ToString());
            }
        }

        public static Runner[] GetRunners(string ownerName, string repositoryName)
        {
            using (HttpResponseMessage response = Client.GetAsync($"https://api.github.com/repos/{ownerName}/{repositoryName}/actions/runners").Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to get Runner Status");
                    Console.ResetColor();
                    return null;
                }

                JObject runnerResponse = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                Console.WriteLine(runnerResponse.ToString());

                return JsonConvert.DeserializeObject<Runner[]>(runnerResponse["runners"].ToString());
            }
        }
    }
}
