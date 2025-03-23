using System;
using System.Collections.Generic;

namespace NanoDNA.GitHubActionsManager
{
    public class RunnerBuilder : GitHubAPIClient
    {
        public Repository Repository { get; private set; }

        public string Name { get; private set; }

        public List<string> Labels { get; private set; }

        public RunnerBuilder(string name, Repository repository)
        {
            Name = name;
            Repository = repository;
            Labels = new List<string>();
        }

        public RunnerBuilder(string name, Repository repository, List<string> labels)
        {
            Name = name;
            Repository = repository;
            Labels = labels;
        }

        public void AddLabel(string label)
        {
            //Check for spaces and duplicates

            Console.WriteLine($"Adding Label: {label}");

            Labels.Add(label);
        }

        //private string GetToken(string githubPAT)
        //{
        //    JObject tokenResponse;
        //    string tokenRegisterURL = $"{Repository.URL}/actions/runners/registration-token";
        //
        //    using (HttpResponseMessage response = GetClient(githubPAT).PostAsync(tokenRegisterURL, null).Result)
        //    {
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine("Failed to get Token");
        //            Console.ResetColor();
        //            return String.Empty;
        //        }
        //
        //        tokenResponse = JObject.Parse(response.Content.ReadAsStringAsync().Result);
        //    }
        //
        //    return tokenResponse["token"].ToString();
        //}

        public Runner Build()
        {
            return new Runner(Name, Repository.Owner.Login, Repository.Name, Labels.ToArray());
        }





    }
}
