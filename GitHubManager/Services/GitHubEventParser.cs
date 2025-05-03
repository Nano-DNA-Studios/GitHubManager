using NanoDNA.GitHubManager.Events;
using NanoDNA.GitHubManager.Interfaces;
using Newtonsoft.Json;
using System;

namespace NanoDNA.GitHubManager.Services
{
    /// <summary>
    /// Parses GitHub Webhook JSON Payloads into the Appropriate Event
    /// </summary>
    public class GitHubEventParser
    {
        /// <summary>
        /// Parses a JSON Payload into the Appropriate GitHub Event
        /// </summary>
        /// <param name="json">JSON Payload from GitHub</param>
        /// <param name="eventName">Name of the GitHub Event</param>
        /// <returns>Event instance as a <see cref="IGitHubEvent"/></returns>
        public static IGitHubEvent Parse(string json, string eventName)
        {
            Console.WriteLine(eventName);

            return eventName switch
            {
                "workflow_job" => JsonConvert.DeserializeObject<WorkflowJobEvent>(json),
                "workflow_run" => JsonConvert.DeserializeObject<WorkflowRunEvent>(json),
                "pull_request" => JsonConvert.DeserializeObject<PullRequestEvent>(json),
                "push" => JsonConvert.DeserializeObject<PushEvent>(json),
                "check_run" => JsonConvert.DeserializeObject<CheckRunEvent>(json),
                _ => null
            };
        }
    }
}
