using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Watchlist.Data.Models
{
	public class Movie
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string Director { get; set; }

		[Required]
		public string ImageUrl { get; set; }

		[Required]
		[Range(typeof(decimal), "0.00", "10.00")]
		public decimal Rating { get; set; }

		public int GenreId { get; set; }

		[ForeignKey(nameof(GenreId))]
		public Genre Genre { get; set; }

		public List<UserMovie> UsersMovies { get; set; } = new List<UserMovie>();

	}
}

