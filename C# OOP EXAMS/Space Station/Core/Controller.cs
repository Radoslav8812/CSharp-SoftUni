using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SpaceStation.Core.Contracts;
using SpaceStation.Repositories;
using SpaceStation.Models.Astronauts.Contracts;
using SpaceStation.Models.Astronauts;
using SpaceStation.Models;
using SpaceStation.Models.Planets.Contracts;
using SpaceStation.Models.Mission.Contracts;
using SpaceStation.Models.Mission;
using SpaceStation.Utilities.Messages;
using SpaceStation.Models.Planets;

namespace SpaceStation.Core
{
	public class Controller : IController
	{
        private AstronautRepository astronautRepo;
        private PlanetRepository planetRepo;
        private IMission missionRepo;

        private int exploredPlanetCount;
        private int deadAstronautsCount;

        public Controller()
		{
            astronautRepo = new AstronautRepository();
            planetRepo = new PlanetRepository();
            missionRepo = new Mission();
		}

        public string AddAstronaut(string type, string astronautName)
        {
            IAstronaut astronaut;

            if (type == "Biologist")
            {
                 astronaut = new Biologist(astronautName);
            }
            else if (type == "Geodesist")
            {
                 astronaut = new Geodesist(astronautName);
            }
            else if (type == "Meteorologist")
            {
                 astronaut = new Meteorologist(astronautName);
            }
            else
            {
                throw new InvalidOperationException(ExceptionMessages.InvalidAstronautType);
            }

            astronautRepo.Add(astronaut);
            string result = string.Format(OutputMessages.AstronautAdded, type, astronautName);
            return result;
        }

        public string AddPlanet(string planetName, params string[] items)
        {
            var planet = new Planet(planetName);

            foreach (var item in items)
            {
                planet.Items.Add(item);
            }

            planetRepo.Add(planet);
            return $"Successfully added Planet: {planetName}!";
        }

        public string ExplorePlanet(string planetName)
        {
            var planet = planetRepo.Models.FirstOrDefault(x => x.Name == planetName);
            var requiredAstronauts = astronautRepo.Models.Where(x => x.Oxygen > 60).ToList();
            
            if (!requiredAstronauts.Any())
            {
                throw new InvalidOperationException("You need at least one astronaut to explore the planet");
            }
            
            
             missionRepo.Explore(planet, requiredAstronauts);
             exploredPlanetCount++;
            
             foreach (var astro in requiredAstronauts)
             {
                 if (!astro.CanBreath)
                 {
                     deadAstronautsCount++;
                 }
             }
    
             return $"Planet: {planet.Name} was explored! Exploration finished with {deadAstronautsCount} dead astronauts!";
        }

        public string Report()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"{exploredPlanetCount} planets were explored!");
            builder.AppendLine("Astronauts info:");

            foreach (var astro in astronautRepo.Models)
            {
                builder.AppendLine($"Name: {astro.Name}");
                builder.AppendLine($"Oxygen: {astro.Oxygen}");
                string itemsInfo = astro.Bag.Items.Any() ? string.Join(", ", astro.Bag.Items) : "none";
                builder.AppendLine($"Bag items: {itemsInfo}");
            }

            return builder.ToString().TrimEnd();
        }

        public string RetireAstronaut(string astronautName)
        {
            var astronaut = astronautRepo.FindByName(astronautName);

            if (astronaut == null)
            {
                throw new InvalidOperationException($"Astronaut {astronautName} doesn't exists!");
            }

            astronautRepo.Remove(astronaut);
            return $"Astronaut {astronautName} was retired!";
        }
    }
}

