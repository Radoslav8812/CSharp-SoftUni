using System;
using System.Collections.Generic;

namespace LINQ
{
	public class Person
	{
		public Person()
		{
			phoneNumberList = new List<PhoneNumber>();
		}

        public string Name { get; set; }
        public List<PhoneNumber> phoneNumberList { get; set; }

    }
}

