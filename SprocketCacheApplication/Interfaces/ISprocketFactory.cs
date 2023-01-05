
namespace SprocketCacheApplication.Interfaces
{
    public interface ISprocketFactory<TSprocket> where TSprocket : class
    {
        Task<TSprocket> CreateSprocket();
    }
}
