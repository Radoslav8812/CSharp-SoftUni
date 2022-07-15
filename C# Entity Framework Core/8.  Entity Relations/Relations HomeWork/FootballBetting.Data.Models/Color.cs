using System;
using System.ComponentModel.DataAnnotations;
using FootballBetting.Data.Common;
using System.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballBetting.Data.Models
{
	public class Color
	{
		public Color()
		{
            this.PrimaryKitTeams = new HashSet<Team>();
            this.SecondaryKitTeams = new HashSet<Team>();
		}

        [Key]
        public int ColorId { get; set; }

		[Required]
        [MaxLength(GlobalConstants.ColorNameMaxLength)]
        public string Name { get; set; }

        [InverseProperty("PrimaryKitColor")]
        public virtual ICollection<Team> PrimaryKitTeams { get; set; }

        [InverseProperty("SecondaryKitColor")]
        public virtual ICollection<Team> SecondaryKitTeams { get; set; }
    }
}

