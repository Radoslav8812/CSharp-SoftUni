using System;
using System.ComponentModel.DataAnnotations;
using FootballBetting.Data.Common;
using System.Data.Common;


namespace FootballBetting.Data.Models
{
	public class Position
	{
		public Position()
		{
            Players = new HashSet<Player>();
		}

        [Key]
        public int PositionId{ get; set; }

        [Required]
        [MaxLength(GlobalConstants.PositionNameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}

