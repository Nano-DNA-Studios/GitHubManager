using NanoDNA.GitHubManager.Models;
using NUnit.Framework;

namespace NanoDNA.GitHubManager.Tests
{
    /// <summary>
    /// Runs the Tests on the Workflow Run Class
    /// </summary>
    internal class WorkflowRunTests : BaseGitHubAPITest
    {
        /// <summary>
        /// Tests if Workflow Runs can be Retrieved from a Repository
        /// </summary>
        [Test]
        public void GetWorkflowRuns ()
        {
            Repository repo = Repository.GetRepository(OwnerName, RepoName);

            WorkflowRun[] workflows = repo.GetWorkflows();

            Assert.IsNotNull(workflows);
            Assert.That(workflows.Length, Is.GreaterThan(0));

            WorkflowRun workflow = workflows[0];

            Assert.IsNotNull(workflow);
            Assert.That(workflow.ID, Is.Not.Null);
            Assert.That(workflow.Name, Is.Not.Null);
            Assert.That(workflow.Event, Is.Not.Null);
            Assert.That(workflow.Status, Is.Not.Null);
            Assert.That(workflow.Conclusion, Is.Not.Null);
            Assert.That(workflow.WorkflowID, Is.GreaterThan(0));
        }

        /// <summary>
        /// Tests if Workflow Run Logs can be extracted from a Successful Workflow Run
        /// </summary>
        [Test]
        public void GetLogs ()
        {
            Repository repo = Repository.GetRepository(OwnerName, RepoName);

            WorkflowRun[] workflows = repo.GetWorkflows();

            Assert.IsNotNull(workflows);

            foreach (WorkflowRun workflow in workflows)
            {
                if (workflow.Conclusion == "success")
                {
                    byte[] logs = workflow.GetLogs();

                    Assert.IsNotNull(logs);
                    Assert.That(logs.Length, Is.GreaterThan(0));

                    break;
                }
            }
        }

        /// <summary>
        /// Tests if a Job can be retrieved from a Successful Workflow Run
        /// </summary>
        [Test]
        public void GetJobs ()
        {
            Repository repo = Repository.GetRepository(OwnerName, RepoName);

            WorkflowRun[] workflows = repo.GetWorkflows();

            Assert.IsNotNull(workflows);

            foreach (WorkflowRun workflow in workflows)
            {
                if (workflow.Conclusion == "success")
                {
                    WorkflowJob[] jobs = workflow.GetJobs();

                    Assert.IsNotNull(jobs);

                    WorkflowJob workflowJob = jobs[0];

                    Assert.IsNotNull(workflowJob);

                    Assert.That(workflowJob.ID, Is.GreaterThan(0));
                    Assert.That(workflowJob.RunID, Is.GreaterThan(0));
                    Assert.That(workflowJob.Name, Is.Not.Null);
                    Assert.That(workflowJob.Status, Is.Not.Null);
                    Assert.That(workflowJob.Conclusion, Is.Not.Null);
                    Assert.That(workflowJob.StartedAt, Is.Not.Null);
                    Assert.That(workflowJob.CompletedAt, Is.Not.Null);
                    Assert.That(workflowJob.URL, Is.Not.Null);
                    Assert.That(workflowJob.HtmlURL, Is.Not.Null);
                    Assert.That(workflowJob.Steps, Is.Not.Null);
                    Assert.That(workflowJob.Steps.Length, Is.GreaterThan(0));

                    Assert.That(workflowJob.Steps[0].Name, Is.Not.Null);
                    Assert.That(workflowJob.Steps[0].Status, Is.Not.Null);
                    Assert.That(workflowJob.Steps[0].Conclusion, Is.Not.Null);
                    Assert.That(workflowJob.Steps[0].Number, Is.GreaterThan(0));
                    Assert.That(workflowJob.Steps[0].StartedAt, Is.Not.Null);
                    Assert.That(workflowJob.Steps[0].CompletedAt, Is.Not.Null);

                    break;
                }
            }
        }
    }
}
