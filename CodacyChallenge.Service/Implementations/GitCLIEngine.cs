using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace CodacyChallenge.Service.Implementations
{
    public class GitCLIEngine : IGitEngine
    {
        public Task<List<GitResponse>> GetAllCommits(string url)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var commitList = new List<GitResponse>();
            Directory.CreateDirectory(tempDirectory);

            try
            {
                using (PowerShell powershell = PowerShell.Create())
                {
                    //I need to put the ^^ so that I can replace it to double quotes after the PowerShell return my object because the it's scaping the double quotes... 
                    var format = "{\"^^Commit^^\":\"^^%H^^\",\"^^Subject^^\":\"^^%s^^\",\"^^CommitNotes^^\":\"^^%N^^\",\"^^Author^^\":\"^^%aN^^\",\"^^Date^^\":\"^^%aD^^\"}";

                    powershell.AddScript($"{GitCommand.Clone} {url} '{tempDirectory}'");
                    powershell.AddScript($"cd '{tempDirectory}'");
                    powershell.AddScript($"{GitCommand.Log} {GitCommand.PrettyFormat(format)}");

                    var results = powershell.Invoke().ToList();
                    results.ForEach(x =>
                    {
                        commitList.Add(JsonConvert.DeserializeObject<GitResponse>(x.ToString().Replace("^^", "\"")));
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            /*TODO: Need to remove the temporary folder from the disk.
            //finally
            //{
            //  
            //    if (Directory.Exists(tempDirectory))
            //    {
            //        var dir = new DirectoryInfo(tempDirectory);
            //        dir.Attributes = FileAttributes.Normal;
            //        dir.Delete(true);
            //    }
            }*/

            return Task.FromResult(commitList);
        }
    }
}
