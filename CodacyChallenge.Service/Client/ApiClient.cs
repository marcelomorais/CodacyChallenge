using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodacyChallenge.API.Client
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private Uri BaseEndpoint { get; set; }

        public ApiClient()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>  
        /// Common method for making GET calls  
        /// </summary>  
        public async Task<T> GetAsync<T>(Uri requestUrl)
        {
            addHeaders();
            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(data);
        }

        private void addHeaders()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "request");
        }
    }
}
