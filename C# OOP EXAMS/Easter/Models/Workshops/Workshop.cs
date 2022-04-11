using System;
using System.Linq;
using Easter.Models.Bunnies;
using Easter.Models.Bunnies.Contracts;
using Easter.Models.Dyes;
using Easter.Models.Dyes.Contracts;
using Easter.Models.Eggs.Contracts;
using Easter.Models.Workshops.Contracts;

namespace Easter.Models.Workshops
{
	public class Workshop : IWorkshop
	{
		public Workshop()
		{
		}

        public void Color(IEgg egg, IBunny bunny)
        {
            //IDye currDye = bunny.Dyes.FirstOrDefault();

            //if (bunny.GetType().Name == "HappyBunny" && !currDye.IsFinished())
            //{
            //    if (bunny.Energy >= 10)
            //    {
            //        bunny.Work();
            //        egg.GetColored();
            //    }

            //    if (currDye.IsFinished())
            //    {
            //        bunny.Dyes.Remove(currDye);
            //    }
            //}
            //else if (bunny.GetType().Name == "SleepyBunny" && !currDye.IsFinished())
            //{
            //    if (bunny.Energy >= 15)
            //    {
            //        bunny.Work();
            //        egg.GetColored();
            //    }

            //    if (currDye.IsFinished())
            //    {
            //        bunny.Dyes.Remove(currDye);
            //    }
            //}


            foreach (var dye in bunny.Dyes)
            {
                while (!dye.IsFinished() && bunny.Energy > 0)
                {
                    if (!egg.IsDone())
                    {
                        egg.GetColored();
                        dye.Use();
                        bunny.Work();
                    }
                    else
                    {
                        break;
                    }
                }
                if (egg.IsDone() || bunny.Energy <= 0)
                {
                    break;
                }
            }

            bunny.Dyes.ToList().RemoveAll(d => d.Power == 0);
        }
    }
}

