using SprocketCacheApplication.Entities;

namespace SprocketCacheApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sprocketCache = new SprocketDictionaryCache<Sprocket>(new SprocketFactory<Sprocket>());

            //var sprocket1 = sprocketCache.Get("sprocket_1");

            //var sprocket2 = sprocketCache.Get("sprocket_2");

            Task<Sprocket> sprocketOne = Task.Run(async () => await sprocketCache.Get("sprocket_1"));
            Task <Sprocket> sprocketTwo = Task.Run(async () => await sprocketCache.Get("sprocket_1"));
            Task <Sprocket> sprocketthree = Task.Run(async () => await sprocketCache.Get("sprocket_2"));

            Task.WaitAll(sprocketOne, sprocketTwo, sprocketthree);

            //Task task3 = Task.Run(() => TestMethod(ref isExecuted, lockObject));

            //Task.WaitAll(task1, task2, task3);
        }

        private static void TestMethod(ref bool isExecuted, object lockObject)
        {
            lock (lockObject)
            {
                if (!isExecuted)
                {
                    Console.WriteLine("EXECUTED!!!!");
                    isExecuted = true;
                }
            }
        }
    }
}