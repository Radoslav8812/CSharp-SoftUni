using System;
using System.ComponentModel.DataAnnotations;

namespace Contacts.Data.Models
{
	public class Contact
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string LastName { get; set; }

		[Required]
		[StringLength(60, MinimumLength = 10)]
		public string Email { get; set; }

		[Required]
		[StringLength(13, MinimumLength = 10)]
		public string PhoneNumber { get; set; }

		public string? Address { get; set; }

		[Required]
		public string Website { get; set; }

		public List<ApplicationUserContact> ApplicationUsersContacts = new List<ApplicationUserContact>();

    }
}

