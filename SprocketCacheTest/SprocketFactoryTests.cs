using SprocketCacheApplication.Entities;

namespace SprocketCacheTest
{
    public class SprocketCacheTests : IClassFixture<SprocketFactoryTestsFixture>
    {
        private readonly SprocketFactoryTestsFixture _fixture;

        public SprocketCacheTests(SprocketFactoryTestsFixture fixture)
        {
            this._fixture = fixture;
        }

        [Fact]
        public async Task CheckCacheReturnsSprocketInstanceByKey()
        {
            Sprocket newSprocket = await this._fixture.SprocketCache.Get("sprocket_1");

            Assert.NotNull(newSprocket);
        }

        [Fact]
        public async Task CheckCacheReturnsSameSprocketWithinExpirationPeriod()
        {
            string key = "sprocket_1";

            Sprocket sprocketOne = await this._fixture.SprocketCache.Get(key);
            Sprocket sprocketTwo = await this._fixture.SprocketCache.Get(key);

            Assert.Equal(sprocketOne, sprocketTwo);
        }

        [Fact]
        public async Task CheckCacheReturnsNewSprocketAfterPeriodIsExpired()
        {
            string key = "sprocket_1";

            Sprocket sprocketOne = await this._fixture.SprocketCache.Get(key);
            Thread.Sleep(this._fixture.TestExpirationPeriod + 100);
            Sprocket sprocketTwo = await this._fixture.SprocketCache.Get(key);

            Assert.NotEqual(sprocketOne, sprocketTwo);
        }

        [Fact]
        public async Task CheckCacheReturnsDifferentSprocketsForDifferentKeys()
        {
            string keyOne = "sprocket_1";
            string keyTwo = "sprocket_2";

            Sprocket sprocketOne = await this._fixture.SprocketCache.Get(keyOne);
            Sprocket sprocketTwo = await this._fixture.SprocketCache.Get(keyTwo);

            Assert.NotEqual(sprocketOne, sprocketTwo);
        }

        [Fact]
        public void CheckCacheIsThreadSafeWhenReturningSprocketInstance()
        {
            string key = "sprocket_1";

            Task<Sprocket> sprocketOne = Task.Run(async () => await this._fixture.SprocketCache.Get(key));
            Task<Sprocket> sprocketTwo = Task.Run(async () => await this._fixture.SprocketCache.Get(key));

            Task.WaitAll(sprocketOne, sprocketTwo);

            Assert.True(ReferenceEquals(sprocketOne.Result, sprocketTwo.Result));
        }
    }
}