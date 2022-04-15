using System;
using NavalVessels.Models.Contracts;

namespace NavalVessels.Models
{
	public class Submarine : Vessel, ISubmarine
	{
        private bool submergeMode;

        public Submarine(string name, double mainWeaponCaliber, double speed) : base(name, mainWeaponCaliber, speed, 200)
        {
            SubmergeMode = false; 
        }

        public bool SubmergeMode
        {
            get
            {
                return submergeMode;
            }
            private set
            {
                submergeMode = value;
            }
        }

        public override void RepairVessel()
        {
            if (ArmorThickness < 200)
            {
                ArmorThickness = 200;
            }
        }

        public void ToggleSubmergeMode()
        {
            if (SubmergeMode == true)
            {
                MainWeaponCaliber += 40;
                Speed -= 4;
                SubmergeMode = false;
            }
            else if (SubmergeMode == false)
            {
                MainWeaponCaliber -= 40;
                Speed += 4;
                SubmergeMode = true;
            }
        }

        public override string ToString()
        {
            return base.ToString() + Environment.NewLine +
                $" *Submerge mode: { (SubmergeMode ? "ON" : "OFF")}";
        }
    }
}

