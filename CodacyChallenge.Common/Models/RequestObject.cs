using CodacyChallenge.Common.Enumerators;
using CodacyChallenge.Common.Interfaces;

namespace CodacyChallenge.Common.Models
{
    public class RequestObject : IPagination
    {
        public string Url { get; set; }
        public RequestType RequestType { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
