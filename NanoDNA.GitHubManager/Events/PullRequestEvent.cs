using NanoDNA.GitHubManager.Interfaces;

namespace NanoDNA.GitHubManager.Events
{
    /// <summary>
    /// Represents a new Pull Request Event on GitHub
    /// </summary>
    public class PullRequestEvent : IGitHubEvent
    {
        /// <inheritdoc/>
        public string EventType => "pull_request";
    }
}