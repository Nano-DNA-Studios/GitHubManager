using System;
using System.Collections.Generic;
using NanoDNA.GitHubManager.Models;

namespace NanoDNA.GitHubManager
{
    /// <summary>
    /// Helps configure and create new GitHub self-hosted runner instances tied to a specific repository
    /// </summary>
    public class RunnerBuilder : GitHubAPIClient
    {
        /// <summary>
        /// Repository the Runner will be Registered to
        /// </summary>
        public Repository Repository { get; private set; }

        /// <summary>
        /// Name of the Runner
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Labels the Runner will be Registered with
        /// </summary>
        public List<string> Labels { get; private set; }

        /// <summary>
        /// Toggle for the Runner to be Ephemeral
        /// </summary>
        public bool Ephemeral { get; private set; }

        /// <summary>
        /// Docker Image the Runner will Spin Up from
        /// </summary>
        public string Image { get; private set; }

        /// <summary>
        /// Initializes a new Empty Instance of a <see cref="RunnerBuilder"/>
        /// </summary>
        /// <param name="name">Name of the Runner</param>
        /// <param name="image">Docker Image the Runner will Spin Up from</param>
        /// <param name="repository">Repository the Runner is Registered to</param>
        /// <param name="ephemeral">Toggle for the Runner to be Ephemeral</param>
        public RunnerBuilder(string name, string image, Repository repository, bool ephemeral)
        {
            Name = name;
            Repository = repository;
            Labels = new List<string>();
            Ephemeral = ephemeral;
        }

        /// <summary>
        /// Initializes a new Instance of <see cref="RunnerBuilder"/> with a predifined List of Labels
        /// </summary>
        /// <param name="name">Name of the Runner</param>
        /// <param name="image">Docker Image the Runner will Spin Up from</param>
        /// <param name="repository">Repository the Runner will be Registered to</param>
        /// <param name="ephemeral">Toggle for the Runner to be Ephemeral</param>
        /// <param name="labels">List of Labels to Give to the Runner</param>
        public RunnerBuilder(string name, string image, Repository repository, bool ephemeral, List<string> labels)
        {
            Name = name;
            Repository = repository;
            Labels = labels;
            Ephemeral = ephemeral;
        }

        /// <summary>
        /// Adds a Label to the List of Labels for the Runner
        /// </summary>
        /// <param name="label">Label added to the Runner</param>
        /// <exception cref="ArgumentException">Thrown if the Label already Exists in the List</exception>
        public void AddLabel(string label)
        {
            if (Labels.Contains(label))
                throw new ArgumentException("Label already exists");

            Labels.Add(label);
        }

        /// <summary>
        /// Builds the Runner and returns the Instance
        /// </summary>
        /// <returns>Initialized Runner Instance</returns>
        public Runner Build()
        {
            return new Runner(Name, Image, Repository.Owner.Login, Repository.Name, Labels.ToArray(), Ephemeral);
        }
    }
}
