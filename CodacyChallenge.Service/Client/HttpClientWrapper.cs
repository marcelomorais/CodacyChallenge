using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodacyChallenge.Service.Client
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private HttpClient _client;

        public HttpClientWrapper(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }

        public void AddHeaders(Dictionary<string, string> keyValues)
        {
            foreach (var item in keyValues)
            {
                _client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }
        }

    }
}
