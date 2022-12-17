using System;
using System.ComponentModel.DataAnnotations;

namespace Watchlist.Models
{
	public class RegisterViewModel
	{
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(60, MinimumLength = 10)]
        public string Email { get; set; }
    }
}

