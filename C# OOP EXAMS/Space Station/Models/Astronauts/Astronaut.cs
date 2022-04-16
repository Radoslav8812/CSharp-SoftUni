using System;
using SpaceStation.Models.Astronauts.Contracts;
using SpaceStation.Models.Bags;
using SpaceStation.Models.Bags.Contracts;
using SpaceStation.Utilities.Messages;

namespace SpaceStation.Models.Astronauts
{
	public abstract class Astronaut : IAstronaut
	{
        private string name;
        private double oxygen;

		protected Astronaut(string name, double oxygen)
		{
            Name = name;
            Oxygen = oxygen;
            Bag = new Backpack();
		}

        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(Name), ExceptionMessages.InvalidAstronautName);
                }
                name = value;
            }
        }

        public double Oxygen
        {
            get
            {
                return oxygen;
            }
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ExceptionMessages.InvalidOxygen);
                }
                oxygen = value;
            }
        }

        public bool CanBreath => Oxygen > 0;

        public IBag Bag { get; }

        public virtual void Breath()
        {
            if (Oxygen - 10 > 0)
            {
                Oxygen -= 10;
            }
            else
            {
                Oxygen = 0;
            }
        }
    }
}

