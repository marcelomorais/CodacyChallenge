using CodacyChallenge.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodacyChallenge.Common.Interfaces
{
    public interface IGitEngine
    {
        Task<List<GitResponse>> GetCommitsWithPagination(RequestObject request);
    }
}
