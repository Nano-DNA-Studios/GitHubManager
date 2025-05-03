using NanoDNA.GitHubManager.Interfaces;

namespace NanoDNA.GitHubManager.Events
{
    /// <summary>
    /// Represents a new Check Run Event on GitHub
    /// </summary>
    public class CheckRunEvent : IGitHubEvent
    {
        /// <inheritdoc/>
        public string EventType => "check_run";
    }
}