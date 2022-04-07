using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AquaShop.Core.Contracts;
using AquaShop.Models.Aquariums;
using AquaShop.Models.Aquariums.Contracts;
using AquaShop.Models.Decorations;
using AquaShop.Models.Decorations.Contracts;
using AquaShop.Models.Fish;
using AquaShop.Models.Fish.Contracts;
using AquaShop.Repositories;
using AquaShop.Repositories.Contracts;

namespace AquaShop.Core
{
    public class Controller : IController
    {
        private IRepository<IDecoration> decorationRepository;
        private ICollection<IAquarium> aquariums;

        public Controller()
        {
            decorationRepository = new DecorationRepository();
            aquariums = new List<IAquarium>();
        }

        public string AddAquarium(string aquariumType, string aquariumName)
        {
            IAquarium aquarium;

            if (aquariumType == nameof(FreshwaterAquarium))
            {
                aquarium = new FreshwaterAquarium(aquariumName);
            }
            else if (aquariumType == nameof(SaltwaterAquarium))
            {
                aquarium = new SaltwaterAquarium(aquariumName);
            }
            else
            {
                throw new InvalidOperationException("Invalid aquarium type.");
            }

            aquariums.Add(aquarium);
            return $"Successfully added {aquariumType}.";
        }

        public string AddDecoration(string decorationType)
        {
            IDecoration decoration;

            if (decorationType == nameof(Ornament))
            {
                decoration = new Ornament();
            }
            else if (decorationType == nameof(Plant))
            {
                decoration = new Plant();
            }
            else
            {
                throw new InvalidOperationException("Invalid decoration type.");
            }

            decorationRepository.Add(decoration);
            return $"Successfully added {decorationType}.";
        }

        public string AddFish(string aquariumName, string fishType, string fishName, string fishSpecies, decimal price)
        {
            IFish fish;
            IAquarium aquarium = aquariums.FirstOrDefault(x => x.Name == aquariumName);

           if (fishType == nameof(FreshwaterFish))
           {
               fish = new FreshwaterFish(fishName, fishSpecies, price);
           }
           else if (fishType == nameof(SaltwaterFish))
           {
               fish = new SaltwaterFish(fishName, fishSpecies, price);
           }
           else
           {
               throw new InvalidOperationException("Invalid fish type.");
           }
           
           if(fish.GetType() == typeof(FreshwaterFish) && aquarium.GetType() == typeof(SaltwaterAquarium))
           {
               return "Water not suitable.";
           }
           
           if (fish.GetType() == typeof(SaltwaterFish) && aquarium.GetType() == typeof(FreshwaterAquarium))
           {
               return "Water not suitable.";
           }

            aquarium.AddFish(fish);
            return $"Successfully added {fishType} to {aquariumName}.";
        }

        public string CalculateValue(string aquariumName)
        {
            IAquarium aquarium = aquariums.FirstOrDefault(x => x.Name == aquariumName);
            decimal currentValue = aquarium.Fish.Sum(x => x.Price) + aquarium.Decorations.Sum(x => x.Price);
            
            return $"The value of Aquarium {aquarium.Name} is {currentValue:f2}.";
        }

        public string FeedFish(string aquariumName)
        {
            var aqurium = aquariums.FirstOrDefault(x => x.Name == aquariumName);
            aqurium.Feed();

            return $"Fish fed: {aqurium.Fish.Count}";
        }

        public string InsertDecoration(string aquariumName, string decorationType)
        {
            var aquarium = aquariums.FirstOrDefault(x => x.Name == aquariumName);
            var decoration = decorationRepository.FindByType(decorationType);

            if (decoration == null)
            {
                throw new InvalidOperationException($"There isn't a decoration of type {decorationType}.");
            }

            aquarium.AddDecoration(decoration);
            decorationRepository.Remove(decoration);
           
            return $"Successfully added {decorationType} to {aquariumName}.";
        }

        public string Report()
        {
            var builder = new StringBuilder();

            foreach (var item in aquariums)
            {
                builder.AppendLine(item.GetInfo());
            }

            return builder.ToString().TrimEnd();
        }
    }
}

