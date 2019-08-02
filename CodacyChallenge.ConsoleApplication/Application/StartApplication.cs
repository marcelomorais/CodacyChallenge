using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using CodacyChallenge.Service.Client.Interface;
using CodacyChallenge.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace CodacyChallenge.Application
{
    public class StartApplication : IStartApplication
    {
        private IGitEngine _gitEngine;
        private RequestObject _request;
        private int _totalPages = 0;
        private IMemoryCacheWrapper _cache;

        public StartApplication(IGitEngine gitEngine, IMemoryCacheWrapper cache)
        {
            _cache = cache;
            _gitEngine = gitEngine;
            _request = new RequestObject();
        }
        public async Task Execute(string url)
        {
            Console.WriteLine("Pulling commit list, it may take a few minutes...\n");

            _request.Url = url;

            await _gitEngine.GetCommitsWithPagination(_request);

            var cachedCommitList = _cache.Get<List<GitResponse>>(_request.Url);

            _totalPages = (cachedCommitList.Count + _request.PageSize - 1) / _request.PageSize;

            if (cachedCommitList.Any())
            {
                CallNextPage(cachedCommitList);
            }
        }

        private void CallNextPage(List<GitResponse> commitList)
        {
            var paginatedCommits = commitList.Paginate(_request);

            if (_request.PageNumber <= _totalPages)
            {
                paginatedCommits.ForEach(x =>
                {
                    Console.WriteLine(JsonConvert.SerializeObject(x));
                });

                Console.WriteLine($"\nThis Repository has a total of {_totalPages} pages with max of {_request.PageSize} elements per page.");
                Console.WriteLine($"You are on the page: {_request.PageNumber}.\n");
                Console.WriteLine($"N - Next Page\nX - Exit\nOr enter the page number.");
                Console.WriteLine($"Press X to exit.\n");
            }
            else
            {
                Console.WriteLine($"\nThe page {_request.PageNumber} does not exist.\n");
                Console.WriteLine($"Please try a different page number or enter X to exit.\n");
            }


            CheckPressedKey();
            CallNextPage(commitList);
        }

        private void CheckPressedKey()
        {
            int result = _request.PageNumber;
            string keyPressed = string.Empty;

            while (!(keyPressed == "n" || int.TryParse(keyPressed, out result)))
            {
                keyPressed = Console.ReadLine().ToLowerInvariant();

                if (keyPressed == "x")
                {
                    Environment.Exit(0);
                }
            }

            _request.PageNumber = keyPressed == "n" ? _request.PageNumber + 1 : result;
        }
    }
}
