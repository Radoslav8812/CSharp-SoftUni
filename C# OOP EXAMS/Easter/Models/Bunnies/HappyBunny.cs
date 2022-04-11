using System;
using Easter.Models.Bunnies.Contracts;

namespace Easter.Models.Bunnies
{
    public class HappyBunny : Bunny, IBunny
    {
        public HappyBunny(string name) : base(name, 100)
        {
        }

        public override void Work()
        {
            base.Work();
        }
    }
}

