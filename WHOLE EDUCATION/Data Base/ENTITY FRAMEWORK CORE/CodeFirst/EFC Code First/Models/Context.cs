using System;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;

namespace EFC_Code_First.Models
{
	public class Context : DbContext
	{

        public Context()
        {

        }

        public Context(DbContextOptions dbContextOptions)  : base(dbContextOptions)
        {

        }

		public DbSet<Comment> Comments { get; set; }

		public DbSet<Question> Questions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost; Database=Slido; Trusted_Connection=True; Integrated Security=false;UserId=sa; Password=Docker@123");

            }
        }
    }
}

