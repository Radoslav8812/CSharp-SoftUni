using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Footballers.Data.Models
{
	public class Team
	{
		public Team()
		{
			TeamsFootballers = new HashSet<TeamFootballer>();
		}

		public int Id { get; set; }

		[Required]
		public string Name { get; set; } // regex

		[Required]
		public string Nationality { get; set; } //

		[Required]
		public int Trophies { get; set; }

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; }
    }
}

