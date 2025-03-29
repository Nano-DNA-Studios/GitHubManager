using NanoDNA.GitHubManager.Interfaces;

namespace NanoDNA.GitHubManager.Events
{
    internal class PushEvent : IGitHubEvent
    {
        public string EventType => "push";
    }
}