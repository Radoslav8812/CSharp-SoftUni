using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gym.Models.Athletes.Contracts;
using Gym.Models.Equipment.Contracts;
using Gym.Models.Gyms.Contracts;

namespace Gym.Models.Gyms
{
	public abstract class Gym : IGym
	{
        private string name;
        private int capacity;
        private List<IEquipment> equipments;
        private List<IAthlete> athletes;

		protected Gym(string name, int capacity)
		{
            Name = name;
            Capacity = capacity;
            equipments = new List<IEquipment>();
            athletes = new List<IAthlete>();
		}

        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Gym name cannot be null or empty.");
                }
                name = value;
            }
        }

        public int Capacity
        {
            get
            {
                return capacity;
            }
            private set
            {
                capacity = value;
            }
        }

        public double EquipmentWeight => equipments.Sum(x => x.Weight);

        public ICollection<IEquipment> Equipment
        {
            get
            {
                return equipments;
            }
        }

        public ICollection<IAthlete> Athletes
        {
            get
            {
                return athletes;
            }
        }

        public void AddAthlete(IAthlete athlete)
        {
            if (Capacity > athletes.Count)
            {
                athletes.Add(athlete);
            }
            else
            {
                throw new InvalidOperationException("Not enough space in the gym.");
            }
        }

        public void AddEquipment(IEquipment equipment)
        {
            equipments.Add(equipment);
        }

        public void Exercise()
        {
            foreach (var athlete in athletes)
            {
                athlete.Exercise();
            }
        }

        public string GymInfo()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"{Name} is a {GetType().Name}");
            builder.AppendLine($"Athletes: {(Athletes.Any() ? string.Join(", ", Athletes.Select(x => x.FullName)) : "No athletes")}");
            builder.AppendLine($"Equipment total count: {Equipment.Count}");
            builder.AppendLine($"Equipment total weight: {Equipment.Sum(x => x.Weight):f2} grams");

            return builder.ToString().TrimEnd();
        }

        public bool RemoveAthlete(IAthlete athlete)
        {
            return athletes.Remove(athlete);
        }
    }
}

