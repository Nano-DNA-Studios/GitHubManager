using NUnit.Framework;
using System;

namespace NanoDNA.GitHubManager.Tests
{
    /// <summary>
    /// Base Test Class for GitHub API Tests
    /// </summary>
    internal class BaseGitHubAPITest
    {
        /// <summary>
        /// Loads the GitHub Personal Access Token from the Environment Variable
        /// </summary>
        public string GitHubPAT = Environment.GetEnvironmentVariable("GITHUBPAT");

        /// <summary>
        /// Name of the Owner of the Repository
        /// </summary>
        public const string OwnerName = "Nano-DNA-Studios";

        /// <summary>
        /// Name of the Repository
        /// </summary>
        public const string RepoName = "NanoDNA.GitHubManager";

        /// <summary>
        /// Default Image for the Runner
        /// </summary>
        public const string DefaultImage = "mrdnalex/github-action-worker-container-dotnet";

        /// <summary>
        /// One Time Setup Function for Setting the GitHub Personal Access Token
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            GitHubAPIClient.SetGitHubPAT(GitHubPAT);
        }
    }
}
