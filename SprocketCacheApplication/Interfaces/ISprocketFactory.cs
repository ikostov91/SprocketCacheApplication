namespace SprocketCacheApplication.Interfaces
{
    public interface ISprocketFactory<TSprocket>
        where TSprocket : class, ISprocket
    {
        Task<TSprocket> CreateSprocket();
    }
}
