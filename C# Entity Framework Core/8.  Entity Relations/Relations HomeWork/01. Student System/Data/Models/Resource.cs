using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models
{
	public class Resource
	{
		public Resource()
		{
		}

		public int ResourceId { get; set; }

        public string Name { get; set; }

        [Required]		
        [Column(TypeName = "varchar(2048)")]
		public string Url { get; set; }

		public ResourceType ResourceType { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

    }
}

