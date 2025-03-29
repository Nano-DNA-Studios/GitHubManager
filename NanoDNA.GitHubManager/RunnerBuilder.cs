using System;
using System.Collections.Generic;

namespace NanoDNA.GitHubManager
{
    public class RunnerBuilder : GitHubAPIClient
    {
        public Repository Repository { get; private set; }

        public string Name { get; private set; }

        public List<string> Labels { get; private set; }

        public bool Ephermal { get; private set; }

        public RunnerBuilder(string name, Repository repository, bool ephemeral)
        {
            Name = name;
            Repository = repository;
            Labels = new List<string>();
            Ephermal = ephemeral;
        }

        /// <summary>
        /// Initializes a new Runner Builder Instance with a predifined List of Labels
        /// </summary>
        /// <param name="name">Name of the Runner</param>
        /// <param name="repository">Repository the Runner will be Registered to</param>
        /// <param name="labels"></param>
        public RunnerBuilder(string name, Repository repository, bool ephemeral, List<string> labels)
        {
            Name = name;
            Repository = repository;
            Labels = labels;
            Ephermal = ephemeral;
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
            return new Runner(Name, Repository.Owner.Login, Repository.Name, Labels.ToArray(), Ephermal);
        }
    }
}
