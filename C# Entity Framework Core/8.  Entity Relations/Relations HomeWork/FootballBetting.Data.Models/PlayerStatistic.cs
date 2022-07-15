using System;
using System.ComponentModel.DataAnnotations;
using FootballBetting.Data.Common;
using System.Data.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballBetting.Data.Models
{
	// Mapping Class !
	public class PlayerStatistic
	{
		public PlayerStatistic()
		{
		}

        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }
        public virtual Game Game { get; set; }

        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        public byte ScoredGoals { get; set; }

        public byte Assists { get; set; }

        public byte MinutesPlayed { get; set; }

    }
}

