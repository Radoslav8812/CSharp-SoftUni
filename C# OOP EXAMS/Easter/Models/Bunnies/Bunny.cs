using System;
using System.Collections.Generic;
using System.Linq;
using Easter.Models.Bunnies.Contracts;
using Easter.Models.Dyes.Contracts;

namespace Easter.Models.Bunnies
{
	public abstract class Bunny : IBunny
	{
        private string name;
        private int energy;
        private List<IDye> dyes;

		public Bunny(string name, int energy)
		{
            Name = name;
            Energy = energy;

            dyes = new List<IDye>();
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
                    throw new ArgumentException("Bunny name cannot be null or empty.");
                }
                name = value;
            }
        }

        public int Energy
        {
            get
            {
                return energy;
            }
            protected set
            {
                if (value < 0)
                {
                    value = 0;
                }
                energy = value;
            }
        }

        public ICollection<IDye> Dyes => dyes;

        public void AddDye(IDye dye)
        {
            dyes.Add(dye);
        }

        public virtual void Work()
        {
            Energy -= 10;
        }

        public override string ToString()
        {
            return $"Name: {Name}" + Environment.NewLine +
            $"Energy: {Energy}" + Environment.NewLine +
            $"Dyes: {dyes.Count(x => x.Power > 0)} not finished";

        }
    }
}

