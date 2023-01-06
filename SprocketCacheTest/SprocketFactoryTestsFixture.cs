using SprocketCacheApplication.Entities;
using SprocketCacheApplication.Interfaces;
namespace SprocketCacheTest
{
    public class SprocketFactoryTestsFixture
    {
        public SprocketFactoryTestsFixture()
        {
            this.SprocketCache = new SprocketDictionaryCache<Sprocket>(new SprocketFactory<Sprocket>(), this.TestExpirationPeriod);
        }

        public readonly int TestExpirationPeriod = 400;
        public readonly ISprocketCache<Sprocket> SprocketCache;
    }
}
