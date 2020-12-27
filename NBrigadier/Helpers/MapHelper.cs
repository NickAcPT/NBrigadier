using System;
using System.Collections.Generic;
using System.Linq;

namespace NBrigadier.Helpers
{
    public static class MapHelper
    {
        public static bool Any<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            return source.Any(new Func<TSource, bool>(s => predicate(s)));
        }
        
        
        public static TV GetOrDefault<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, TV value = default)
        {
            return dictionary.TryGetValue(key, out var ret) ? ret : value;
        }
    }
}