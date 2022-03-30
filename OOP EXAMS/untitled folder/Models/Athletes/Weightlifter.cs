using System;
using Gym.Models.Athletes.Contracts;

namespace Gym.Models.Athletes
{
    public class Weightlifter : Athlete, IAthlete
    {
        public Weightlifter(string fullName, string motivation, int numberOfMedals) : base(fullName, motivation, numberOfMedals, 50)
        {
        }

        public override void Exercise()
        {
            Stamina += 10;

            if (Stamina > 100)
            {
                Stamina = 100;
                throw new ArgumentException("Stamina cannot exceed 100 points.");
            }
        }
    }
}

