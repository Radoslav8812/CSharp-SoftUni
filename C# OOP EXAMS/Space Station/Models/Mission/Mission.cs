using System;
using System.Collections.Generic;
using System.Linq;
using SpaceStation.Models.Astronauts.Contracts;
using SpaceStation.Models.Mission.Contracts;
using SpaceStation.Models.Planets.Contracts;

namespace SpaceStation.Models.Mission
{
	public class Mission : IMission
	{

        public void Explore(IPlanet planet, ICollection<IAstronaut> astronauts)
        {
            var hiredAstronauts = astronauts.Where(x => x.Oxygen > 0).ToList();

            foreach (var astrounaut in hiredAstronauts)
            {
                while (astrounaut.CanBreath && planet.Items.Any())
                {
                    var item = planet.Items.FirstOrDefault();
                    astrounaut.Bag.Items.Add(item);
                    planet.Items.Remove(item);
                    astrounaut.Breath();
                }

                if (!planet.Items.Any() || hiredAstronauts.Count == 0)
                {
                    break;
                }
            }
        }
    }
}

