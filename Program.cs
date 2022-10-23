﻿using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Newtonsoft.Json;
using shedule_bot.backend;

namespace schedule_bot
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

                //responce on different command
                switch (message.Text.ToLower())
                {
                    case "/start":
                        string chooseGroup = UserInterface.InitialChooseUserGroup();
                        UserInterface.InitialCreateUser(_botUpdates.username, chooseGroup);
                        bot.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Ви успішно вибрали групу: {chooseGroup}\nЯкщо ви обрали не правильно або хочете змінити групу натисніть кнопку \"змінити групу\"");
                        break;
                    case "getDbList":
                        {
                            bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ви натиснули Кнопку 1");
                            break;
                        }
                    case "test2":
                        {
                            bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Ви натиснули Кнопку 2");
                            break;
                        }
                    // case "testcreateuser":
                    //     {
                    //         UserInterface.TestCreateUser(_botUpdates.username, "KSM");
                    //         break;
                    //     }
                    case "download":
                        {
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
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Start listening for @{bot.GetMeAsync().Result.Username}");

            var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions { AllowedUpdates = { }, };
            bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cts.Token);
            //ApplicationContext.DbConnect();
            Console.ReadLine();
        }
    }
}