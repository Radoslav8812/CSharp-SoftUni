using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AquaShop.Models.Aquariums.Contracts;
using AquaShop.Models.Decorations.Contracts;
using AquaShop.Models.Fish.Contracts;

namespace AquaShop.Models.Aquariums
{
	public abstract class Aquarium : IAquarium
	{
        private string name;

        private List<IFish> fishes;
        private List<IDecoration> decorations;

        public Aquarium(string name, int capacity)
		{
            Name = name;
            Capacity = capacity;

            decorations = new List<IDecoration>();
            fishes = new List<IFish>();
		}

        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Aquarium name cannot be null or empty.");
                }
                name = value;
            }
        }

        public int Capacity { get; }

        public int Comfort => Decorations.Sum(x => x.Comfort);

        public ICollection<IDecoration> Decorations
        {
            get
            {
                return decorations;
            }
        }

        public ICollection<IFish> Fish
        {
            get
            {
                return fishes;
            }
        }

        public void AddDecoration(IDecoration decoration)
        {
            Decorations.Add(decoration);
        }

        public void AddFish(IFish fish)
        {
            if (fishes.Count < this.Capacity)
            {
                Fish.Add(fish);
            }
            else
            {
                throw new InvalidOperationException("Not enough capacity.");
            }
        }

        public void Feed()
        {
            foreach (var fish in fishes)
            {
                fish.Eat();
            }
        }

        public string GetInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{this.Name} ({this.GetType().Name}):");
            sb.AppendLine($"Fish: {(this.Fish.Any() ? string.Join(", ", this.Fish.Select(x => x.Name)) : "none")}");
            sb.AppendLine($"Decorations: {this.Decorations.Count}");
            sb.AppendLine($"Comfort: {this.Comfort}");

            return sb.ToString().TrimEnd();
        }

        public bool RemoveFish(IFish fish)
        {
            return Fish.Remove(fish);
        }
    }
}

