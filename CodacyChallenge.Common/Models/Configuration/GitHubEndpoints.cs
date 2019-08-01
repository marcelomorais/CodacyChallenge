using Newtonsoft.Json;

namespace CodacyChallenge.Common.Models.Configuration
{
    [JsonObject("GitHubApi")]
    public class GitHubEndpoints
    {
        public string GetAllCommits { get; set; }
    }
}
