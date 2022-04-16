using System;
using System.Collections.Generic;
using System.Linq;
using SpaceStation.Models.Planets.Contracts;
using SpaceStation.Repositories.Contracts;

namespace SpaceStation.Repositories
{
	public class PlanetRepository : IRepository<IPlanet>
	{
        private readonly List<IPlanet> models;

		public PlanetRepository()
		{
            models = new List<IPlanet>();
		}

        public IReadOnlyCollection<IPlanet> Models
        {
            get
            {
                return models;
            }
        }

        public void Add(IPlanet model)
        {
            models.Add(model);
        }

        public IPlanet FindByName(string name)
        {
            return models.FirstOrDefault(x => x.Name == name);
        }

        public bool Remove(IPlanet model)
        {
            return models.Remove(model);
        }
    }
}

