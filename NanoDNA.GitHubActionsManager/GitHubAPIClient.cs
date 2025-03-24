using System.Diagnostics;
using System.Net.Http;

namespace NanoDNA.GitHubActionsManager
{
    /// <summary>
    /// Base Class for GitHub API Communication
    /// </summary>
    public class GitHubAPIClient
    {
        /// <summary>
        /// GitHub Personal Access Token for API Communication
        /// </summary>
        private static string GitHubPAT { get; set; }

        private static HttpClient _client;

        protected static HttpClient Client => _client;

        public static void SetGitHubPAT (string githubPAT)
        {
            GitHubPAT = githubPAT;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", $"token {GitHubPAT}");
            _client.DefaultRequestHeaders.Add("User-Agent", "CSharp-App");
            _client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
        }

        protected static string GetRepoLink (string ownerName, string repositoryName)
        {
            return $"https://api.github.com/repos/{ownerName}/{repositoryName}";
        }

        protected static string GetHTMLRepoLink(string ownerName, string repositoryName)
        {
            return $"https://github.com/{ownerName}/{repositoryName}";
        }
    }
}
