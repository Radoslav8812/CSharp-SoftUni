using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ORMFundamentals.Data.Models
{
	public class Employee
	{
		public Employee()
		{
            ManagedDepartments = new HashSet<Department>();
		}

        [Key]
		public int EmployeeID { get; set; }

        [Required]
        [MaxLength(50)]
		public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [MaxLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(50)]
        public string JobTitle { get; set; }

        [ForeignKey(nameof(Department))]
        public int DepartmentID { get; set; }
        public Department Department { get; set; }

        public virtual ICollection<Department> ManagedDepartments { get; set; }
    } 
}

