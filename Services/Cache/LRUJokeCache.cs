using JokeApi.Models;

namespace JokeApi.Services.Cache
{
    public class LRUJokeCache : IJokeCache
    {
        private readonly Dictionary<string, Dictionary<JokeLengthCategory, List<Joke>>> _cache;
        private readonly LinkedList<string> _accessOrder;
        private readonly int _maxCacheSize;
        private readonly ILogger _logger;

        public LRUJokeCache(int maxCacheSize, ILogger logger)
        {
            _cache = new Dictionary<string, Dictionary<JokeLengthCategory, List<Joke>>>();
            _accessOrder = new LinkedList<string>();
            _maxCacheSize = maxCacheSize;
            _logger = logger;
        }

        public Dictionary<JokeLengthCategory, List<Joke>> Get(string key)
        {
            if (_cache.TryGetValue(key, out var cachedResults))
            {
                // Update the access order
                _accessOrder.Remove(key);
                _accessOrder.AddFirst(key);

                _logger.LogInformation("Cache hit for key: {Key}", key);

                return cachedResults;
            }

            _logger.LogInformation("Cache miss for key: {Key}", key);

            return null;
        }

        public void Set(string key, Dictionary<JokeLengthCategory, List<Joke>> value)
        {
            // Add the value to the cache
            _cache[key] = value;

            // Update the access order
            _accessOrder.Remove(key);
            _accessOrder.AddFirst(key);

            // If the cache size exceeds the maximum limit, remove the least recently used term
            if (_cache.Count > _maxCacheSize)
            {
                var termToRemove = _accessOrder.Last.Value;
                _accessOrder.RemoveLast();
                _cache.Remove(termToRemove);
            }

            _logger.LogInformation("Cached value for key: {Key}", key);
        }
    }
}
