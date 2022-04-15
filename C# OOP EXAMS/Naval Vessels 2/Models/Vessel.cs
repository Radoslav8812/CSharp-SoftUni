using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NavalVessels.Models.Contracts;

namespace NavalVessels.Models
{
	public abstract class Vessel : IVessel
	{
        private string name;
        private double mainWeaponCaliber;
        private double speed;
        private double armorThickness;
        ICaptain captain;
        private List<string> targets;

		protected Vessel(string name, double mainWeaponCaliber, double speed, double armorThickness)
		{
            Name = name;
            MainWeaponCaliber = mainWeaponCaliber;
            Speed = speed;
            ArmorThickness = armorThickness;

            targets = new List<string>();
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
                    throw new ArgumentNullException("Vessel name cannot be null or empty.");
                }
                name = value;
            }
        }

        public ICaptain Captain
        {
            get
            {
                return captain;
            }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException("Captain cannot be null.");
                }
                captain = value;
            }
        }
        public double ArmorThickness
        {
            get
            {
                return armorThickness;
            }
            set
            {
                armorThickness = value;
            }
        }

        public double MainWeaponCaliber
        {
            get
            {
                return mainWeaponCaliber;
            }
            set
            {
                mainWeaponCaliber = value;
            }
        }

        public double Speed
        {
            get
            {
                return speed;
            }
            protected set
            {
                speed = value;
            }
        }

        public ICollection<string> Targets
        {
            get
            {
                return targets;
            }
        }

        public void Attack(IVessel target)
        {
            if (target == null)
            {
                throw new NullReferenceException("Target cannot be null.");
            }

            target.ArmorThickness -= MainWeaponCaliber;
            targets.Add(target.Name);
        }

        public abstract void RepairVessel();


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"- {Name}");
            sb.AppendLine($" *Type: {GetType().Name}");
            sb.AppendLine($" *Armor thickness: {ArmorThickness}");
            sb.AppendLine($" *Main weapon caliber: {MainWeaponCaliber}");
            sb.AppendLine($" *Speed: {Speed} knots");
            sb.AppendLine($" *Targets: {(targets.Any() ? string.Join(", ", targets) : "None")}");

            return sb.ToString().TrimEnd();
        }
    }
}

