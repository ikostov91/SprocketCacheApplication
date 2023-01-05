using SprocketCacheApplication.Entities;
using SprocketCacheApplication.Interfaces;

namespace SprocketCacheTest
{
    public class SprocketCacheTests
    {
        private readonly int _testExpirationPeriod = 400;
        private readonly ISprocketCache<Sprocket> _sprocketCache;

        public SprocketCacheTests()
        {
            this._sprocketCache = new SprocketDictionaryCache<Sprocket>(new SprocketFactory<Sprocket>(), this._testExpirationPeriod);
        }

        [Fact]
        public async Task CheckCacheReturnsSprocketInstanceByKey()
        {
            Sprocket newSprocket = await this._sprocketCache.Get("sprocket_1");

            Assert.NotNull(newSprocket);
        }

        [Fact]
        public async Task CheckCacheReturnsSameSprocketWithinExpirationPeriod()
        {
            string key = "sprocket_1";

            Sprocket sprocketOne = await this._sprocketCache.Get(key);
            Sprocket sprocketTwo = await this._sprocketCache.Get(key);

            Assert.Equal(sprocketOne, sprocketTwo);
        }

        [Fact]
        public async Task CheckCacheReturnsNewSprocketAfterPeriodIsExpired()
        {
            string key = "sprocket_1";

            Sprocket sprocketOne = await this._sprocketCache.Get(key);
            Thread.Sleep(500);
            Sprocket sprocketTwo = await this._sprocketCache.Get(key);

            Assert.NotEqual(sprocketOne, sprocketTwo);
        }

        [Fact]
        public async Task CheckCacheReturnsDifferentSprocketsForDifferentKeys()
        {
            string keyOne = "sprocket_1";
            string keyTwo = "sprocket_2";

            Sprocket sprocketOne = await this._sprocketCache.Get(keyOne);
            Sprocket sprocketTwo = await this._sprocketCache.Get(keyTwo);

            Assert.NotEqual(sprocketOne, sprocketTwo);
        }

        [Fact]
        public async Task CheckCacheIsThreadSafeWhenReturningSprocketInstance()
        {
            string key = "sprocket_1";

            Task<Sprocket> sprocketOne = Task.Run(async () => await this._sprocketCache.Get(key));
            Task<Sprocket> sprocketTwo = Task.Run(async () => await this._sprocketCache.Get(key));

            //Sprocket sprocketOne = await this._sprocketCache.Get(key);
            //Thread.Sleep(10);
            //Sprocket sprocketTwo = await this._sprocketCache.Get(key);

            Task.WaitAll(sprocketOne, sprocketTwo);

            Assert.True(ReferenceEquals(sprocketOne.Result, sprocketTwo.Result));
        }
    }
}