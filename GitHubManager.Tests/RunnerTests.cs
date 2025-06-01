using NanoDNA.GitHubManager.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Threading;

namespace NanoDNA.GitHubManager.Tests
{
    /// <summary>
    /// Class for the Runner Tests
    /// </summary>
    internal class RunnerTests : BaseGitHubAPITest
    {
        /// <summary>
        /// Tests the Runner Builder Class for Building a Runner Instance
        /// </summary>
        [Test]
        public void RunnerBuilderTest()
        {
            string runnerTest = "RunnerTest1";

            Repository repo = Repository.GetRepository(OwnerName, RepoName);

            RunnerBuilder runnerBuilder = new RunnerBuilder(runnerTest, DefaultImage, repo, false);

            Assert.IsNotNull(runnerBuilder);
            Assert.IsNotNull(runnerBuilder.Labels);
            Assert.That(runnerBuilder.Name, Is.EqualTo(runnerTest));
            Assert.That(runnerBuilder.Image, Is.EqualTo(DefaultImage));
            Assert.That(runnerBuilder.Repository, Is.EqualTo(repo));
            Assert.That(runnerBuilder.Ephemeral, Is.False);
            Assert.That(runnerBuilder.Labels.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests the Runner Builder Class for Building a Runner Instance with Labels
        /// </summary>
        [Test]
        public void RunnerBuilderTestWithLabels()
        {
            string runnerTest = "RunnerTest2";

            Repository repo = Repository.GetRepository(OwnerName, RepoName);

            RunnerBuilder runnerBuilder = new RunnerBuilder(runnerTest, DefaultImage, repo, false);

            runnerBuilder.AddLabel("label1");
            runnerBuilder.AddLabel("label2");
            runnerBuilder.AddLabel("label3");

            Assert.IsNotNull(runnerBuilder);
            Assert.IsNotNull(runnerBuilder.Labels);
            Assert.That(runnerBuilder.Name, Is.EqualTo(runnerTest));
            Assert.That(runnerBuilder.Image, Is.EqualTo(DefaultImage));
            Assert.That(runnerBuilder.Repository, Is.EqualTo(repo));
            Assert.That(runnerBuilder.Ephemeral, Is.False);
            Assert.That(runnerBuilder.Labels.Count, Is.EqualTo(3));
            Assert.That(runnerBuilder.Labels[0], Is.EqualTo("label1"));
            Assert.That(runnerBuilder.Labels[1], Is.EqualTo("label2"));
            Assert.That(runnerBuilder.Labels[2], Is.EqualTo("label3"));
        }

        /// <summary>
        /// Tests if a Runner can be Initialized
        /// </summary>
        [Test]
        public void RunnerInitializationTest()
        {
            string runnerTest = "RunnerTest3";

            Repository repo = Repository.GetRepository(OwnerName, RepoName);
            RunnerBuilder runnerBuilder = new RunnerBuilder(runnerTest, DefaultImage, repo, true);
            Runner runner = runnerBuilder.Build();

            Assert.IsNotNull(runner);
            Assert.IsNotNull(runner.Container);
            Assert.IsNotNull(runner.Labels);
            Assert.That(runner.Name, Is.EqualTo(runnerTest));
            Assert.That(runner.OwnerName, Is.EqualTo(OwnerName));
            Assert.That(runner.RepositoryName, Is.EqualTo(RepoName));
            Assert.That(runner.Container.Name, Is.EqualTo(runnerTest.ToLower()));
            Assert.That(runner.Container.Image, Is.EqualTo(DefaultImage));

            runner.Start();

            Thread.Sleep(5000);

            runner.SyncInfo();

            Assert.IsNotNull(runner);
            Assert.IsNotNull(runner.ID);
            Assert.IsNotNull(runner.Name);
            Assert.IsNotNull(runner.OS);
            Assert.IsNotNull(runner.Status);
            Assert.IsNotNull(runner.Busy);
            Assert.IsNotNull(runner.Labels);

            Assert.That(runner.Name, Is.EqualTo(runnerTest));
            Assert.That(runner.OwnerName, Is.EqualTo(OwnerName));
            Assert.That(runner.RepositoryName, Is.EqualTo(RepoName));
            Assert.That(runner.Container.Name, Is.EqualTo(runnerTest.ToLower()));
            Assert.That(runner.Status, Is.EqualTo("online"));
            Assert.That(runner.OS, Is.EqualTo("Linux"));
            Assert.That(runner.Labels.Single((label) => label.Name == "self-hosted"), Is.InstanceOf<RunnerLabel>());
            Assert.IsFalse(runner.Busy);
            Assert.IsTrue(runner.Running());
            Assert.IsTrue(runner.Registered());

            runner.Stop();
        }

        /// <summary>
        /// Tests if Ephemeral Runners can be Initialized
        /// </summary>
        [Test]
        public void EphemeralRunnerTest()
        {
            string runnerTest = "RunnerTest4";

            Repository repo = Repository.GetRepository(OwnerName, RepoName);
            RunnerBuilder runnerBuilder = new RunnerBuilder(runnerTest, DefaultImage, repo, true);
            Runner runner = runnerBuilder.Build();

            Assert.IsNotNull(runner);
            Assert.IsNotNull(runner.Container);
            Assert.IsNotNull(runner.Labels);

            Assert.That(runner.Name, Is.EqualTo(runnerTest));
            Assert.That(runner.OwnerName, Is.EqualTo(OwnerName));
            Assert.That(runner.RepositoryName, Is.EqualTo(RepoName));
            Assert.That(runner.Container.Name, Is.EqualTo(runnerTest.ToLower()));
            Assert.That(runner.Container.Image, Is.EqualTo(DefaultImage));
            Assert.That(runner.Ephemeral, Is.True);
            Assert.IsFalse(runner.Running());
            Assert.IsFalse(runner.Registered());

            runner.Start();

            Thread.Sleep(5000);

            runner.SyncInfo();

            Assert.IsNotNull(runner);
            Assert.IsNotNull(runner.ID);
            Assert.IsNotNull(runner.Name);
            Assert.IsNotNull(runner.OS);
            Assert.IsNotNull(runner.Status);
            Assert.IsNotNull(runner.Busy);
            Assert.IsNotNull(runner.Labels);

            Assert.That(runner.Name, Is.EqualTo(runnerTest));
            Assert.That(runner.OwnerName, Is.EqualTo(OwnerName));
            Assert.That(runner.RepositoryName, Is.EqualTo(RepoName));
            Assert.That(runner.Container.Name, Is.EqualTo(runnerTest.ToLower()));
            Assert.That(runner.Status, Is.EqualTo("online"));
            Assert.That(runner.OS, Is.EqualTo("Linux"));
            Assert.That(runner.Labels.Single((label) => label.Name == "self-hosted"), Is.InstanceOf<RunnerLabel>());
            Assert.IsFalse(runner.Busy);
            Assert.IsTrue(runner.Running());
            Assert.IsTrue(runner.Registered());

            Thread.Sleep(30000);

            if (runner.Registered() || runner.Running())
            {
                runner.Stop();
                Assert.Fail("Runner did not auto delete");
            }

            if (runner.Container.Exists())
                runner.Container.Remove(true);
        }

        /// <summary>
        /// Tests if a Runner can be Started and Registered with the GitHub API
        /// </summary>
        [Test]
        public void RunnerRunningAndRegisteredTest()
        {
            string runnerTest = "RunnerTest5";

            Repository repo = Repository.GetRepository(OwnerName, RepoName);
            RunnerBuilder runnerBuilder = new RunnerBuilder(runnerTest, DefaultImage, repo, false);
            Runner runner = runnerBuilder.Build();

            Assert.IsNotNull(runner);
            Assert.IsFalse(runner.Running());
            Assert.IsFalse(runner.Registered());

            runner.Start();

            Thread.Sleep(5000);

            runner.SyncInfo();

            Assert.IsNotNull(runner);
            Assert.IsTrue(runner.Running());
            Assert.IsTrue(runner.Registered());

            runner.Stop();

            Assert.IsFalse(runner.Running());
            Assert.IsFalse(runner.Registered());
        }

        /// <summary>
        /// Tests if the Sync Info Function modifies the Runner and Syncs the Info properly with the GitHub API
        /// </summary>
        [Test]
        public void SyncInfoTest()
        {
            string runnerTest = "RunnerTest6";

            Repository repo = Repository.GetRepository(OwnerName, RepoName);
            RunnerBuilder runnerBuilder = new RunnerBuilder(runnerTest, DefaultImage, repo, false);
            Runner runner = runnerBuilder.Build();

            Runner initialCopy = JsonConvert.DeserializeObject<Runner>(JsonConvert.SerializeObject(runner));

            runner.Start();

            Thread.Sleep(3000);

            runner.SyncInfo();

            Assert.IsNotNull(runner);
            Assert.IsNotNull(initialCopy);

            Assert.That(runner.Name, Is.EqualTo(initialCopy.Name));

            Assert.IsTrue(runner.Busy == initialCopy.Busy);

            Assert.IsFalse(runner.OwnerName == initialCopy.OwnerName);
            Assert.IsFalse(runner.RepositoryName == initialCopy.RepositoryName);
            Assert.IsFalse(runner.ID == initialCopy.ID);
            Assert.IsFalse(runner.OS == initialCopy.OS);
            Assert.IsFalse(runner.Status == initialCopy.Status);
            Assert.IsFalse(runner.Labels == initialCopy.Labels);
            Assert.IsFalse(runner.Labels.Length == initialCopy.Labels.Length);

            runner.Stop();
        }

        /// <summary>
        /// Tests if the Runner can be Unregistered from the GitHub API but still have the Container running
        /// </summary>
        [Test]
        public void UnregisterTest()
        {
            string runnerTest = "RunnerTest7";

            Repository repo = Repository.GetRepository(OwnerName, RepoName);
            RunnerBuilder runnerBuilder = new RunnerBuilder(runnerTest, DefaultImage, repo, false);
            Runner runner = runnerBuilder.Build();

            Assert.IsFalse(runner.Running());
            Assert.IsFalse(runner.Registered());

            runner.Start();

            Thread.Sleep(3000);

            runner.SyncInfo();

            Assert.IsTrue(runner.Running());
            Assert.IsTrue(runner.Registered());

            runner.Unregister();

            Thread.Sleep(3000);

            Assert.IsTrue(runner.Running());
            Assert.IsFalse(runner.Registered());

            runner.Stop();

            Assert.IsFalse(runner.Running());
            Assert.IsFalse(runner.Registered());
        }
    }
}