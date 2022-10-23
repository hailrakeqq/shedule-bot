//using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace shedule_bot.backend
{
    [Table("users")]
    public class User
    {
        [Key]
        //[Column(Order = 1)]
        public int id { get; set; }
        public string? username { get; set; }
        public string? usergroup { get; set; }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=kemkdb;Username=postgres;Password=13378;");

    }
}
