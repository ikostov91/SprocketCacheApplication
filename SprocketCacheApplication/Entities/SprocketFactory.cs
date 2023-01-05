using SprocketCacheApplication.Interfaces;

namespace SprocketCacheApplication.Entities
{
    public class SprocketFactory<TSprocket> : ISprocketFactory<TSprocket>
        where TSprocket : class, new()
    {
        public async Task<TSprocket> CreateSprocket()
        {
            return await Task.FromResult(new TSprocket());
        }
    }
}
