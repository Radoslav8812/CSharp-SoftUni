﻿using System;
using System.Collections.Generic;
using SpaceStation.Models.Bags.Contracts;

namespace SpaceStation.Models.Bags
{
	public class Backpack : IBag
	{
		private List<string> items;

		public Backpack()
		{
			items = new List<string>();
		}

        public ICollection<string> Items
        {
            get
            {
                return items;
            }
        }
    }
}

