using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using Newtonsoft.Json.Linq;
using NanoDNA.DockerManager;
using System.Collections.Generic;

namespace NanoDNA.GitHubManager.Models
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
        public long ID { get; private set; }

        /// <summary>
        /// Name of the Runner Instance
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Operating System of the Runner Instance
        /// </summary>
        [JsonProperty("os")]
        public string OS { get; private set; }

        /// <summary>
        /// Status of the Runner Instance (online , offline, etc.)
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; private set; }

        /// <summary>
        /// Flag indicating if the Runner Instance is Busy running a Workflow
        /// </summary>
        [JsonProperty("busy")]
        public bool Busy { get; private set; }

        /// <summary>
        /// Labels assigned to the Runner Instance
        /// </summary>
        [JsonProperty("labels")]
        public RunnerLabel[] Labels { get; private set; }

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
        /// Toggle indicating if the Runner is Ephemeral
        /// </summary>
        public bool Ephemeral { get; private set; }

        /// <summary>
        /// Timer Responsible for Syncing the Runner with the GitHub API, Used for Ephemeral Runners
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Event that is Triggered when the Runner is being Stopped
        /// </summary>
        public event Action<Runner> StopRunner;

        /// <summary>
        /// Number of times the Runner is Idle in a Row, used for Ephemeral Runners
        /// </summary>
        private int idleCount = 0;

        /// <summary>
        /// Initializes a new Runner Instance with the specified Name, Owner, Repository and Labels
        /// </summary>
        /// <param name="name">Name of the Runner</param>
        /// <param name="image">Docker Image the Runner will Spin Up from</param>
        /// <param name="ownerName">Name of the Repositories Owner</param>
        /// <param name="repositoryName">Name of the Repository</param>
        /// <param name="labels">Labels to add to the Runner</param>
        /// <param name="ephemeral">Toggle for if the Runner should be Ephemeral and remove itself once Idle for long enough</param>
        internal Runner(string name, string image, string ownerName, string repositoryName, string[] labels, bool ephemeral)
        {
            Name = name;
            OwnerName = ownerName;
            RepositoryName = repositoryName;
            Ephemeral = ephemeral;
            Container = new DockerContainer(Name.ToLower(), image, true);

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
        /// Starts a new Runner Instance in a Docker Container and Registers it with the GitHub API
        /// </summary>
        public void Start()
        {
            Container.AddEnvironmentVariable("REPO", GetHTMLRepoLink(OwnerName, RepositoryName));
            Container.AddEnvironmentVariable("TOKEN", GetToken());
            Container.AddEnvironmentVariable("RUNNERGROUP", "");
            Container.AddEnvironmentVariable("RUNNERNAME", Name);
            Container.AddEnvironmentVariable("RUNNERLABELS", GetRegistrationLabels());
            Container.AddEnvironmentVariable("RUNNERWORKDIR", "WorkDir");

            Container.Start();

            WaitUntilRegistered();
            SyncInfo();

            if (Ephemeral)
                _timer = new Timer((e) => EphemeralSync(), null, 5000, 5000);
        }

        /// <summary>
        /// Syncs the Ephemeral Runner to the GitHub API and Stops the Runner if it is not Busy
        /// </summary>
        private void EphemeralSync()
        {
            SyncInfo();

            if (!Busy && Status == "online")
                idleCount++;
            else
                idleCount = 0;

            if (idleCount > 3)
                Stop();
        }

        /// <summary>
        /// Generic Function for Repeatedly checking a Condition until it is met or a Timeout Occurs
        /// </summary>
        /// <param name="condition">Function representing the Condition to repeatedly Check. Function will block thread until the condition is equivalent to <paramref name="expectedResult"/></param>
        /// <param name="expectedResult">The expected result of the condition function before unblocking the thread</param>
        /// <param name="maxWaitSec">Maximum number of Seconds to wait for before continuing as a Timeout error</param>
        private void WaitUntil(Func<bool> condition, bool expectedResult, int maxWaitSec)
        {
            int waitCount = maxWaitSec * 2;
            int count = 0;
            while (condition() != expectedResult && count < waitCount)
            {
                Thread.Sleep(500);
                count++;
            }
        }

        /// <summary>
        /// Syncs Info from the GitHub API about the Runner and returns the Busy Result
        /// </summary>
        /// <returns>True if Busy, False otherwise</returns>
        private bool BusySync ()
        {
            SyncInfo();
            return Busy;
        }

        /// <summary>
        /// Waits for the Runner to have a Busy Status before continuing or until timeout
        /// </summary>
        public void WaitUntilBusy(int maxWaitSec = 50) => WaitUntil(BusySync, true, maxWaitSec);
        
        /// <summary>
        /// Waits for the Runner to have an Idle Status before continuing or until timeout
        /// </summary>
        public void WaitUntilIdle(int maxWaitSec = 50) => WaitUntil(BusySync, false, maxWaitSec);
        
        /// <summary>
        /// Waits on the GitHub Action Runner to be Registered and Visible on the GitHub API
        /// </summary>
        private void WaitUntilRegistered(int maxWaitSec = 50) => WaitUntil(Registered, true, maxWaitSec);
        
        /// <summary>
        /// Waits on the Runner to be Unregistered and Removed on the GitHub API
        /// </summary>
        private void WaitUntilUnregistered(int maxWaitSec = 50) => WaitUntil(Registered, false, maxWaitSec);

        /// <summary>
        /// Checks if the Runner Container is Running
        /// </summary>
        /// <returns>True if the Container is Running, False otherwise</returns>
        public bool Running() => Container.Running();

        /// <summary>
        /// Checks if the Runner is Registered on the GitHub API
        /// </summary>
        /// <returns>True if the Runner is Registered and Ready in the GitHub API, False otherwise</returns>
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

                return (long)runnerResponse["total_count"] == 1;
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

            return Labels.Length == 0 ? "" : $"\"{labels}\"";
        }

        /// <summary>
        /// Stops the Runner by Unregistering it from the GitHub API and Stopping the Docker Container
        /// </summary>
        public void Stop()
        {
            StopRunner?.Invoke(this);

            if (Ephemeral && _timer != null)
                _timer.Dispose();

            if (Registered())
            {
                Unregister();
                WaitUntilUnregistered();
            }

            if (Container.Running())
                Container.Stop();

            if (Container.Exists())
                Container.Remove();
        }

        /// <summary>
        /// Unregisters the Runner from the GitHub API
        /// </summary>
        public void Unregister()
        {
            try
            {
                if (Registered() && Container.EnvironmentVariables.ContainsKey("TOKEN"))
                    Container.Execute($"/home/GitWorker/ActionRunner/config.sh remove --token {Container.EnvironmentVariables["TOKEN"]}");
                else
                    Container.Execute($"bash -c \"/home/GitWorker/ActionRunner/config.sh remove --token $TOKEN\"");
            }
            catch (Exception e)
            {
                if (Registered())
                    Container.Execute($"bash -c \"/home/GitWorker/ActionRunner/config.sh remove --token $TOKEN\"");
            }

            if (Registered())
                throw new Exception("Failed to Unregister Runner from GitHub API");
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
                    return string.Empty;
                }

                return JObject.Parse(response.Content.ReadAsStringAsync().Result)["token"].ToString();
            }
        }

        /// <summary>
        /// Syncs the Runners Information with the GitHub API
        /// </summary>
        public void SyncInfo()
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

                if ((long)runnerResponse["total_count"] == 0)
                    return;

                Name = runnerResponse["runners"][0]["name"].ToString();
                ID = (long)runnerResponse["runners"][0]["id"];
                OS = runnerResponse["runners"][0]["os"].ToString();
                Status = runnerResponse["runners"][0]["status"].ToString();
                Busy = (bool)runnerResponse["runners"][0]["busy"];
                Labels = JsonConvert.DeserializeObject<RunnerLabel[]>(runnerResponse["runners"][0]["labels"].ToString());
            }
        }
    }
}