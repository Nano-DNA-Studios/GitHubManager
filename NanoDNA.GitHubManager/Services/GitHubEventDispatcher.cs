using System;
using System.Collections.Generic;
using NanoDNA.GitHubManager.Interfaces;

namespace NanoDNA.GitHubManager.Services
{
    /// <summary>
    /// Dispatches GitHub Events to the Appropriate Handlers
    /// </summary>
    public class GitHubEventDispatcher
    {
        /// <summary>
        /// Dictionary of Handlers for Specific Events
        /// </summary>
        private readonly Dictionary<string, List<Action<IGitHubEvent>>> _handlers = new();

        /// <summary>
        /// Registers a Handler for a Specific Event
        /// </summary>
        /// <typeparam name="T">Type of Event the Handler is Subcribed to</typeparam>
        /// <param name="handler">Function / Handler to run once the Event is dispatched</param>
        public void On<T>(Action<T> handler) where T : IGitHubEvent
        {
            string key = typeof(T).Name;
            if (!_handlers.ContainsKey(key))
                _handlers[key] = new();

            _handlers[key].Add(e => handler((T)e));
        }

        /// <summary>
        /// Dispatches the Event Received to the Appropriate Handler
        /// </summary>
        /// <param name="githubEvent">Event to Dispatch</param>
        public void Dispatch(IGitHubEvent githubEvent)
        {
            string key = githubEvent.GetType().Name;
            if (_handlers.TryGetValue(key, out List<Action<IGitHubEvent>> handlers))
            {
                foreach (Action<IGitHubEvent> handler in handlers)
                    handler(githubEvent);
            }
        }
    }
}
