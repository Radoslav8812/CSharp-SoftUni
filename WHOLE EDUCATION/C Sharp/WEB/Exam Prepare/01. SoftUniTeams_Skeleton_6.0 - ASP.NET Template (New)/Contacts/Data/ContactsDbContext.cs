using Contacts.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Contacts.Data
{
    public class ContactsDbContext : IdentityDbContext<ApplicationUser>
    {
        public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
        {
            
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ApplicationUserContact> ApplicationUserContacts{ get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserContact>()
                .HasKey(x => new { x.ContactId, x.ApplicationUserId });

            builder.Entity<ApplicationUser>()
                .Property("UserName")
                .HasMaxLength(20);

            builder.Entity<ApplicationUser>()
               .Property("Email")
               .HasMaxLength(60);

            builder
                .Entity<Contact>()
                .HasData(new Contact()
                {
                    Id = 1,
                    FirstName = "Bruce",
                    LastName = "Wayne",
                    PhoneNumber = "+359881223344",
                    Address = "Gotham City",
                    Email = "imbatman@batman.com",
                    Website = "www.batman.com"
                });
            
        }
    }
}