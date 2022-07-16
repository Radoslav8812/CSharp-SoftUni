using System;
using System;
using System.ComponentModel.DataAnnotations;
using FootballBetting.Data.Common;
using System.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballBetting.Data.Models
{
	public class Town
	{
		public Town()
		{
            Teams = new HashSet<Team>();
		}

        [Key]
        public int TownId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.TownNameMaxLength)]
        public string Name { get; set; }

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }


        public ICollection<Team> Teams { get; set; }
    }
}

