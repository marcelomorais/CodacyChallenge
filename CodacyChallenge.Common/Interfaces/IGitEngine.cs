using CodacyChallenge.Common.Models;
using System.Collections.Generic;

namespace CodacyChallenge.Common.Interfaces
{
    public interface IGitEngine
    {
        List<GitCommit> GetAllCommits(string url);
    }
}
