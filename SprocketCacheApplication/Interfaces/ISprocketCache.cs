namespace SprocketCacheApplication.Interfaces
{
    public interface ISprocketCache<TSprocket>
        where TSprocket : class, ISprocket
    {
        Task<TSprocket> Get(string key);
    }
}
