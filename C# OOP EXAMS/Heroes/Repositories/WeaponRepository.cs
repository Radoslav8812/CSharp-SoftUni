using System;
using System.Linq;
using System.Collections.Generic;
using Heroes.Models.Contracts;
using Heroes.Repositories.Contracts;

namespace Heroes.Repositories
{
	public class WeaponRepository : IRepository<IWeapon>
	{
        private List<IWeapon> models;

		public WeaponRepository()
		{
            models = new List<IWeapon>();
		}

        public IReadOnlyCollection<IWeapon> Models
        {
            get
            {
                return models;
            }
        }

        public void Add(IWeapon model)
        {
            models.Add(model);
        }

        public IWeapon FindByName(string name)
        {
            return models.FirstOrDefault(x => x.Name == name);
        }

        public bool Remove(IWeapon model)
        {
            return models.Remove(model);
        }
    }
}

