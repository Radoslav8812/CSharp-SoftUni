using System;
using System.Collections.Generic;
using System.Linq;
using AquaShop.Models.Aquariums.Contracts;
using AquaShop.Models.Decorations.Contracts;
using AquaShop.Models.Fish.Contracts;

namespace AquaShop.Models.Aquariums
{
	public abstract class Aquarium : IAquarium
	{
        private string name;
        private int capacity;
        private ICollection<IFish> fishes;
        private ICollection<IDecoration> decorations;

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

        public int Capacity
        {
            get
            {
                return capacity;
            }
            protected set
            {
                capacity = value;
            }
        }

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
            return $"{Name} ({GetType().Name}):{Environment.NewLine}Fish: {(Fish.Count == 0 ? "none" : String.Join(", ", Fish))}{Environment.NewLine}Decorations: {this.Decorations.Count}{Environment.NewLine}Comfort: {this.Comfort}";
        }

        public bool RemoveFish(IFish fish)
        {
            return fishes.Remove(fish);
        }
    }
}

