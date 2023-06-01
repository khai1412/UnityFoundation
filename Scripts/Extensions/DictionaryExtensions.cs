namespace TheOneStudio.HyperCasual.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValueFactory = null)
        {
            return dictionary.TryGetValue(key, out var value) ? value : (defaultValueFactory ?? (() => default))();
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValueFactory = null)
        {
            return dictionary[key] = dictionary.GetOrDefault(key, defaultValueFactory);
        }

        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new(dictionary);
        }
    }
}