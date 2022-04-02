using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gym.Core.Contracts;
using Gym.Models.Athletes;
using Gym.Models.Athletes.Contracts;
using Gym.Models.Equipment;
using Gym.Models.Gyms;
using Gym.Models.Gyms.Contracts;
using Gym.Repositories;

namespace Gym.Core
{
    public class Controller : IController
    {
        private EquipmentRepository equipmentRepository;
        private ICollection<IGym> gyms;

        public Controller()
        {
            equipmentRepository = new EquipmentRepository();
            gyms = new List<IGym>();
        }

        public string AddAthlete(string gymName, string athleteType, string athleteName, string motivation, int numberOfMedals)
        {
            IAthlete currentAthlete;
            var currentGym = gyms.FirstOrDefault(x => x.Name == gymName);

            if (athleteType == "Boxer")
            {
                currentAthlete = new Boxer(athleteName, motivation, numberOfMedals);

                if (athleteType == "Boxer" && currentGym.GetType().Name != "BoxingGym")
                {
                    return $"The gym is not appropriate.";
                }
            }
            else  if (athleteType == "Weightlifter")
            {
                currentAthlete = new Weightlifter(athleteName, motivation, numberOfMedals);

                if (athleteType == "Weightlifter" && currentGym.GetType().Name != "WeightliftingGym")
                {
                    return $"The gym is not appropriate.";
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid athlete type.");
            }

            currentGym.AddAthlete(currentAthlete);
            return $"Successfully added {athleteType} to {gymName}.";
        }

        public string AddEquipment(string equipmentType)
        {
            if (equipmentType == "BoxingGloves")
            {
                equipmentRepository.Add(new BoxingGloves());
            }
            else if (equipmentType == "Kettlebell")
            {
                equipmentRepository.Add(new Kettlebell());
            }
            else
            {
                throw new InvalidOperationException("Invalid equipment type.");
            }

            return $"Successfully added {equipmentType}.";
        }

        public string AddGym(string gymType, string gymName)
        {
            if (gymType == "BoxingGym")
            {
                gyms.Add(new BoxingGym(gymName));
            }
            else if (gymType == "WeightliftingGym")
            {
                gyms.Add(new WeightliftingGym(gymName));
            }
            else
            {
                throw new InvalidOperationException("Invalid gym type.");
            }

            return $"Successfully added { gymType}.";
        }

        public string EquipmentWeight(string gymName)
        {
            var currentGym = gyms.FirstOrDefault(x => x.Name == gymName);
            var weight = currentGym.EquipmentWeight;

            return $"The total weight of the equipment in the gym {currentGym.Name} is {weight:f2} grams.";
        }

        public string InsertEquipment(string gymName, string equipmentType)
        {

            var currentEquipment = equipmentRepository.FindByType(equipmentType);

            if (currentEquipment == null)
            {
                throw new InvalidOperationException($"There isn’t equipment of type {equipmentType}.");
            }

            var currentGym = gyms.FirstOrDefault(x => x.Name == gymName);

            if (currentGym != null)
            {
                currentGym.AddEquipment(currentEquipment);
                equipmentRepository.Remove(currentEquipment);
            }

            return $"Successfully added {equipmentType} to {gymName}.";
        }

        public string Report()
        {
            var builder = new StringBuilder();

            foreach (var gym in gyms)
            {
                builder.AppendLine(gym.GymInfo());
            }

            return builder.ToString().Trim();
        }

        public string TrainAthletes(string gymName)
        {
            IGym currentGym = gyms.FirstOrDefault(x => x.Name == gymName);

            if (currentGym != null)
            {
                currentGym.Exercise();
            }
            
            return $"Exercise athletes: {currentGym.Athletes.Count}.";
        }
    }
}
