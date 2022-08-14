
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ProjectInputModel
    {
        [Required]
        [XmlElement(nameof(Name))]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [XmlElement(nameof(OpenDate))]
        public string OpenDate { get; set; }

        [XmlElement(nameof(DueDate))]
        public string DueDate { get; set; }

        [XmlArray(nameof(Tasks))]
        public TaskOfProjectInputModel[] Tasks { get; set; }
    }

    [XmlType("Task")]
    public class TaskOfProjectInputModel
    {
        [Required]
        [XmlElement(nameof(Name))]
        [StringLength(40, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [XmlElement(nameof(OpenDate))]
        public string OpenDate { get; set; }

        [Required]
        [XmlElement(nameof(DueDate))]
        public string DueDate { get; set; }

        [Required]
        [EnumDataType(typeof(ExecutionType))]
        public string ExecutionType { get; set; }

        [Required]
        [EnumDataType(typeof(LabelType))]
        public string LabelType { get; set; }

    }
}


//Project
//•	Id - integer, Primary Key
//•	Name - text with length [2, 40] (required)
//•	OpenDate - date and time(required)
//•	DueDate - date and time(can be null)
//•	Tasks - collection of type Task

//Task
//•	Id - integer, Primary Key
//•	Name - text with length [2, 40] (required)
//•	OpenDate - date and time(required)
//•	DueDate - date and time(required)
//•	ExecutionType - enumeration of type ExecutionType, with possible values (ProductBacklog, SprintBacklog, InProgress, Finished) (required)
//•	LabelType - enumeration of type LabelType, with possible values (Priority, CSharpAdvanced, JavaAdvanced, EntityFramework, Hibernate) (required)
//•	ProjectId - integer, foreign key(required)
//•	Project - Project
//•	EmployeesTasks - collection of type EmployeeTask
