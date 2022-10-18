using Microsoft.EntityFrameworkCore;

namespace schedule_bot
{
    public class User
    {
        public int id;
        public string? username;
        public string? group;
    }

    public class ApplicationContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPassword = ToolChain.GetItemFromDotEnv("DbPassword");
            optionsBuilder.UseNpgsql($"Host=localhost;Port=5433;Database=kemkdb;Username=postgres;Password={dbPassword}");
        }
    }
}