﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NBrigadier.Helpers
{
    public static class CollectionsHelper
    {
        public static List<T> AsList<T>(params T[] values) => values.ToList();

        public static ICollection<T> AddAll<T>(ICollection<T> original, ICollection<T> values)
        {
            foreach (var value in values) original.Add(value);

            return original;
        }
        public static List<T> EmptyList<T>() => new();
        public static List<T> SingletonList<T>(T value) => new() {value};

        public static ICollection<T> Sort<T>(this ICollection<T> value, Func<T, T, int> sortFunc) =>
            value.OrderBy(c => c, Comparer<T>.Create((x, y) => sortFunc(x, y))).ToList();

        public static bool IsEmpty<TK, TV>(this IDictionary<TK, TV> value) => value.Count == 0;
        
        public static IDictionary<TK, TV> EmptyMap<TK, TV>() => new Dictionary<TK, TV>();
    }
}