using System.IO;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace scheduleBot
{

    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("TOKEN");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var chatId = message.Chat.Id;
                switch (message.Text.ToLower())
                {
                    case "/start":
                        MainInline(botClient: botClient, chatId: chatId, cancellationToken: cancellationToken);
                        break;
                    case "test1":
                        {
                            Console.WriteLine("Нажата Кнопка 1");
                            bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ви натиснули Кнопку 1");
                            break;
                        }
                    case "test2":
                        {
                            Console.WriteLine("Нажата Кнопка 2");
                            bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ви натиснули Кнопку 2");
                            break;
                        }
                    case "download":
                        {
                            Console.WriteLine("Нажата кнопка download");

                            await using Stream stream = System.IO.File.OpenRead(@"./main.xls");
                            botClient.SendDocumentAsync(
                            chatId: chatId,
                            document: new InputOnlineFile(content: stream, fileName: "main.xls"),
                            caption: "Розклад на 1 семестр 2022",
                            cancellationToken: cancellationToken);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        public static async void MainInline(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(
                new[]
                    {
                        new KeyboardButton[] {"test1", "test2", "download" }
                        //new KeyboardButton[] { "Download" }
                    }
                    )
            { ResizeKeyboard = true };

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Choose a response",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }
        static void Main(string[] args)
        {
            Console.WriteLine($"Start listening for @{bot.GetMeAsync().Result.Username}");

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions { AllowedUpdates = { }, };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}
<<<<<<< HEAD
=======

>>>>>>> b8a913d (update)
