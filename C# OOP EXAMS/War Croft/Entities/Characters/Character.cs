using System;

using WarCroft.Constants;
using WarCroft.Entities.Inventory;
using WarCroft.Entities.Items;

namespace WarCroft.Entities.Characters.Contracts
{
    public abstract class Character
    {
		private string name;
		private double baseHealth;
		private double health;
        private double baseArmor;
        private double armor;
        private double abilityPoints;
        private IBag bag;

		protected Character(string name, double health, double armor, double abilityPoints, Bag bag)
        {
            Name = name;
            BaseHealth = baseHealth;
            Health = health;
            BaseArmor = armor;
            Armor = BaseArmor;
            AbilityPoints = abilityPoints;
            Bag = bag;
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
					throw new ArgumentException("Name cannot be null or whitespace!");
                }
				name = value;
            }
        }

		public double BaseHealth
        {
            get
            {
				return baseHealth;
            }
			private set
            {
				baseHealth = value;
            }
        }

		public double Health
        {
            get
            {
                return health;
            }
            set
            {
                if (value > BaseHealth)
                {
                    health = 0;
                }
                else if (value < 0)
                {
                    health = 0;
                }
                else
                {
                    health = value;
                }
            }
        }

        private double BaseArmor
        {
            get
            {
                return baseArmor;
            }
            set
            {
                baseArmor = value;
            }
        }

        public double Armor
        {
            get
            {
                return armor;
            }
            private set
            {
                if (value < 0)
                {
                    armor = 0;
                }
                else
                {
                    armor = value;
                }
            }
        }

        public double AbilityPoints
        {
            get
            {
                return abilityPoints;
            }
            private set
            {
                abilityPoints = value;
            }
        }

        public IBag Bag
        {
            get
            {
                return bag;
            }
            private set
            {
                bag = value;
            }
        }

		public bool IsAlive { get; set; } = true;

        public void TakeDamage(double hitPoints)
        {
            EnsureAlive();
            double remainingPoints = Armor < hitPoints ? hitPoints - Armor : 0;

            if (remainingPoints > 0)
            {
                Health -= remainingPoints;

                if (Health == 0)
                {
                    IsAlive = false;
                }
            }
        }

        public void UseItem(Item item)
        {
            EnsureAlive();

            item.AffectCharacter(this);
        }

        protected void EnsureAlive()
		{
			if (!this.IsAlive)
			{
				throw new InvalidOperationException(ExceptionMessages.AffectedCharacterDead);
			}
		}
	}
}