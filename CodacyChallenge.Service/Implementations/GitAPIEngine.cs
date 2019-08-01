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

        public async Task<List<GitResponse>> GetCommitsWithPagination(RequestObject request)
        {
            var splittedUrl = request.Url.Split('/').ToList().TakeLast(2);
            var requestUrl = string.Concat(string.Format(_gitHubSettings.GetAllCommits, splittedUrl.FirstOrDefault(), splittedUrl.LastOrDefault()), $"?per_page={request.PageSize}");
            var commits = await _apiClient.GetAsync<List<GitResponse>>(new Uri(requestUrl)).ConfigureAwait(false);

            return commits;
        }


    }
}
