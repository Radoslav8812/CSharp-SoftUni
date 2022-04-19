using System;
namespace Heroes.Models.Weapons
{
	public class Mace : Weapon
	{
        private const int MACE_DMG = 25;
        
        public Mace(string name, int durability) : base(name, durability)
        {
            
        }

        public override int DoDamage()
        {
            Durability -= 1;
            
            if (Durability == 0)
            {
                Durability = 0;
            }

            return MACE_DMG;
        }
    }
}

