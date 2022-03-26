using System;
using System.Collections.Generic;
using Bakery.Models.BakedFoods.Contracts;
using Bakery.Models.Drinks.Contracts;
using Bakery.Models.Tables.Contracts;

namespace Bakery.Models.Tables
{
	public abstract class Table : ITable
	{
        private List<IBakedFood> FoodOrders;
        private List<IDrink> DrinksOrder;
        private int capacity;
        private int numberOfPeople;
        private decimal pricePerPerson;

		public Table(int tableNumber, int capacity, decimal pricePerPerson)
		{
            FoodOrders = new List<IBakedFood>();
            DrinksOrder = new List<IDrink>();
            TableNumber = tableNumber;
            Capacity = capacity;
            PricePerPerson = pricePerPerson;
		}

        public int TableNumber { get; protected set; }

        public int Capacity
        {
            get
            {
                return capacity;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Capacity has to be greater than 0");
                }
                capacity = value;
            }
        }

        public int NumberOfPeople
        {
            get
            {
                return numberOfPeople;
            }
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Cannot place zero or less people!");
                }
                numberOfPeople = value;
            }
        }

        public decimal PricePerPerson
        {
            get
            {
                return pricePerPerson;
            }
            private set
            {
                value = this.Price / this.NumberOfPeople;
            }
        }

        public bool IsReserved => true;

        public decimal Price
        {
            get
            {
                return pricePerPerson * numberOfPeople;
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public decimal GetBill()
        {
            throw new NotImplementedException();
        }

        public string GetFreeTableInfo()
        {
            throw new NotImplementedException();
        }

        public void OrderDrink(IDrink drink)
        {
            throw new NotImplementedException();
        }

        public void OrderFood(IBakedFood food)
        {
            throw new NotImplementedException();
        }

        public void Reserve(int numberOfPeople)
        {
            throw new NotImplementedException();
        }
    }
}

