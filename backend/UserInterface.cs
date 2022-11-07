using Telegram.Bot;
using Telegram.Bot.Types;
using Npgsql;

namespace shedule_bot.backend
{
    public class UserInterface
    {
        static ITelegramBotClient bot = new TelegramBotClient(ToolChain.GetItemFromDotEnv("TOKEN"));
        static CancellationTokenSource cts = new CancellationTokenSource();

        public async static void InitialCreateUser(string username, string group, long chatId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User newUser = new User { username = username, usergroup = group, timestamp = "16:00" }; //16:00 default timestamp for all userss 

                db.Users.Add(newUser);
                db.SaveChanges();
            }
            await bot.SendTextMessageAsync(chatId: chatId, text: $"Ви успішно вибрали групу: {group}\nЯкщо ви обрали не правильно або хочете змінити групу натисніть кнопку \"змінити групу\"\nЧас нагадування стоїть 16:00 стандартно для всіх користувачів , ви можете змінити час за допомогою команди \"Змінити час нагадування <час нагадування>\"");
            ToolChain.MainInline(bot, chatId, cts.Token); //send main inline keyboard after succesfull user create 
        }

        //function for change Usergroup info for existing user
        public async static void ChangeUserGroup(string username, string group, long chatId)
        {
            using var connectionToDb = new NpgsqlConnection(ApplicationContext.GetConnectionString());
            connectionToDb.Open();
            var cmd = new NpgsqlCommand($"UPDATE users SET usergroup = '{group}' WHERE username = '{username}';", connectionToDb);
            object commandResult = cmd.ExecuteScalar();
            connectionToDb.Close();

            await bot.SendTextMessageAsync(chatId: chatId, text: $"Ви успішно змінили групу!!!\nТепер ваша група {group}");
            ToolChain.MainInline(bot, chatId, cts.Token); //send main inline keyboard after succesfull user create 
        }

        public async static void ChangeUserTimestamp(string username, string timestamp, long chatId)
        {
            using var connectionToDb = new NpgsqlConnection(ApplicationContext.GetConnectionString());
            connectionToDb.Open();
            var cmd = new NpgsqlCommand($"UPDATE users SET timestamp = '{timestamp}' WHERE username = '{username}';", connectionToDb);
            object commandResult = cmd.ExecuteScalar();
            connectionToDb.Close();

            await bot.SendTextMessageAsync(chatId: chatId, text: $"Ви успішно змінили час відправлення нагадування!!!\nТепер нагадування будуть приходити в *{timestamp}*!");
            ToolChain.MainInline(bot, chatId, cts.Token); //send main inline keyboard after succesfull user create 
        }

        //check if user exist in db; if exist => return true ; else => return false
        public static bool CheckIfUserExisting(string username)
        {
            using var connectionToDb = new NpgsqlConnection(ApplicationContext.GetConnectionString());
            connectionToDb.Open();
            var cmd = new NpgsqlCommand($"SELECT COUNT(users) FROM users WHERE username = '{username}';", connectionToDb);
            object commandResult = cmd.ExecuteScalar();

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

    }
}