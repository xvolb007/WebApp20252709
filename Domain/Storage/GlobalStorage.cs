using System.Collections.Concurrent;

namespace Domain.Storage
{
    public class GlobalStorage
    {
        private static readonly ConcurrentDictionary<int, (decimal Value, DateTime LastUpdated)> _storage
        = new ConcurrentDictionary<int, (decimal, DateTime)>();

        public static (decimal Value, DateTime LastUpdated)? Get(int key)
        {
            return _storage.TryGetValue(key, out var value) ? value : null;
        }

        public static void Set(int key, decimal value)
        {
            _storage[key] = (value, DateTime.UtcNow);
        }
    }
}
