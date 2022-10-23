using System.Globalization;
using schedule_bot;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
namespace shedule_bot.backend
{
    public class UserInterface
    {
        static ITelegramBotClient bot = new TelegramBotClient(ToolChain.GetItemFromDotEnv("TOKEN"));
        static CancellationTokenSource cts = new CancellationTokenSource();
        static Update? update;
        static long chatId = update.Message.Chat.Id;

        public static string InitialChooseUserGroup()
        {
            ToolChain.GroupInline(bot, chatId, cts.Token);
            string? chooseGroup;
            switch (update?.Message?.Text?.ToLower())
            {
                case "1KCM-A":
                    chooseGroup = "1KCM-A";
                    return chooseGroup;

                case "1KCM-B":
                    chooseGroup = "1KCM-B";
                    return chooseGroup;

                case "2KCM-A":
                    chooseGroup = "2KCM-A";
                    return chooseGroup;

                case "2KCM-B":
                    chooseGroup = "2KCM-B";
                    return chooseGroup;

                case "3KCM-A":
                    chooseGroup = "3KCM-A";
                    return chooseGroup;

                case "3KCM-B":
                    chooseGroup = "3KCM-B";
                    return chooseGroup;

                case "3KCM-11":
                    chooseGroup = "3KCM-11";
                    return chooseGroup;
            }
            return "";
        }

        public static void InitialCreateUser(string username, string group)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User newUser = new User { username = username, usergroup = group };

                db.Users.Add(newUser);
                db.SaveChanges();
            }
        }
        public static void TestConnection()
        {
            using (ApplicationContext db = new ApplicationContext())//var conn = new NpgsqlConnection(@$"Server=localhost;Database=kemkdb;User ID=postgres;Password=13378;")
            {
                Console.WriteLine("Opening connection");
            }
        }
        public static void TestCreateUser(string username, string group)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User newUser = new User { username = username, usergroup = group };

                db.Users.Add(newUser);
                db.SaveChanges();
            }
        }


        public static void GetUserData()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // получаем объекты из бд и выводим на консоль
                var users = db.Users.ToList();
                Console.WriteLine("Users list: ");
                foreach (User u in users)
                {
                    Console.WriteLine($"{u.username} - {u.usergroup}");
                }
            }
        }

        public static void ChangeUserGroup()
        {

        }
    }
}