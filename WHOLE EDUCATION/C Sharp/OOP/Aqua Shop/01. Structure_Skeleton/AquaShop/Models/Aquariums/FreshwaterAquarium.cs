using System;
using AquaShop.Models.Aquariums.Contracts;

namespace AquaShop.Models.Aquariums
{
	public class FreshwaterAquarium : Aquarium
	{
		
        public FreshwaterAquarium(string name) : base(name, 50)
        {
        }
    }
}

