using System;
using Microsoft.AspNetCore.Identity;

namespace Contacts.Data.Models
{
	public class ApplicationUser : IdentityUser
	{
        public List<ApplicationUserContact> ApplicationUsersContacts { get; set; } = new List<ApplicationUserContact>();
    }
}

