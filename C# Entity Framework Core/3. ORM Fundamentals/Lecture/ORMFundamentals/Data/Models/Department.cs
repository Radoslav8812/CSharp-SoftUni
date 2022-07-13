
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ORMFundamentals.Data.Models
{
	public class Department
	{
		public Department()
		{
			Employees = new HashSet<Employee>();
		}

        [Key]
		public int DepartmentID { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey(nameof(Employee))]		
		public int? ManagerID { get; set; }
		public virtual Employee Employee { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}

