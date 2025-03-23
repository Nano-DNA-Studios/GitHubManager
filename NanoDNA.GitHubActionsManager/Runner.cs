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
        public DockerContainer Container { get; private set; }

        public string OwnerName { get; private set; }

        public string RepositoryName { get; private set; }

        internal Runner(string name, string owner, string repository, string[] labels)
        {
            Name = name;
            OwnerName = owner;
            RepositoryName = repository;

            List<RunnerLabel> runnerLabels = new List<RunnerLabel>();

            foreach (string label in labels)
                runnerLabels.Add(new RunnerLabel(label));

            Labels = runnerLabels.ToArray();
        }

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

        public void Start(string githubPAT)
        {
            Container = new DockerContainer(Name.ToLower(), "mrdnalex/github-action-worker-container");

            Console.WriteLine($"Labels : {GetRegistrationLabels()}");

            Container.AddEnvironmentVariable("REPO", GetHTMLRepoLink(OwnerName, RepositoryName));
            Container.AddEnvironmentVariable("TOKEN", GetToken(githubPAT));
            Container.AddEnvironmentVariable("RUNNERGROUP", "");
            Container.AddEnvironmentVariable("RUNNERNAME", Name);
            Container.AddEnvironmentVariable("RUNNERLABELS", "self-hosted,gitapiexperiment");
            Container.AddEnvironmentVariable("RUNNERWORKDIR", "WorkDir");

            foreach (KeyValuePair<string, string> env in Container.EnvironmentVariables)
            {
                Console.WriteLine($"{env.Key} : {env.Value}");
            }

            Container.Start();

            WaitForRegistered(githubPAT);

            Console.WriteLine($"Is Alive : {IsAlive(githubPAT)}");
        }
       
        private void WaitForRegistered (string githubPAT)
        {
            int count = 0;

            while (!IsAlive(githubPAT) && count < 50)
            {
                Thread.Sleep(100);
                count++;
            }

            Console.WriteLine($"Wait Count : {count}");
        }

        private void WaitForUnregistered(string githubPAT)
        {
            int count = 0;

            while (IsAlive(githubPAT) && count < 50)
            {
                Thread.Sleep(100);
                count++;
            }

            Console.WriteLine($"Wait Count : {count}");
        }

        //Split into 2 Versions (Registered for On GitHub API, Running for Container is alive)
        public bool IsAlive(string githubPAT)
        {
            using (HttpResponseMessage response = GetClient(githubPAT).GetAsync($"https://api.github.com/repos/{OwnerName}/{RepositoryName}/actions/runners?name={Name}").Result)
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

        private string GetRegistrationLabels()
        {
            string labels = "";

            Console.WriteLine($"Labels Length : {Labels.Length}");

            foreach (RunnerLabel label in Labels)
            {
                Console.WriteLine(label.Name);
                labels += label.Name + ",";
            }

            return labels.TrimEnd(',');
        }

        public void Stop(string githubPAT)
        {
            Container.Execute($"/home/GitWorker/ActionRunner/config.sh remove --token {Container.EnvironmentVariables["TOKEN"]}");

            WaitForUnregistered(githubPAT);

            Container.Stop();

            Console.WriteLine(Container.GetLogs());
            Container.Remove();
        }

        public string GetToken(string githubPAT)
        {
            JObject tokenResponse;
            string tokenRegisterURL = $"{GetRepoLink(OwnerName, RepositoryName)}/actions/runners/registration-token";

            using (HttpResponseMessage response = GetClient(githubPAT).PostAsync(tokenRegisterURL, null).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed to get Token");
                    Console.ResetColor();
                    return String.Empty;
                }

                tokenResponse = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            }

            return tokenResponse["token"].ToString();

        }

        //public Runner StartGitHubActionsRunner(string githubPAT)
        //{
        //    DockerContainer container = new DockerContainer("gitapiexperiments", "mrdnalex/github-action-worker-container");
        //
        //    container.AddEnvironmentVariable("REPO", HtmlURL);
        //    container.AddEnvironmentVariable("TOKEN", GetToken(githubPAT));
        //    container.AddEnvironmentVariable("RUNNERGROUP", "");
        //    container.AddEnvironmentVariable("RUNNERNAME", "GitHubAPIExperiments");
        //    container.AddEnvironmentVariable("RUNNERLABELS", "self-hosted,gitapiexperiment");
        //    container.AddEnvironmentVariable("RUNNERWORKDIR", "WorkDir");
        //
        //    container.Start();
        //
        //    Runner[] runners = GetRunners(githubPAT);
        //
        //    if (runners.Length == 0)
        //        throw new Exception("No Runners Found");
        //
        //    Console.WriteLine("Displaying Payload");
        //    Console.WriteLine(JsonConvert.SerializeObject(runners, Formatting.Indented));
        //
        //    Runner runner = runners[0];
        //
        //    runner.Container = container;
        //
        //    return runner;
        //}
    }
}
