using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace shedule_bot
{
    public class ToolChain
    {
        static ITelegramBotClient bot = new TelegramBotClient(ToolChain.GetItemFromDotEnv("TOKEN"));
        static CancellationTokenSource cts = new CancellationTokenSource();


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

        public static async void MainInline(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(
                new[]
                    {
                        new KeyboardButton[] { "download", "get shedule" },
                        new KeyboardButton[] { "create-test", "change-test", "test-view"},
                        new KeyboardButton[] { "Змінити групу" }
                    })
            { ResizeKeyboard = true };

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Choose a response",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }

        public static async void GroupInline(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(
                new[]
                    {
                        new KeyboardButton[] {"1KCM-A","1KCM-B", "2KCM-A","2KCM-B", "3KCM-A","3KCM-B","3KCM-11" },
                        // new KeyboardButton[] { "1AKIT", "2AKIT","3AKIT","4AKIT" },
                        // new KeyboardButton[] { "1ET", "2ET", "3"}
                    })
            { ResizeKeyboard = true };

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Оберіть вашу групу",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }
    }
}