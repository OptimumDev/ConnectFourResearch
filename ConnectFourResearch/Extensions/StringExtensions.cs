using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectFourResearch.Extensions
{
    public static class StringExtensions
    {
        public static string StrJoin<T>(this IEnumerable<T> items, string delimiter)
        {
            return string.Join(delimiter, items);
        }

        public static string StrJoin<T>(this IEnumerable<T> items, 
            string delimiter, Func<T, string> toString)
        {
            return items.Select(toString).StrJoin(delimiter);
        }
        
        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }
    }
}