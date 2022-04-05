using System;
using AquaShop.Models.Fish.Contracts;

namespace AquaShop.Models.Fish
{
	public class FreshwaterFish : Fish, IFish
	{
        public FreshwaterFish(string name, string species, decimal price) : base(name, species, price)
        {
            this.Size = 3;
        }

        public override void Eat()
        {
            this.Size += 3;
        }
    }
}

