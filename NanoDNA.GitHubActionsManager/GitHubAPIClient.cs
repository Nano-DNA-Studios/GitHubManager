using System.Net.Http;

namespace NanoDNA.GitHubActionsManager
{
    public class GitHubAPIClient
    {
        public static HttpClient GetClient(string githubPAT)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"token {githubPAT}");
            client.DefaultRequestHeaders.Add("User-Agent", "CSharp-App");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");

            return client;
        }

        public static string GetRepoLink (string ownerName, string repositoryName)
        {
            return $"https://api.github.com/repos/{ownerName}/{repositoryName}";

        }

        public static string GetHTMLRepoLink(string ownerName, string repositoryName)
        {
            return $"https://github.com/{ownerName}/{repositoryName}";

        }


    }
}
