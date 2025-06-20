// MIT License
// 
// Copyright (c) 2025 LoveDoLove
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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