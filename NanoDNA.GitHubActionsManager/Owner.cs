using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NanoDNA.GitHubActionsManager
{
    /// <summary>
    /// Owner Information from the Webhook Payload
    /// </summary>
    public class Owner
    {
        /// <summary>
        /// Login Name of the Repository Owner
        /// </summary>
        [JsonProperty("login")]
        public string Login { get; set; }

        /// <summary>
        /// Extra Data provided to the Owner Object
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JToken> ExtraData { get; set; }
    }
}
