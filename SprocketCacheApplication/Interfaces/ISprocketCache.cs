namespace SprocketCacheApplication.Interfaces
{
    public interface ISprocketCache<TSprocket> where TSprocket : class
    {
        Task<TSprocket> Get(string key);
    }
}
