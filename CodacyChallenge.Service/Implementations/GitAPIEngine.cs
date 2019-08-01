using CodacyChallenge.API.Client;
using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using CodacyChallenge.Common.Models.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodacyChallenge.Service.Implementations
{
    public class GitAPIEngine : IGitEngine
    {
        private IApiClient _apiClient;
        private GitHubEndpoints _gitHubSettings;
        //private IMemoryCache _cache;

        public GitAPIEngine(IOptions<GitHubEndpoints> gitHubSettings, IApiClient apiClient)//IMemoryCache memoryCache)
        {
            _gitHubSettings = gitHubSettings.Value;
            _apiClient = apiClient;
            //_cache = memoryCache;
        }

        public async Task<List<GitResponse>> GetCommitsWithPagination(RequestObject request)
        {
            var sha = string.Empty;

            var splittedUrl = request.Url.Split('/').ToList().TakeLast(2);
            var requestUrl = string.Concat(string.Format(_gitHubSettings.GetAllCommits, splittedUrl.FirstOrDefault(), splittedUrl.LastOrDefault()), $"?per_page={request.PageSize}");

            //CheckCache(request, ref sha, ref requestUrl);

            var commits = await _apiClient.GetAsync<List<GitResponse>>(requestUrl).ConfigureAwait(false);

            //_cache.Set(nameof(GitResponse.Sha), commits.LastOrDefault().Sha);
            //_cache.Set(nameof(request.PageNumber), request.PageNumber);
            //_cache.Set(nameof(request.Url), request.Url);

            return commits;
        }

        //private void CheckCache(RequestObject request, ref string sha, ref string requestUrl)
        //{
        //    if (_cache.TryGetValue(nameof(request.PageNumber), out int pageNumber) && _cache.TryGetValue(nameof(request.Url), out string url))
        //    {
        //        if (url != request.Url)
        //        {
        //            _cache.Remove(nameof(request.PageNumber));
        //            _cache.Remove(nameof(GitResponse.Sha));
        //        }
        //        else
        //        {
        //            if (pageNumber != request.PageNumber)
        //            {
        //                _cache.Set(nameof(request.PageNumber), request.PageNumber);
        //                sha = _cache.Get(nameof(GitResponse.Sha)).ToString();
        //                requestUrl += $"&sha={sha}";
        //            }
        //        }
        //    }
        //}
    }
}
