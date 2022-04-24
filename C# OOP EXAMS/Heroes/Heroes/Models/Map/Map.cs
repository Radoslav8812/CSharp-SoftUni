using System;
using System.Collections.Generic;
using System.Linq;
using Heroes.Models.Contracts;
using Heroes.Repositories;

namespace Heroes.Models.Map
{
	public class Map : IMap
	{
        public Map()
        {
        }

        public string Fight(ICollection<IHero> players)
        {
            var knights = players.Where(x => x.GetType().Name == "Knight").ToList();
            var barbarians = players.Where(x => x.GetType().Name == "Barbarian").ToList();

            var deadBarbs = new List<IHero>();
            var deadKnights = new List<IHero>();

            bool boolean = false;

            while (knights.Any() && barbarians.Any())
            {
               
                for (int i = 0; i < knights.Count; i++)
                {
                    for (int j = 0; j < barbarians.Count; j++)
                    {
                        barbarians[j].TakeDamage(knights[i].Weapon.DoDamage());

                        if (barbarians[j].Health <= 0)
                        {
                            deadBarbs.Add(barbarians[j]);
                            barbarians.Remove(barbarians[j]);
                            j--;

                            if (barbarians.Count == 0)
                            {
                                boolean = true;
                                break;
                            }
                        }
                    }

                    if (boolean)
                    {
                        break;
                    }
                }

                if (boolean)
                {
                    break;
                }

                for (int i = 0; i < barbarians.Count; i++)
                {
                    for (int j = 0; j < knights.Count; j++)
                    {
                        knights[j].TakeDamage(barbarians[i].Weapon.DoDamage());
                        if (knights[j].Health <= 0)
                        {
                            deadKnights.Add(knights[j]);
                            knights.Remove(knights[j]);
                            j--;
                            if (knights.Count == 0)
                            {
                                boolean = true;
                                break;
                            }

                        }
                    }
                    if (boolean)
                    {
                        break;
                    }
                }
            }
        

            string result = string.Empty;
            if (knights.Any())
            {
                result =  $"The knights took {deadKnights.Count} casualties but won the battle.";
            }

            if (barbarians.Any())
            {
                result = $"The barbarians took {deadBarbs.Count} casualties but won the battle.";
            }

            return result;
        }
    }
}

