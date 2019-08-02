using CodacyChallenge.Service.Client.Interface;
using System;
using System.Runtime.Caching;

namespace CodacyChallenge.Service.Client
{
    public class MemoryCacheWrapper : IMemoryCacheWrapper
    {
        private MemoryCache _memoryCache;

        public MemoryCacheWrapper(string cacheName)
        {
            _memoryCache = new MemoryCache(cacheName);
        }

        public void Add(string key, object value)
        {
            _memoryCache.Add(key, value, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromMinutes(1) });
        }

        public T Get<T>(string key)
        {
            return (T)_memoryCache.Get(key);
        }
    }
}
