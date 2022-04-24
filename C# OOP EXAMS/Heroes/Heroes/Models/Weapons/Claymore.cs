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
            if (Durability - 1 >= 0)
            {
                Durability -= 1;
                return 20;

            }
            else
            {
                Durability = 0;
                return 0;
            }
        }
    }
}

