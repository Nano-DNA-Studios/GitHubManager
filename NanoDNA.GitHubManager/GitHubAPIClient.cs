using System.Net.Http;

namespace NanoDNA.GitHubManager
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

        /// <summary>
        /// Single Instance of the HttpClient for API Communication
        /// </summary>
        private static HttpClient _client { get; set; }

        /// <summary>
        /// HttpClient for API Communication
        /// </summary>
        protected static HttpClient Client => _client;

        /// <summary>
        /// Sets the GitHub Personal Access Token for API Communication
        /// </summary>
        /// <param name="githubPAT">GitHub Personal Access Token for API Communication</param>
        public static void SetGitHubPAT(string githubPAT)
        {
            GitHubPAT = githubPAT;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", $"token {GitHubPAT}");
            _client.DefaultRequestHeaders.Add("User-Agent", "CSharp-App");
            _client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
        }

        /// <summary>
        /// Gets the Repositories API Link using the Owner Name and Repository Name
        /// </summary>
        /// <param name="ownerName">Name of the Owner of the Repository</param>
        /// <param name="repositoryName">Name of the Repository</param>
        /// <returns>API URL to the Repository Requested</returns>
        protected static string GetRepoLink(string ownerName, string repositoryName)
        {
            return $"https://api.github.com/repos/{ownerName}/{repositoryName}";
        }

        /// <summary>
        /// Gets the HTML Link to the Repository using the Owner Name and Repository Name
        /// </summary>
        /// <param name="ownerName">Name of the Owner of the Repository</param>
        /// <param name="repositoryName">Name of the Repository</param>
        /// <returns>HTML URL to the Repository Requested</returns>
        protected static string GetHTMLRepoLink(string ownerName, string repositoryName)
        {
            return $"https://github.com/{ownerName}/{repositoryName}";
        }
    }
}