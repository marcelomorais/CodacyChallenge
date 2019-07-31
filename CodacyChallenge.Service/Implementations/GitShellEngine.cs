using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace CodacyChallenge.Service
{
    public class GitCLIEngine : IGitEngine
    {
        public List<GitCommit> GetAllCommits(string url)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var commitList = new List<GitCommit>();
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
                        commitList.Add(JsonConvert.DeserializeObject<GitCommit>(x.ToString().Replace("^^", "\"")));
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //Need to remove the temporary folder.

                //if (Directory.Exists(tempDirectory))
                //{
                //    var dir = new DirectoryInfo(tempDirectory);
                //    dir.Attributes = FileAttributes.Normal;
                //    dir.Delete(true);
                //}
            }

            return commitList;
        }
    }
}
