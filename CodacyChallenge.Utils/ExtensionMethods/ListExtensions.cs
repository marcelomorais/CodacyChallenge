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
    }
}
