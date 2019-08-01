using CodacyChallenge.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodacyChallenge.Utils
{
    public static class ListExtensions
    {
        //This method is used only on Console Application so that the pagination can be controlled by the configuration file "config.json"
        //public static List<List<T>> BreakInPages<T>(this List<T> source, int itemsPerPage)
        //{
        //    return source
        //        .Select((x, i) => new { Index = i, Value = x })
        //        .GroupBy(x => x.Index / itemsPerPage)
        //        .Select(x => x.Select(v => v.Value).ToList())
        //        .ToList();
        //}

        public static List<T> Paginate<T>(this List<T> source, IPagination pagination)
        {
            return source
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize).ToList();
        }
    }
}
