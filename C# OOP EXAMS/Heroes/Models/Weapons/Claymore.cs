using System;
namespace Heroes.Models.Weapons
{
    public class Claymore : Weapon
    {
        private const int CLAYMORE_DMG = 20;

        public Claymore(string name, int durability) : base(name, durability)
        {
            
        }

        public override int DoDamage()
        {
            Durability -= 1;

            if (Durability == 0)
            {
                Durability = 0;
            }

            return CLAYMORE_DMG;
        }
    }
}

