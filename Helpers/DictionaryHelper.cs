using System;
using System.Collections.Generic;

namespace ExtraLethalCompany.Helpers
{
    public static class DictionaryHelper
    {
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> creationLogic)
        {
            if (!dictionary.TryGetValue(key, out TValue value))
                dictionary.Add(key, value = creationLogic());

            return value;
        }

        public static TValue GetOrCreateNew<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            if (!dictionary.TryGetValue(key, out TValue value))
                dictionary.Add(key, value = new TValue());

            return value;
        }

        public static TValue GetOrCreateDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (!dictionary.TryGetValue(key, out TValue value))
                dictionary.Add(key, value = default);

            return value;
        }
    }
}
