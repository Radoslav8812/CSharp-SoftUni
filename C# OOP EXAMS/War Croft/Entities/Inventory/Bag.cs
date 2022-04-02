using System;
using System.Collections.Generic;
using System.Linq;
using WarCroft.Entities.Items;

namespace WarCroft.Entities.Inventory
{
	public abstract class Bag : IBag
	{
        private int capacity = 100;
        private readonly ICollection<Item> items;

		protected Bag(int capacity)
		{
            Capacity = capacity;
            items = new List<Item>();
		}

        public int Capacity
        {
            get
            {
                return capacity;
            }
            set
            {
                capacity = value;
            }
        }

        public int Load => items.Any() ? items.Sum(x => x.Weight) : 0;

        public IReadOnlyCollection<Item> Items => items.ToList().AsReadOnly();

        public void AddItem(Item item)
        {
            if (Load + item.Weight > Capacity)
            {
                throw new InvalidOperationException("Bag is full!");
            }

            items.Add(item);
        }

        public Item GetItem(string name)
        {
            var currentItem = items.FirstOrDefault(x => x.GetType().Name == name);

            if (!items.Any())
            {
                throw new InvalidOperationException("Bag is empty!");
            }

            if (currentItem == null)
            {
                throw new ArgumentException($"No item with name {name} in bag!");
            }

            items.Remove(currentItem);
            return currentItem;
        }
    }
}

