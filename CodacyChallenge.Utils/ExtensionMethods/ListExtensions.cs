using CodacyChallenge.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodacyChallenge.Utils
{
    public static class ListExtensions
    {
        public static List<List<T>> BreakInPages<T>(this List<T> source, int itemsPerPage)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / itemsPerPage)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static List<T> TakeLast<T>(this List<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N)).ToList();
        }

        public static List<T> Paginate<T>(this List<T> source, IPagination pagination)
        {
            return source
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize).ToList();
        }
    }
}
