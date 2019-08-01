using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodacyChallenge.API.Client;
using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using CodacyChallenge.Common.Models.Configuration;
using Microsoft.Extensions.Options;

namespace CodacyChallenge.Service.Implementations
{
    public class GitAPIEngine : IGitEngine
    {
        private ApiClient _apiClient;
        private GitHubEndpoints _gitHubSettings;

        public GitAPIEngine(IOptions<GitHubEndpoints> gitHubSettings)
        {
            _gitHubSettings = gitHubSettings.Value;
            _apiClient = new ApiClient();
        }

        public async Task<List<GitResponse>> GetAllCommits(string url)
        {
            var splittedUrl = url.Split('/').ToList().TakeLast(2);
            var requestUrl = string.Format(_gitHubSettings.GetAllCommits, splittedUrl.FirstOrDefault(), splittedUrl.LastOrDefault());
            var commits = await _apiClient.GetAsync<List<GitResponse>>(new Uri(requestUrl)).ConfigureAwait(false);

            return commits;
        }
    }
}
