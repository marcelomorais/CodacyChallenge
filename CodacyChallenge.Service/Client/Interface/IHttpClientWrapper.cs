using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodacyChallenge.Service.Client
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string url);

        void AddHeaders(Dictionary<string, string> keyValues);
    }
}