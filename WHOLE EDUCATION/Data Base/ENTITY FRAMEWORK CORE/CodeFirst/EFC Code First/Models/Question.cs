using System;
using System.ComponentModel.DataAnnotations;

namespace EFC_Code_First.Models
{
	public class Question
	{

		
		public int Id { get; set; }

		[Required]
		[MaxLength(200)]
		public string Content { get; set; }

		public DateTime CreatedOn { get; set; }


	}
}

