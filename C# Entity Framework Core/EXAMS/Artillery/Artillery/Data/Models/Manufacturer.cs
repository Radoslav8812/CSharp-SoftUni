﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
	public class Manufacturer
	{
		public Manufacturer()
		{
			Guns = new HashSet<Gun>();
		}

		[Key]
		public int Id { get; set; }

		[Required]
		public string ManufacturerName { get; set; }

		[Required]
		public string Founded { get; set; }

		public virtual ICollection<Gun> Guns { get; set; }
	}
}

