using SprocketCacheApplication.Interfaces;

namespace SprocketCacheApplication.Entities
{
    public class SprocketDictionaryCache<TSprocket> : ISprocketCache<TSprocket>
        where TSprocket : class, ISprocket
    {
        private const int DEFAULT_EXPIRATION_PERIOD = 300;

        private class SprocketCacheEntry<TSprocket>
        {
            public TSprocket Sprocket { get; set; } = default!;
            public DateTime CreationTime { get; set; }
        }

        private readonly Dictionary<string, SprocketCacheEntry<TSprocket>> _cache = new();

        private static readonly SemaphoreSlim semaphoreSlim = new(1);

        private readonly int _expirationPeriod;
        private readonly ISprocketFactory<TSprocket> _sprocketFactory;

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
                if (_cache.TryGetValue(key, out SprocketCacheEntry<TSprocket>? entry))
                {
                    if (DateTime.Now < entry.CreationTime.AddMilliseconds(this._expirationPeriod))
                    {
                        return entry.Sprocket;
                    }
                }

                TSprocket newSprocket = await this._sprocketFactory.CreateSprocket();
                _cache[key] = new SprocketCacheEntry<TSprocket>() { Sprocket = newSprocket, CreationTime = DateTime.Now };

                return newSprocket;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}
