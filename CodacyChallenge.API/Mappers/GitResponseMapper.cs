using CodacyChallenge.Common.Interfaces;
using CodacyChallenge.Common.Models;
using System.Collections.Generic;

namespace CodacyChallenge.API.Mappers
{
    public static class GitResponseMapper
    {
        public static ResponseObject ToResponseObject(this List<GitResponse> responseList, IPagination pagination)
        {
            return new ResponseObject
            {
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                Response = responseList
            };
        }
    }
}
