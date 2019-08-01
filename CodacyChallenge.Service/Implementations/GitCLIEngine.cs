using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using CodacyChallenge.Common.Models.Exceptions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace CodacyChallenge.Service.Implementations
{
    public class GitCLIEngine : IGitEngine
    {
        //This Class will return a simplified version of the commits using the pretty format built on GitCommand class.
        public Task<List<GitResponse>> GetCommitsWithPagination(RequestObject request)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var commitList = new List<GitResponse>();
            Directory.CreateDirectory(tempDirectory);

            using (PowerShell powershell = PowerShell.Create())
            {
                powershell.AddScript($"{GitCommand.Clone} {request.Url} '{tempDirectory}'");
                powershell.AddScript($"cd '{tempDirectory}'");
                powershell.AddScript($"{GitCommand.Log} {GitCommand.PrettyFormat}");

                var results = powershell.Invoke().ToList();

                //This condition is because the powershell identify the clone command as an error and always return the "HadError" property as true
                //so I need to validate if we have more than one error in our stream.
                if (!results.Any() && powershell.HadErrors)
                {
                    throw new CLIException(powershell.Streams);
                }

                results.ForEach(x =>
                {
                    commitList.Add(JsonConvert.DeserializeObject<GitResponse>(x.ToString().Replace("^^", "\"")));
                });
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
