using Newtonsoft.Json;

namespace CodacyChallenge.ConsoleApplication.Configuration
{
    [JsonObject("Config")]
    public class Configuration
    {
        public int ItemsPerPage { get; set; }
    }
}
