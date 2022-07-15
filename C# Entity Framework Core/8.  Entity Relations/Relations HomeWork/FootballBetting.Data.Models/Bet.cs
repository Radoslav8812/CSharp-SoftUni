using System;
using System.ComponentModel.DataAnnotations;
using FootballBetting.Data.Common;
using System.Data.Common;
using FootballBetting.Data.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballBetting.Data.Models
{
	public class Bet
	{
		public Bet()
		{
            
		}

        [Key]
        public int BetId { get; set; }

        public decimal Amount { get; set; }

        [Required]
        public Prediction Prediction { get; set; }

        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}

