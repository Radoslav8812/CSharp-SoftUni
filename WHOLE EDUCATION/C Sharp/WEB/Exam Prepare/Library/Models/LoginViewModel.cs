using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
	public class LoginViewModel
	{
		[Required]
		[StringLength(20, MinimumLength = 5)]
		public string UserName { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 5)]
		[DataType(DataType.Password)]
		public string Password { get; set; }

	}
}

