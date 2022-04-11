using System;
using System.Linq;
using System.Collections;
using Easter.Core.Contracts;
using Easter.Models.Bunnies;
using Easter.Models.Bunnies.Contracts;
using Easter.Models.Dyes;
using Easter.Models.Dyes.Contracts;
using Easter.Models.Eggs;
using Easter.Models.Eggs.Contracts;
using Easter.Repositories;
using Easter.Repositories.Contracts;
using Easter.Models.Workshops;
using Easter.Models.Workshops.Contracts;
using System.Text;

namespace Easter.Core
{
	public class Controller : IController
	{
        private IRepository<IBunny> bunnies;
        private IRepository<IEgg> eggs;
        private int countColoredEggs;

		public Controller()
		{
            bunnies = new BunnyRepository();
            eggs = new EggRepository();
		}

        public string AddBunny(string bunnyType, string bunnyName)
        {
            if (bunnyType == "HappyBunny")
            {
                IBunny bunny = new HappyBunny(bunnyName);
                bunnies.Add(bunny);
            }
            else if (bunnyType == "SleepyBunny")
            {
                IBunny bunny = new SleepyBunny(bunnyName);
                bunnies.Add(bunny);
            }
            else
            {
                throw new InvalidOperationException("Invalid bunny type.");
            }

            return $"Successfully added {bunnyType} named {bunnyName}.";
        }

        public string AddDyeToBunny(string bunnyName, int power)
        {
            IDye currDye = new Dye(power);
            IBunny currBunny = bunnies.FindByName(bunnyName);

            if (currBunny == null)
            {
                throw new InvalidOperationException("The bunny you want to add a dye to doesn't exist!");
            }
            else
            {
                currBunny.AddDye(currDye);
                return $"Successfully added dye with power {currDye.Power} to bunny {bunnyName}!";
            }
            
        }

        public string AddEgg(string eggName, int energyRequired)
        {
            IEgg currEgg = new Egg(eggName, energyRequired);
            eggs.Add(currEgg);
            return $"Successfully added egg: {currEgg.Name}!";
        }

        public string ColorEgg(string eggName)
        {
            IWorkshop workShop = new Workshop();
            var currEgg = eggs.FindByName(eggName);
            var mostPowerfullBunnies = bunnies.Models.Where(x => x.Energy >= 50).ToList();

            if (mostPowerfullBunnies.Count == 0)
            {
                throw new InvalidOperationException("There is no bunny ready to start coloring!");
            }

            foreach (var bunny in mostPowerfullBunnies.OrderByDescending(x => x.Energy))
            {
                workShop.Color(currEgg, bunny);

                if (bunny.Energy == 0)
                {
                    mostPowerfullBunnies.Remove(bunny);
                }

            }

            if (currEgg.IsDone())
            {
                countColoredEggs++;
                return $"Egg {eggName} is done.";
            }
            else
            {
                return $"Egg {eggName} is not done.";
            }  
        }

        public string Report()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"{countColoredEggs} eggs are done!");
            builder.AppendLine("Bunnies info:");

            foreach (var bunny in bunnies.Models.Where(x => x.Energy > 0))
            {
                builder.AppendLine(bunny.ToString());
                
            }

            return builder.ToString().TrimEnd();
        }
    }
}

