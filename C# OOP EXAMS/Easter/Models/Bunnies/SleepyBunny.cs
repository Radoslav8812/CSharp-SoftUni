using System;
using Easter.Models.Bunnies.Contracts;

namespace Easter.Models.Bunnies
{
	public class SleepyBunny : Bunny, IBunny
	{
        public SleepyBunny(string name) : base(name, 50)
        {
        }

        public override void Work()
        {
            base.Work();
            Energy -= 5;
        }
    }
}

