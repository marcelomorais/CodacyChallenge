using CodacyChallenge.Common.Enumerators;

namespace CodacyChallenge.Common.Models
{
    public class RequestObject
    {
        public RequestType RequestType { get; set; }
        public string url { get; set; }
    }
}
