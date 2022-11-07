using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
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

                //test timestamp change
                Regex trueDataChangeCommandPattern = new Regex(@"Змінити час нагадування \b\w{1,2}:\w{1,2}\b");
                Regex trueDataPattern = new Regex(@"\b\w{1,2}:\w{1,2}\b");

                MatchCollection checkDataExist = trueDataPattern.Matches(messageText);
                MatchCollection checkDataChangeCommandPatternExist = trueDataChangeCommandPattern.Matches(messageText);
                if (message.Text == "Змінити час нагадування" || checkDataChangeCommandPatternExist.Count > 0)
                {
                    string[] currentCommand = message.Text.Split(" ");
                    try
                    {
                        if (checkDataExist.Count == 0)
                            await bot.SendTextMessageAsync(chatId, text: "Введіть команду для зміни часу нагадування:\nЗмінити час нагадування <час (17:00)>");
                        else
                            UserInterface.ChangeUserTimestamp(_botUpdates.username, currentCommand[3], chatId);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await bot.SendTextMessageAsync(chatId, text: "Введіть команду для зміни часу нагадування:\nЗмінити час нагадування <час (17:00)>");
                    }
                }

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

                    //test section
                    case "create-test":
                        try
                        {
                            XlsTest.CreateFile();
                        }
                        catch (System.Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case "test-view":
                        try
                        {
                            await bot.SendTextMessageAsync(chatId: chatId, text: XlsTest.ViewDataFromCell());
                        }
                        catch (System.Exception ex) { Console.WriteLine(ex.Message); }
                        break;

                    case "change-test":
                        try
                        {
                            XlsTest.ChangeCellData();
                        }
                        catch (System.Exception ex) { Console.WriteLine(ex.Message); }
                        break;
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