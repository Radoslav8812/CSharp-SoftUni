using System;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
	public class SongPerformer
	{
		public SongPerformer()
		{
		}

        
        public int SongId { get; set; }

        [Required]
        public Song Song { get; set; }

        public int PerformerId { get; set; }
        public Performer Performer { get; set; }


    }
}

