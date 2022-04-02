using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasterRaces.Core.Contracts;
using EasterRaces.Models.Cars;
using EasterRaces.Models.Cars.Contracts;
using EasterRaces.Models.Drivers.Contracts;
using EasterRaces.Models.Drivers.Entities;
using EasterRaces.Models.Races.Contracts;
using EasterRaces.Models.Races.Entities;
using EasterRaces.Repositories.Contracts;
using EasterRaces.Repositories.Entities;

namespace EasterRaces.Core.Entities
{
    public class ChampionshipController : IChampionshipController
    {
        private readonly List<IDriver> driverRepo;
        private readonly List<ICar> carRepo;
        private readonly List<IRace> raceRepo;

        public ChampionshipController()
        {
            driverRepo = new List<IDriver>();
            carRepo = new List<ICar>();
            raceRepo = new List<IRace>();
        }

        public string AddCarToDriver(string driverName, string carModel)
        {
            var driver = driverRepo.FirstOrDefault(x => x.Name == driverName);
            var car = carRepo.FirstOrDefault(x => x.Model == carModel);

            if (driver == null)
            {
                throw new InvalidOperationException($"Driver {driverName} could not be found.");
            }

            if (car == null)
            {
                throw new InvalidOperationException($"Car {carModel} could not be found.");
            }

            driver.AddCar(car);
            carRepo.Remove(car);
            return $"Driver {driverName} received car {carModel}.";
        }

        public string AddDriverToRace(string raceName, string driverName)
        {
            var race = raceRepo.FirstOrDefault(x => x.Name == raceName);
            var driver = driverRepo.FirstOrDefault(x => x.Name == driverName);

            if (race == null)
            {
                throw new InvalidOperationException($"Race {raceName} could not be found.");
            }

            if (driver == null)
            {
                throw new InvalidOperationException($"Driver {driverName} could not be found.");
            }

            race.AddDriver(driver);
            return $"Driver {driverName} added in {raceName} race.";
        }

        public string CreateCar(string type, string model, int horsePower)
        {
            var existCar = carRepo.FirstOrDefault(x => x.Model == model);

            if (existCar != null)
            {
                throw new ArgumentException($"Car {model} is already create.");
            }

            ICar car = null;

            if (type == "Muscle")
            {
                car = new MuscleCar(model, horsePower);
            }
            else if (type == "Sports")
            {
                car = new SportsCar(model, horsePower);
            }

            carRepo.Add(car);
            return $"{car.GetType().Name} {model} is created.";
        }

        public string CreateDriver(string driverName)
        {
            var driverExist = driverRepo.FirstOrDefault(x => x.Name == driverName);

            if (driverExist != null)
            {
                throw new ArgumentException($"Driver {driverName} is already created.");
            }
            
            var driver = new Driver(driverName);
            driverRepo.Add(driver);
            return $"Driver {driverName} is created.";
            
        }

        public string CreateRace(string name, int laps)
        {
            if (raceRepo.FirstOrDefault(x => x.Name == name) != null)
            {
                throw new InvalidOperationException($"Race {name} is already create.");
            }
            else
            {
                var race = new Race(name, laps);
                raceRepo.Add(race);
                return $"Race {name} is created.";
            }
        }

        public string StartRace(string raceName)
        {
            var race = raceRepo.FirstOrDefault(x => x.Name == raceName);

            if (race == null)
            {
                throw new InvalidOperationException($"Race {raceName} could not be found.");
            }

            if (race.Drivers.Count < 3)
            {
                throw new InvalidOperationException($"Race {raceName} cannot start with less than 3 participants.");
            }

            var topDrivers = race.Drivers.OrderByDescending(x => x.Car.CalculateRacePoints(race.Laps)).ToList();

            var first = topDrivers[0];
            var second = topDrivers[1];
            var third = topDrivers[2];

            var builder = new StringBuilder();

            builder.AppendLine($"Driver {first.Name} wins {race.Name} race.");
            builder.AppendLine($"Driver {second.Name} is second in {race.Name} race.");
            builder.AppendLine($"Driver {third.Name} is third in {race.Name} race.");

            raceRepo.Remove(race);

            return builder.ToString().TrimEnd();
        }
    }
}
