using System;
using Heroes.Models.Contracts;
using Heroes.Models.Weapons;

namespace Heroes.Models.Heroes
{
	public abstract class Hero : IHero
	{
        private string name;
        private int health;
        private int armour;
        IWeapon weapon;
        private bool isALive;

		public Hero(string name, int health, int armour)
		{
            Name = name;
            Health = health;
            Armour = armour;
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
                    throw new ArgumentException("Hero name cannot be null or empty.");
                }
                name = value;
            }
        }

        public int Health
        {
            get
            {
                return health;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Hero health cannot be below 0.");
                }
                health = value;
            }
        }

        public int Armour
        {
            get
            {
                return armour;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Hero armour cannot be below 0.");
                }
                armour = value;
            }
        }

        public IWeapon Weapon
        {
            get
            {
                return weapon;
            }
            set
            {
                if (weapon == null)
                {
                    throw new ArgumentException("Weapon cannot be null.");
                }
                weapon = value;
            }
        }

        public bool IsAlive
        {
            get
            {
                return isALive;
            }
            private set
            {
                if (Health > 0)
                {
                    isALive = true;
                }
                else
                {
                    isALive = false;
                }
                isALive = value;
            }
        }

        public void AddWeapon(IWeapon weapon)
        {
            if (weapon.GetType().Name == "Claymore")
            {
                this.weapon = weapon;
            }
            else
            {
                if (weapon.GetType().Name == "Mace")
                {
                    this.weapon = new Mace(weapon.Name, weapon.Durability);
                }
            }
        }

        public void TakeDamage(int points)
        {
            if (Armour - points > 0)
            {
                Armour -= points;
            }
            else if (Armour - points < 0)
            {
                var remainigPoints = points - Armour;
                Health -= remainigPoints;
            }
        }
    }
}

