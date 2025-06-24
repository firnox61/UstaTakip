using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.RegularExpressions;
using UstaTakip.Core.Utilities.IoC;

namespace UstaTakip.Infrastructure.Caching
{
    public class MemoryCacheManager : ICacheManager
    {
        IMemoryCache _memoryCache;

        public MemoryCacheManager()
        {
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
        }

        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            var cacheEntriesCollectionDefinition = typeof(MemoryCache)
        .GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

            if (cacheEntriesCollectionDefinition == null)
            {
                Console.WriteLine("⚠️ Cache sisteminde EntriesCollection özelliği erişilemiyor. Cache temizlenemedi.");
                return;
            }

            var entries = cacheEntriesCollectionDefinition.GetValue(_memoryCache) as dynamic;
            if (entries == null)
            {
                Console.WriteLine("⚠️ Cache boş veya giriş koleksiyonu alınamadı.");
                return;
            }

            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in entries)
            {
                ICacheEntry entry = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(entry);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues
                .Where(entry => regex.IsMatch(entry.Key.ToString()))
                .Select(entry => entry.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
                Console.WriteLine($"🗑 Cache kaldırıldı: {key}");
            }
        }
    }
}
