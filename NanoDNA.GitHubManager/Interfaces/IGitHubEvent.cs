namespace NanoDNA.GitHubManager.Interfaces
{
    /// <summary>
    /// Interface for GitHub Events
    /// </summary>
    public interface IGitHubEvent
    {
        /// <summary>
        /// Event Type sent by GitHub
        /// </summary>
        string EventType { get; }
    }
}