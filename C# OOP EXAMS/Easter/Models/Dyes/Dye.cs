using System;
using Easter.Models.Dyes.Contracts;

namespace Easter.Models.Dyes
{
	public class Dye : IDye
	{
        private int power;

		public Dye(int power)
		{
            Power = power;
		}

        public int Power
        {
            get
            {
                return power;
            }
            private set
            {
                if (value < 0)
                {
                    value = 0;
                }
                power = value;
            }
        }

        public bool IsFinished()
        {
            if (Power == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Use()
        {
            Power -= 10;
        }
    }
}

