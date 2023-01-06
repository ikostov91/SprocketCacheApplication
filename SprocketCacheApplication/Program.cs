using SprocketCacheApplication.Entities;

namespace SprocketCacheApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sprocketCache = new SprocketDictionaryCache<Sprocket>(new SprocketFactory<Sprocket>());

            Task<Sprocket> sprocketOne = Task.Run(async () => await sprocketCache.Get("sprocket_1"));
            Task<Sprocket> sprocketTwo = Task.Run(async () => await sprocketCache.Get("sprocket_1"));
            Task<Sprocket> sprocketthree = Task.Run(async () => await sprocketCache.Get("sprocket_2"));

            Task.WaitAll(sprocketOne, sprocketTwo, sprocketthree);
        }
    }
}