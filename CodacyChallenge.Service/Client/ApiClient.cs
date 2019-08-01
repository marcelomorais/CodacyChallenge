using CodacyChallenge.Service.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodacyChallenge.API.Client
{
    public class ApiClient : IApiClient
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly Dictionary<string,string> headers = new Dictionary<string, string> { { "User-Agent", "request" } };

        public ApiClient(IHttpClientWrapper httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>  
        /// Common method for making GET calls  
        /// </summary>  
        public async Task<T> GetAsync<T>(string requestUrl)
        {
            _httpClient.AddHeaders(headers);
            var response = await _httpClient.GetAsync(requestUrl).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
