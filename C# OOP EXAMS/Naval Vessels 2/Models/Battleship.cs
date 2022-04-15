using System;
using NavalVessels.Models.Contracts;

namespace NavalVessels.Models
{
	public class Battleship : Vessel , IBattleship
	{
        private bool sonarMode;

        public Battleship(string name, double mainWeaponCaliber, double speed) : base(name, mainWeaponCaliber, speed, 300)
        {
            SonarMode = false;
        }

        public bool SonarMode
        {
            get
            {
                return sonarMode;
            }
            private set
            {
                sonarMode = value;
            }
        }

        public override void RepairVessel()
        {
            if (this.ArmorThickness < 300)
            {
                this.ArmorThickness = 300;
            }
        }

        public void ToggleSonarMode()
        {
            if (SonarMode == true)
            {
                MainWeaponCaliber += 40;
                Speed -= 5;
                SonarMode = false;
            }
            else if (SonarMode == false)
            {
                MainWeaponCaliber -= 40;
                Speed += 5;
                SonarMode = true;
            }
        }

        public override string ToString()
        {
            return base.ToString() + Environment.NewLine +
                $" *Sonar mode: { (SonarMode ? "ON" : "OFF")}";
        }
    }
}

