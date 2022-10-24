using System;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Newtonsoft.Json;
using shedule_bot.backend;

namespace shedule_bot
{
    struct BotUpdates
    {
        public long chatId;
        public string? username;
        public string text;
    }

    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient(ToolChain.GetItemFromDotEnv("TOKEN"));
        static string fileName = "update.json";
        static List<BotUpdates> botUpdates = new List<BotUpdates>();

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                long chatId = message.Chat.Id;
                var cts = new CancellationTokenSource();

                //write updates to json
                var _botUpdates = new BotUpdates
                {
                    text = message.Text,
                    chatId = message.Chat.Id,
                    username = message.Chat.Username
                };
                Console.WriteLine($"username: {_botUpdates.username} | channel id: {_botUpdates.chatId} | text: {_botUpdates.text}");
                botUpdates.Add(_botUpdates);
                var botUpdatesString = JsonConvert.SerializeObject(botUpdates);
                System.IO.File.WriteAllText(fileName, botUpdatesString);

                switch (message.Text)
                {
                    case "/start":
                        ToolChain.GroupInline(bot, chatId, cts.Token);
                        break;


                    //it's code work but I'm not sure how correct and better i can do 
                    case "1KCM-A":
                        UserInterface.InitialCreateUser(_botUpdates.username, "1KCM-A");
                        await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Ви успішно вибрали групу: 1KCM-A\nЯкщо ви обрали не правильно або хочете змінити групу натисніть кнопку \"змінити групу\"");
                        ToolChain.MainInline(bot, chatId, cts.Token);
                        break;
                    case "1KCM-B":
                        UserInterface.InitialCreateUser(_botUpdates.username, "1KCM-B");
                        await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Ви успішно вибрали групу: 1KCM-B\nЯкщо ви обрали не правильно або хочете змінити групу натисніть кнопку \"змінити групу\"");
                        ToolChain.MainInline(bot, chatId, cts.Token);
                        break;
                    case "2KCM-A":
                        UserInterface.InitialCreateUser(_botUpdates.username, "2KCM-A");
                        await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Ви успішно вибрали групу: 2KCM-A\nЯкщо ви обрали не правильно або хочете змінити групу натисніть кнопку \"змінити групу\"");
                        ToolChain.MainInline(bot, chatId, cts.Token);
                        break;
                    case "2KCM-B":
                        UserInterface.InitialCreateUser(_botUpdates.username, "2KCM-B");
                        await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Ви успішно вибрали групу: 2KCM-B\nЯкщо ви обрали не правильно або хочете змінити групу натисніть кнопку \"змінити групу\"");
                        ToolChain.MainInline(bot, chatId, cts.Token);
                        break;
                    case "3KCM-A":
                        UserInterface.InitialCreateUser(_botUpdates.username, "3KCM-A");
                        await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Ви успішно вибрали групу: 3KCM-A\nЯкщо ви обрали не правильно або хочете змінити групу натисніть кнопку \"змінити групу\"");
                        ToolChain.MainInline(bot, chatId, cts.Token);
                        break;
                    case "3KCM-B":
                        UserInterface.InitialCreateUser(_botUpdates.username, "3KCM-B");
                        await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Ви успішно вибрали групу: 3KCM-B\nЯкщо ви обрали не правильно або хочете змінити групу натисніть кнопку \"змінити групу\"");
                        ToolChain.MainInline(bot, chatId, cts.Token);
                        break;
                    case "3KCM-11":
                        UserInterface.InitialCreateUser(_botUpdates.username, "3KCM-11");
                        await bot.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Ви успішно вибрали групу: 3KCM-11\nЯкщо ви обрали не правильно або хочете змінити групу натисніть кнопку \"змінити групу\"");
                        ToolChain.MainInline(bot, chatId, cts.Token);
                        break;


                    case "download":
                        {
                            await using Stream stream = System.IO.File.OpenRead(@"./main.xls");
                            await botClient.SendDocumentAsync(
                            chatId: chatId,
                            document: new InputOnlineFile(content: stream, fileName: "main.xls"),
                            caption: "Розклад на 1 семестр 2022",
                            cancellationToken: cancellationToken);
                            break;
                        }
                }
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Start listening for @{bot.GetMeAsync().Result.Username}");

            var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions { AllowedUpdates = { }, };
            bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cts.Token);
            Console.ReadLine();
        }
    }
}