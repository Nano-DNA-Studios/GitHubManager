using NanoDNA.GitHubManager.Interfaces;

namespace NanoDNA.GitHubManager.Events
{
    internal class PullRequestEvent : IGitHubEvent
    {
        public string EventType => "pull_request";
    }
}