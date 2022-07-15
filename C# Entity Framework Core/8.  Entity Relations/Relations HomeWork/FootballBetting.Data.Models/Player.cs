using System;
using System.ComponentModel.DataAnnotations;
using FootballBetting.Data.Common;
using System.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballBetting.Data.Models
{
	public class Player
	{
		public Player()
		{
            PlayerStatistics = new HashSet<PlayerStatistic>();
		}

        [Key]
        public int PlayerId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PlayerNameMaxLength)]
        public string Name { get; set; }

        public byte SquadNumber { get; set; }

        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        [ForeignKey(nameof(Position))]
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }

        public bool IsInjured { get; set; }

        public object MyProperty { get; set; }

        public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; }
    }
}
