using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json;
namespace scheduleBot
{
    public class ToolChain
    {
        public static string GetToken() //can use for get data from .env file
        {
            DotNetEnv.Env.Load();
            string? token = Environment.GetEnvironmentVariable("TOKEN");
            return token;
        }

        static List<BotUpdates> botUpdates = new List<BotUpdates>();
        public static void ReadJson(string fileName)
        {
            try
            {
                string botUpdatesString = System.IO.File.ReadAllText(fileName);

                botUpdates = JsonConvert.DeserializeObject<List<BotUpdates>>(botUpdatesString) ?? botUpdates;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error reading or deserializing: {ex}");
            }
        }

    }
}