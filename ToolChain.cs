using Newtonsoft.Json;

namespace schedule_bot
{
    public class ToolChain
    {
        public static string GetItemFromDotEnv(string value) //can use for get data from .env file
        {
            DotNetEnv.Env.Load();
            string? item = Environment.GetEnvironmentVariable(value);
            return item;
        }

        static List<BotUpdates> botUpdates = new List<BotUpdates>();
        public static void ReadJson(string fileName)
        {
            try
            {
                string botUpdatesString = System.IO.File.ReadAllText(fileName);

                botUpdates = JsonConvert.DeserializeObject<List<BotUpdates>>(botUpdatesString) ?? botUpdates;
            }
            catch (System.Exception ex) { Console.WriteLine($"Error reading or deserializing: {ex}"); }
        }

    }
}