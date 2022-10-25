using Telegram.Bot;
using Telegram.Bot.Types;
using Npgsql;

namespace shedule_bot.backend
{
    public class UserInterface
    {
        static ITelegramBotClient bot = new TelegramBotClient(ToolChain.GetItemFromDotEnv("TOKEN"));
        static CancellationTokenSource cts = new CancellationTokenSource();
        static Update? update;

        // public static string InitialChooseUserGroup(long chatId)
        // {
        //     string chooseGroup = "";
        //     switch (update?.Message?.Text?.ToLower())
        //     {
        //         case "1KCM-A":
        //             chooseGroup = "1KCM-A";
        //             break;
        //         case "1KCM-B":
        //             chooseGroup = "1KCM-B";
        //             break;
        //         case "2KCM-A":
        //             chooseGroup = "2KCM-A";
        //             break;
        //         case "2KCM-B":
        //             chooseGroup = "2KCM-B";
        //             break;
        //         case "3KCM-A":
        //             chooseGroup = "3KCM-A";
        //             break;
        //         case "3KCM-B":
        //             chooseGroup = "3KCM-B";
        //             break;
        //         case "3KCM-11":
        //             chooseGroup = "3KCM-11";
        //             break;
        //     }
        //     return chooseGroup;
        // }

        public static void InitialCreateUser(string username, string group)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User newUser = new User { username = username, usergroup = group };

                db.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        //check if user exist in db; if exist => return true ; else => return false
        public static bool CheckIfUserExisting(string username)
        {
            using var connectionToDb = new NpgsqlConnection(ApplicationContext.GetConnectionString());
            connectionToDb.Open();
            var cmd = new NpgsqlCommand($"SELECT COUNT(users) FROM users WHERE username = '${username}';", connectionToDb);
            var commandResult = cmd.ExecuteNonQuery();

            connectionToDb.Close();
            if (Convert.ToSByte(commandResult) > 0)
                return true;
            return false;
        }

        public static void TestConnection()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine("Opening connection");
            }
        }

        public static void GetUserData()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                //get object from db and console print
                var users = db.Users.ToList();
                Console.WriteLine("Users list: ");
                foreach (User u in users)
                    Console.WriteLine($"{u.username} - {u.usergroup}");
            }
        }

        //function for change Usergroup info for existing user
        public static void ChangeUserGroup()
        {

        }
    }
}