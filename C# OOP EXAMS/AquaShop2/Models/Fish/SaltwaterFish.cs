using System;
using AquaShop.Models.Fish.Contracts;

namespace AquaShop.Models.Fish
{
	public class SaltwaterFish : Fish, IFish
	{
        public SaltwaterFish(string name, string species, decimal price) : base(name, species, price)
        {
            this.Size = 5;
        }

        public override void Eat()
        {
            this.Size += 2;
        }
    }
}

