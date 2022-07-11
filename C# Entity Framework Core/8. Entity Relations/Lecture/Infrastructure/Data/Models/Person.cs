using System;
using System.Collections.Generic;

namespace Infrastructure.Data.Models
{
	public class Person
	{
		public Person()
		{
			petsList = new List<Dog>();
		}

        public int Id { get; set; }
		public string Name { get; set; }
		public List<Dog> petsList { get; set; }

    }
}

