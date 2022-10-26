using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace shedule_bot.backend
{
    [Table("users")]
    public class User
    {
        [Key]
        public int id { get; set; }
        public string? username { get; set; }
        public string? usergroup { get; set; }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        private static string connectionString = $"Host=localhost;Database=kemkdb;Username=postgres;Password={ToolChain.GetItemFromDotEnv("DbPassword")};";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(connectionString);

        public static string GetConnectionString() => connectionString;

    }
    // public class DataBaseConnect
    // {
    //     private static string connectionString = $"Host=localhost;Username=postgres;Password=${ToolChain.GetItemFromDotEnv("DbPassword")};Database=kemkdb";

    //     public async static void NpgsqlConnect() // Npgsql.NpgsqlConnection
    //     {
    //         await using var conn = new NpgsqlConnection(connectionString);
    //         await conn.OpenAsync();
    //     }
    // }
}
