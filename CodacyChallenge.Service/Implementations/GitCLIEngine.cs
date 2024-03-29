﻿using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using CodacyChallenge.Common.Models.Exceptions;
using CodacyChallenge.Service.Client;
using CodacyChallenge.Service.Client.Interface;
using CodacyChallenge.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace CodacyChallenge.Service.Implementations
{
    public class GitCLIEngine : IGitEngine
    {
        private IPowershellWrapper _powershell;
        private static IMemoryCacheWrapper _cache;

        public GitCLIEngine(IPowershellWrapper powershell, IMemoryCacheWrapper cache)
        {
            _cache = cache;
            _powershell = powershell;
        }

        //This Class will return a simplified version of the commits using the pretty format built on GitCommand class.
        public Task<List<GitResponse>> GetCommitsWithPagination(RequestObject request)
        {
            var commitList = new List<GitResponse>();
            var cachedCommitList = _cache.Get<List<GitResponse>>(request.Url);

            if (cachedCommitList != null)
            {
                commitList = cachedCommitList.Paginate(request);
            }
            else
            {
                var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(tempDirectory);

                _powershell.AddScript($"{GitCommand.Clone} {request.Url} '{tempDirectory}'",
                    $"cd '{tempDirectory}'",
                    $"{GitCommand.Log} {GitCommand.PrettyFormat}");

                var results = _powershell.Invoke();

                //This condition is because the powershell identify the clone command as an error and always return the "HadError" property as true
                //so I need to check if we have any record to validate if it's a false positive or not.
                if (!results.Any() && _powershell.HadErrors)
                {
                    throw new CLIException(_powershell.StreamErrors);
                }

                _powershell.Dispose();

                results.ForEach(x =>
                {
                    commitList.Add(JsonConvert.DeserializeObject<GitResponse>(x.ToString().Replace("^^", "\"")));
                });

                _cache.Add(request.Url, commitList);

                commitList = commitList.Paginate(request);
            }
            /*TODO: Need to remove the temporary folder from the disk.
            //  
            //    if (Directory.Exists(tempDirectory))
            //    {
            //        var dir = new DirectoryInfo(tempDirectory);
            //        dir.Attributes = FileAttributes.Normal;
            //        dir.Delete(true);
            //    }
            */

            return Task.FromResult(commitList);
        }
    }
}
