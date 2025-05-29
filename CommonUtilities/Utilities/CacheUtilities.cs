using System.Runtime.Caching;

namespace CommonUtilities.Utilities;

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