using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using NanoDNA.GitHubManager.Interfaces;
using Microsoft.Extensions.Logging;

namespace NanoDNA.GitHubManager.Services
{
    /// <summary>
    /// Basic Webhook Receiver for GitHub Events.
    /// </summary>
    public class GitHubWebhookService
    {
        /// <summary>
        /// Secret from the GitHub Webhook.
        /// </summary>
        private readonly string _secret;

        /// <summary>
        /// Event Dispatcher for GitHub Events.
        /// </summary>
        private readonly GitHubEventDispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubWebhookService"/>.
        /// </summary>
        /// <param name="secret">Secret for Verifying received Webhooks</param>
        public GitHubWebhookService(string secret)
        {
            _secret = secret;
            _dispatcher = new GitHubEventDispatcher();
        }

        /// <summary>
        /// Event Subscription for GitHub Events.
        /// </summary>
        /// <typeparam name="T">Type of Event to Subscribe to</typeparam>
        /// <param name="handler">Function / Handler to run on Event</param>
        public void On<T>(Action<T> handler) where T : IGitHubEvent => _dispatcher.On(handler);

        /// <summary>
        /// Creates a Basic Webhook Receiver Server Application for GitHub Events.
        /// </summary>
        /// <returns>New Application Instance</returns>
        private WebApplication CreateWebhookApp()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Logging.AddFilter("Microsoft", LogLevel.Warning);
            builder.Logging.AddFilter("System", LogLevel.Warning);
            builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
            
            WebApplication app = builder.Build();

            app.MapPost("/webhook", async (HttpRequest request) =>
            {
                string eventName = request.Headers["X-GitHub-Event"];
                string signature = request.Headers["X-Hub-Signature-256"];
                string json = await new StreamReader(request.Body).ReadToEndAsync();

                if (!GitHubSignature.Verify(json, signature, _secret))
                    return Results.Unauthorized();

                IGitHubEvent githubEvent = GitHubEventParser.Parse(json, eventName);

                if (githubEvent != null)
                    _dispatcher.Dispatch(githubEvent);

                return Results.Ok();
            });

            return app;
        }

        /// <summary>
        /// Starts a Webhook Receiver for GitHub Events.
        /// </summary>
        /// <param name="port">Port the Webhook Server is Exposed on</param>
        public void Start(int port = 8080)
        {
            WebApplication app = CreateWebhookApp();

            app.Run($"http://0.0.0.0:{port}");
        }

        /// <summary>
        /// Starts a Webhook Receiver Asynchronously for GitHub Events 
        /// </summary>
        /// <param name="port">Port the Webhook Server is Exposed on</param>
        /// <returns>Async Task hosting the Server</returns>
        public async Task StartAsync(int port = 8080)
        {
            WebApplication app = CreateWebhookApp();

            await app.RunAsync($"http://0.0.0.0:{port}");
        }
    }
}
