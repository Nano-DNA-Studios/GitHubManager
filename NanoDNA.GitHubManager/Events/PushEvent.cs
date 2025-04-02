using NanoDNA.GitHubManager.Interfaces;

namespace NanoDNA.GitHubManager.Events
{
    /// <summary>
    /// Represents a new Push Event on GitHub
    /// </summary>
    public class PushEvent : IGitHubEvent
    {
        /// <inheritdoc/>
        public string EventType => "push";
    }
}