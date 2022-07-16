using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
	public class Performer
	{
		public Performer()
		{
            PerformerSongs = new HashSet<SongPerformer>();
		}

        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public String FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        public int Age { get; set; }

        [Required]
        public decimal NetWorth { get; set; }

        public ICollection<SongPerformer> PerformerSongs { get; set; }

    }
}

