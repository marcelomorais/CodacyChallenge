using System;
using System.Collections.Generic;
using System.Text;

namespace CodacyChallenge.Common.Interfaces
{
    public interface IPagination
    {
        int PageSize { get; set; }
        int PageNumber { get; set; }
    }
}
