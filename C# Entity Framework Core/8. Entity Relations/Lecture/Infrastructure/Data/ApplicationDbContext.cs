using System;
using Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=PetDB;Trusted_Connection=True;Integrated Security=false;User Id=sa;Password=Docker@123");
            }
        }

        public DbSet<Person> People { get; set; }

        public DbSet<Dog> Dogs { get; set; }

    }
}

