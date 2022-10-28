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
                        try
                        {
                            if (UserInterface.CheckIfUserExisting(_botUpdates.username))
                                ToolChain.MainInline(bot, chatId, cts.Token);
                            else
                                ToolChain.GroupInline(bot, chatId, cts.Token);
                        }
                        catch (System.Exception ex) { Console.WriteLine(ex); }
                        break;

                    case "Змінити групу":
                        ToolChain.GroupInline(bot, chatId, cts.Token);
                        break;

                    case "1KCM-A":
                        ChooseGroupOrCreateUser(chatId, _botUpdates, "1KCM-A");
                        break;
                    case "1KCM-B":
                        ChooseGroupOrCreateUser(chatId, _botUpdates, "1KCM-B");
                        break;
                    case "2KCM-A":
                        ChooseGroupOrCreateUser(chatId, _botUpdates, "2KCM-A");
                        break;
                    case "2KCM-B":
                        ChooseGroupOrCreateUser(chatId, _botUpdates, "2KCM-B");
                        break;
                    case "3KCM-A":
                        ChooseGroupOrCreateUser(chatId, _botUpdates, "3KCM-A");
                        break;
                    case "3KCM-B":
                        ChooseGroupOrCreateUser(chatId, _botUpdates, "3KCM-B");
                        break;
                    case "3KCM-11":
                        ChooseGroupOrCreateUser(chatId, _botUpdates, "3KCM-11");
                        break;

                    case "get shedule":
                        try
                        {
                            Xls.Print();
                        }
                        catch (System.Exception ex) { Console.WriteLine(ex); }
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

        private static void ChooseGroupOrCreateUser(long chatId, BotUpdates _botUpdates, string group)
        {
            if (UserInterface.CheckIfUserExisting(_botUpdates.username))
                UserInterface.ChangeUserGroup(_botUpdates.username, group, chatId);
            else
                UserInterface.InitialCreateUser(_botUpdates.username, group, chatId);
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