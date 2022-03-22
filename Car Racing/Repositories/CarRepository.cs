using System;
using System.Collections.Generic;
using System.Linq;
using CarRacing.Models.Cars.Contracts;
using CarRacing.Repositories.Contracts;

namespace CarRacing.Repositories
{
	public class CarRepository : IRepository<ICar>
	{
        private List<ICar> cars;

		public CarRepository()
		{
            cars = new List<ICar>();
		}

        public IReadOnlyCollection<ICar> Models => cars;

        public void Add(ICar model)
        {
            if (model == null)
            {
                throw new ArgumentException("Cannot add null in Car Repository");
            }
            else
            {
                cars.Add(model);
            }        
        }

        public ICar FindBy(string property)
        {
            //var car = cars.FirstOrDefault(x => x.VIN == property);

            //if (cars.Contains(car))
            //{
            //    return car;
            //}
            //else
            //{
            //    return null;
            //}

            return cars.FirstOrDefault(x => x.VIN == property);
        }

        public bool Remove(ICar model)
        {
            //if (cars.Contains(model))
            //{
            //    cars.Remove(model);
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}

            return cars.Remove(model);
        }
    }
}

