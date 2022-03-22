using System;
using CarRacing.Models.Cars.Contracts;
using CarRacing.Models.Racers.Contracts;

namespace CarRacing.Models.Racers
{
	public abstract class Racer : IRacer
	{
        private string userName;
        private string racingBehavior;
        private int drivingExpirience;
        ICar car;

		public Racer(string username, string racingBehavior, int drivingExperience, ICar car)
		{
            Username = username;
            RacingBehavior = racingBehavior;
            DrivingExperience = drivingExperience;
            Car = car;
		}

        public string Username
        {
            get
            {
                return userName;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Username cannot be null or empty.");
                }
                userName = value;
            }
        }

        public string RacingBehavior
        {
            get
            {
                return racingBehavior;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Racing behavior cannot be null or empty.");
                }
                racingBehavior = value;
            }
        }

        public int DrivingExperience
        {
            get
            {
                return drivingExpirience;
            }
            protected set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentException("Racer driving experience must be between 0 and 100.");
                }
                drivingExpirience = value;
            }
        }

        public ICar Car
        {
            get
            {
                return car;
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Car cannot be null or empty.");
                }
                car = value;
            }
        }

        public bool IsAvailable()
        {
            if (car.FuelAvailable >= car.FuelConsumptionPerRace)
            {
                return true;
            }
            return false;
        }

        public virtual void Race()
        {
            car.Drive();
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {Username}" + Environment.NewLine +
            $"--Driving behavior: {RacingBehavior}" + Environment.NewLine +
            $"--Driving experience: {DrivingExperience}" + Environment.NewLine +
            $"--Car: {Car.Make} {Car.Model} ({Car.VIN})";
        }
    }
}

