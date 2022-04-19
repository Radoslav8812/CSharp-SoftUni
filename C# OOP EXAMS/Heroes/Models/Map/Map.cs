using System;
using System.Collections.Generic;
using System.Linq;
using Heroes.Models.Contracts;

namespace Heroes.Models.Map
{
	public class Map : IMap
	{
        private List<IHero> heroes;

		public Map()
		{
            heroes = new List<IHero>();
		}

        public string Fight(ICollection<IHero> players)
        {
            var knights = heroes.FindAll(x => x.GetType().Name == "Knight").ToList();
            var barbarians = heroes.FindAll(x => x.GetType().Name == "Barbarian").ToList();

            var deadBarbs = new List<IHero>();
            var deadKnights = new List<IHero>();

            while (true)
            {
                var knight = knights.FirstOrDefault(x => x.IsAlive == true);
                var barb = barbarians.FirstOrDefault(x => x.IsAlive == true);

                barb.TakeDamage(knight.Weapon.DoDamage());
                knight.TakeDamage(barb.Weapon.DoDamage());

                if (barb.Health == 0)
                {
                    barbarians.Remove(barb);
                    deadBarbs.Add(barb);
                }
                if (knight.Health == 0)
                {
                    knights.Remove(knight);
                    deadKnights.Add(knight);
                }

                if (!knights.Any())
                {
                    break;
                }
                if (!barbarians.Any())
                {
                    break;
                }
            }

            string result = string.Empty;
            if (knights.Any())
            {
                result =  $"The knights took {deadKnights.Count} casualties but won the battle.";
            }
            else if (barbarians.Any())
            {
                result = $"The barbarians took {barbarians.Count} casualties but won the battle.";
            }

            return result;
        }
    }
}

