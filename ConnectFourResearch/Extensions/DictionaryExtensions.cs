using System;
using System.Collections.Generic;

namespace ConnectFourResearch.Extensions
{
    public static class DictionaryExtensions
    {
        public static TV GetOrCreate<TK, TV>(this IDictionary<TK, TV> d, TK key, Func<TK, TV> create)
        {
            return d.TryGetValue(key, out var v)
                ? v
                : d[key] = create(key);
        }

        public static TV GetOrDefault<TK, TV>(this IDictionary<TK, TV> d, TK key, TV def = default)
        {
            return d.TryGetValue(key, out var v) ? v : def;
        }
    }
}