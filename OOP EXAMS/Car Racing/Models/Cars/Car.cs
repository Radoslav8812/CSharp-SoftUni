using System;
using CarRacing.Models.Cars.Contracts;

namespace CarRacing.Models.Cars
{
	public abstract class Car : ICar
	{
        private string make;
        private string model;
        private string vin;
        private int horsePower;
        private double fuelAvailable;
        private double fuelConsumptionPerRace;

		protected Car(string make, string model, string vin, int horsePower, double fuelAvailable, double fuelConsumptionPerRace)
		{
            Make = make;
            Model = model;
            VIN = vin;
            HorsePower = horsePower;
            FuelAvailable = fuelAvailable;
            FuelConsumptionPerRace = fuelConsumptionPerRace;
		}

        public string Make
        {
            get
            {
                return make;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Car make cannot be null or empty.");
                }
                make = value;
            }
        }

        public string Model
        {
            get
            {
                return model;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Car model cannot be null or empty.");
                }
                model = value;
            }
        }

        public string VIN
        {
            get
            {
                return vin;
            }
            private set
            {
                if (value.Length != 17)
                {
                    throw new ArgumentException("Car VIN must be exactly 17 characters long.");
                }
                vin = value;
            }
        }

        public int HorsePower
        {
            get
            {
                return horsePower;
            }
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Horse power cannot be below 0.");
                }
                horsePower = value;
            }
        }

        public double FuelAvailable
        {
            get
            {
                return fuelAvailable;
            }
            private set
            {
                if (fuelAvailable < 0)
                {
                    fuelAvailable = 0;
                }
                fuelAvailable = value;
            }
        }

        public double FuelConsumptionPerRace
        {
            get
            {
                return fuelConsumptionPerRace;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Fuel consumption cannot be below 0.");
                }
                fuelConsumptionPerRace = value;
            }
        }

        public virtual void Drive()
        {
            fuelAvailable -= fuelConsumptionPerRace; // ?
        }
    }
}

