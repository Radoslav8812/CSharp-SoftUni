using System;
using System;
using System.ComponentModel.DataAnnotations;
using FootballBetting.Data.Common;
using System.Data.Common;

namespace FootballBetting.Data.Models
{
	public class Country
	{
		public Country()
		{
            Towns = new HashSet<Town>();
		}

        [Key]
        public int CountryId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.CountryNameMaxLength)]
        public string Name { get; set; }


        public virtual ICollection<Town> Towns { get; set; }

    }
}

