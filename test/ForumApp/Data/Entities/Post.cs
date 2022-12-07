using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static ForumApp.Data.DataConstants.Posts;

namespace ForumApp.Data.Entities
{
	public class Post
	{
		public Post()
		{
		}

        [Key]
        public int Id { get; init; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;

        [Comment("Marks recors as deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}

