using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EFC_Code_First.Models
{
	[Index(nameof(QuestionId), Name = "IX_QuestionId123")]
	public class Comment
	{
		
		public int Id { get; set; }

		[Required]
		[MaxLength(200)]
		public string Content { get; set; }


		public int QuestionId { get; set; }
		public Question Question { get; set; }

	}
}

