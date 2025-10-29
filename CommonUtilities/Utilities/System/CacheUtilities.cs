// Copyright 2025 LoveDoLove
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Runtime.Caching;

namespace CommonUtilities.Utilities.System;

/// <summary>
///     Provides utility methods for caching data in memory.
/// </summary>
public static class CacheUtilities
{
    private static readonly ObjectCache Cache = MemoryCache.Default;

    /// <summary>
    ///     Gets an item from the cache. If the item does not exist, it adds the item to the cache using the provided value
    ///     factory.
    /// </summary>
    /// <typeparam name="T">The type of the item to get or add.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="valueFactory">A function that produces the value to add to the cache if the key is not found.</param>
    /// <param name="expiration">The timespan after which the cache item will expire.</param>
    /// <returns>The cached item.</returns>
    public static T GetOrAdd<T>(string key, Func<T> valueFactory, TimeSpan expiration)
    {
        if (Cache.Contains(key)) return (T)Cache[key];

        var value = valueFactory();
        Cache.Add(key, value, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.Add(expiration) });
        return value;
    }

    /// <summary>
    ///     Removes an item from the cache.
    /// </summary>
    /// <param name="key">The cache key of the item to remove.</param>
    public static void Remove(string key)
    {
        if (Cache.Contains(key)) Cache.Remove(key);
    }

    /// <summary>
    ///     Clears all items from the cache.
    /// </summary>
    public static void ClearAll()
    {
        var cacheKeys = new List<string>(Cache.Select(kvp => kvp.Key));
        foreach (var key in cacheKeys) Cache.Remove(key);
    }
}