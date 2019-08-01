using System;
using System.Threading.Tasks;

namespace CodacyChallenge.API.Client
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string requestUrl);
    }
}
