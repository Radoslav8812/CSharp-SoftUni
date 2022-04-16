using System;
using System.Collections.Generic;
using System.Linq;
using SpaceStation.Models.Astronauts.Contracts;
using SpaceStation.Repositories.Contracts;

namespace SpaceStation.Repositories
{
	public class AstronautRepository : IRepository<IAstronaut>
	{
        private readonly List<IAstronaut> models;

		public AstronautRepository()
		{
            models = new List<IAstronaut>();
		}

        public IReadOnlyCollection<IAstronaut> Models
        {
            get
            {
                return models;
            }
        }

        public void Add(IAstronaut model)
        {
            models.Add(model);
        }

        public IAstronaut FindByName(string name)
        {
            return models.FirstOrDefault(x => x.Name == name);
        }

        public bool Remove(IAstronaut model)
        {
            return models.Remove(model);
        }
    }
}

