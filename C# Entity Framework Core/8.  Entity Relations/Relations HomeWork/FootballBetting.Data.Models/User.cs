using System;
using System.ComponentModel.DataAnnotations;
using FootballBetting.Data.Common;
using System.Data.Common;
using FootballBetting.Data.Models.Enums;

namespace FootballBetting.Data.Models
{
	public class User
	{
		public User()
		{
            Bets = new HashSet<Bet>();
		}

        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.UserUserNameMaxLength)]
		public string UserName { get; set; }

        [Required]
        [MaxLength(GlobalConstants.UserPasswordMaxLength)]
		public string Password { get; set; }

        [Required]
        [MaxLength(GlobalConstants.UserEmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(GlobalConstants.UserNameMaxLength)]
        public string Name { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}

