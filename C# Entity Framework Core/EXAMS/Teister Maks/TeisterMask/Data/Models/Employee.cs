﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeisterMask.Data.Models
{
	public class Employee
	{
		public Employee()
		{
			EmployeesTasks = new HashSet<EmployeeTask>();
		}

		[Key]
		public int Id { get; set; }

		[Required]
		public string Username { get; set; } // regex

		[Required]
		public string Email { get; set; }

		[Required]
		public string Phone { get; set; } // regex

		public ICollection<EmployeeTask> EmployeesTasks { get; set; }
    }
}

