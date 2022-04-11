using System;
using Easter.Models.Eggs.Contracts;

namespace Easter.Models.Eggs
{
	public class Egg : IEgg
	{
        private string name;
        private int energyRequired;

        public Egg(string name, int energyRequired)
		{
            Name = name;
            EnergyRequired = energyRequired;
		}

        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Egg name cannot be null or empty.");
                }
                name = value;
            }
        }

        public int EnergyRequired
        {
            get
            {
                return energyRequired;
            }
            private set
            {
                if (value < 0)
                {
                    value = 0;
                }
                energyRequired = value;
            }
        }

        public void GetColored()
        {
            EnergyRequired -= 10;
        }

        public bool IsDone()
        {
            if (EnergyRequired == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

