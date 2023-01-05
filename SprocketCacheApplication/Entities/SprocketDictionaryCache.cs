using SprocketCacheApplication.Interfaces;
using System.Threading;

namespace SprocketCacheApplication.Entities
{
    public class SprocketDictionaryCache<TSprocket> : ISprocketCache<TSprocket> where TSprocket : class
    {
        private class SprocketCacheEntry<TSprocket>
        {
            public TSprocket Sprocket { get; set; } = default!;
            public DateTime CreationDate { get; set; }
        }

        private const int DEFAULT_EXPIRATION_PERIOD = 300;

        private readonly int _expirationPeriod;
        private readonly ISprocketFactory<TSprocket> _sprocketFactory;
        private readonly object lockObject = new();

        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

        private readonly Dictionary<string, SprocketCacheEntry<TSprocket>> _cache = new();

        public SprocketDictionaryCache(ISprocketFactory<TSprocket> sprocketFactory)
            : this(sprocketFactory, DEFAULT_EXPIRATION_PERIOD)
        { }

        public SprocketDictionaryCache(ISprocketFactory<TSprocket> sprocketFactory, int expirationPeriod)
        {
            if (sprocketFactory is null)
            {
                throw new ArgumentNullException(nameof(sprocketFactory));
            }

            if (expirationPeriod <= 0)
            {
                throw new ArgumentException("Expiration period cannot be negative or zero.");
            }

            this._sprocketFactory = sprocketFactory;
            this._expirationPeriod = expirationPeriod;
        }

        public async Task<TSprocket> Get(string key)
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                Console.WriteLine($"CHECK FOR SPROCKET: {key}");
                if (_cache.TryGetValue(key, out SprocketCacheEntry<TSprocket>? entry))
                {
                    Console.WriteLine($"SPROCKET EXISTING: {key}");
                    DateTime currentTime = DateTime.Now;
                    if (currentTime < entry.CreationDate.AddMilliseconds(this._expirationPeriod))
                    {
                        Console.WriteLine($"RETURN EXISTING SPROCKET: {key}");
                        return entry.Sprocket;
                    }
                }

                Console.WriteLine($"CREATE NEW SPROCKET: {key}");
                TSprocket newSprocket = await this._sprocketFactory.CreateSprocket();

                _cache[key] = new SprocketCacheEntry<TSprocket>() { Sprocket = newSprocket, CreationDate = DateTime.Now };

                return newSprocket;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
