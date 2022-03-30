using System;
using Gym.Models.Athletes.Contracts;

namespace Gym.Models.Athletes
{
	public class Boxer : Athlete, IAthlete
	{
        public Boxer(string fullName, string motivation, int numberOfMedals) : base(fullName, motivation, numberOfMedals, 60)
        {
        }

        public override void Exercise()
        {
            Stamina += 15;

            if (Stamina > 100)
            {
                Stamina = 100;
                throw new ArgumentException("Stamina cannot exceed 100 points.");
            }
        }
    }
}

