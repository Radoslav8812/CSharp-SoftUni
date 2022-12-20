using System;
using Library.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
	public class AddBookViewModel
	{
        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Author { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [Range(typeof(decimal), "0,0", "10,0")]
        public decimal Rating { get; set; }

        [Required]
        [StringLength(5000, MinimumLength = 5)]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        public List<Category> Categories { get; set; } = new List<Category>();
    }
}

