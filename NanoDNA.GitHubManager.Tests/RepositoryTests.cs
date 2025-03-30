using NanoDNA.GitHubManager.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;

namespace NanoDNA.GitHubManager.Tests
{
    /// <summary>
    /// Tests for the Repository Class
    /// </summary>
    internal class RepositoryTests : BaseGitHubAPITest
    {
        /// <summary>
        /// Tests the Static GetRepository Function
        /// </summary>
        /// <param name="ownerName">Name of the Owner of the Repository</param>
        /// <param name="repoName">Name of the Repository</param>
        /// <param name="pass">Toggle if the Test should pass</param>
        [Test]
        [TestCase(OwnerName, RepoName, true)]
        [TestCase(OwnerName, RepoName + "s", false)]
        [TestCase(OwnerName + "s", RepoName, false)]
        [TestCase(OwnerName + "s", RepoName + "s", false)]
        public void GetRepositoryTest(string ownerName, string repoName, bool pass)
        {
            if (!pass)
            {
                Assert.Throws<Exception>(() => Repository.GetRepository(ownerName, repoName));
                return;
            }

            Repository repo = Repository.GetRepository(ownerName, repoName);

            Assert.IsNotNull(repo);
            Assert.That(repo.Name, Is.EqualTo(repoName), "Repository Names are not matching");
            Assert.That(repo.Owner.Login, Is.EqualTo(ownerName), "Owner Names are not matching");
        }

        /// <summary>
        /// Tests the GetWorkflows Function
        /// </summary>
        /// <param name="pass">Toggle for the Test to Pass</param>
        [Test]
        [TestCase(true)]
        public void GetWorkflowsTest(bool pass)
        {
            Repository repo = Repository.GetRepository(OwnerName, RepoName);

            if (!pass)
            {
                Assert.Throws<Exception>(() => repo.GetWorkflows());
                return;
            }

            WorkflowRun[] workflows = repo.GetWorkflows();

            Assert.IsNotNull(workflows);
            Assert.That(workflows.Length, Is.GreaterThan(0), "No Workflows Found");

            foreach (WorkflowRun workflow in workflows)
            {
                Assert.IsNotNull(workflow);
                Assert.IsNotNull(workflow.ID);
                Assert.IsNotNull(workflow.WorkflowID);
                Assert.IsNotNull(workflow.Name);
                Assert.IsNotNull(workflow.Status);
                Assert.IsNotNull(workflow.Conclusion);
                Assert.IsNotNull(workflow.CreatedAt);
                Assert.IsNotNull(workflow.UpdatedAt);
                Assert.IsNotNull(workflow.HtmlURL);
                Assert.IsNotNull(workflow.URL);
            }
        }

        /// <summary>
        /// Tests if the GetRunners Function returns Runners
        /// </summary>
        [Test]
        public void GetRunnersTest ()
        {
            try
            {
                Repository repo = Repository.GetRepository(OwnerName, RepoName);
                RunnerBuilder builder = new RunnerBuilder("RunnerTest", DefaultImage, repo, false);
                Runner runner1 = builder.Build();

                Console.WriteLine(JsonConvert.SerializeObject(runner1.Container, Formatting.Indented));

                runner1.Start();

                Runner[] runners = repo.GetRunners();

                Assert.IsNotNull(runners);
                Assert.That(runners.Length, Is.GreaterThan(0), "No Runners Found");
                Assert.That(runners.Single(r => r.Name == runner1.Name), Is.InstanceOf<Runner>());

                foreach (Runner runner in runners)
                {
                    Assert.IsNotNull(runner);
                    Assert.IsNotNull(runner.Name);
                    Assert.IsNotNull(runner.OS);
                    Assert.IsNotNull(runner.Status);
                    Assert.IsNotNull(runner.Busy);
                }

                runner1.Stop();
            } catch (Exception e)
            {
                Assert.Fail(e.Message);


            }

        }
    }
}
