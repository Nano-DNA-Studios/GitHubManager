namespace NanoDNA.GitHubManager.Events
{
    internal class CheckRunEvent : IGitHubEvent
    {
        public string EventType => "check_run";
    }
}