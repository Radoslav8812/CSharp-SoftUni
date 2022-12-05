using System;
using ForumApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Data
{
	public class ForumAppDbContext : DbContext
	{
		public ForumAppDbContext(DbContextOptions<ForumAppDbContext> options) : base(options)
		{
		}

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.ApplyConfiguration<Post>(new PostConfiguration());

            builder.Entity<Post>()
                .Property(p => p.IsDeleted)
                .HasDefaultValue(false);
            //SeedPosts();
            //builder.Entity<Post>()
            //    .HasData(FirstPost, SecondPost, ThirdPost);

            base.OnModelCreating(builder);
        }
    }
}

