namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";



        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Projects");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProjectInputModel[]), xmlRootAttribute);
            using StringReader reader = new StringReader(xmlString);
            ProjectInputModel[] dtoProjects = (ProjectInputModel[])xmlSerializer.Deserialize(reader);

            StringBuilder sb = new StringBuilder();
            List<Project> projects = new List<Project>();

            foreach (ProjectInputModel dtoProject in dtoProjects)
            {
                //•	If there are any validation errors for the project entity (such as invalid name or open date),
                // do not import any part of the entity and append an error message to the method output.
                bool isValidOpenDate = DateTime.TryParseExact(dtoProject.OpenDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime openDate); // => validation for OpenDate(string => Req format DateTime)

                if (!IsValid(dtoProject) || !isValidOpenDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Project newProject = new Project()
                {
                    Name = dtoProject.Name,
                    OpenDate = openDate,
                    DueDate = DateTime.TryParseExact(dtoProject.DueDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate) ? (DateTime?)dueDate : null,

                };

                //•	If there are any validation errors for the task entity (such as invalid name, open or due date are missing,
                // task open date is before project open date or task due date is after project due date), do not import it 

                List<Task> validTasks = new List<Task>();
                foreach (TaskOfProjectInputModel dtoTask in dtoProject.Tasks)
                {
                    bool isTaskOpenDateValid = DateTime.TryParseExact(dtoTask.OpenDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskOpenDate); //=> validation for TaskOpenDate(string => Req format DateTime)

                    bool isTaskDueDateValid = DateTime.TryParseExact(dtoTask.DueDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDueDate); //=> validation for TaskDueDate(string => Req format DateTime)

                    if (!IsValid(dtoTask) || !isTaskOpenDateValid || !isTaskDueDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (taskOpenDate < newProject.OpenDate || taskDueDate > newProject.DueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Task newTask = new Task()
                    {
                        Name = dtoTask.Name,
                        OpenDate = taskOpenDate, // parsed date variable
                        DueDate = taskDueDate, // parsed date variable
                        ExecutionType = Enum.Parse<ExecutionType>(dtoTask.ExecutionType), // Dto String to Enum Value
                        LabelType = Enum.Parse<LabelType>(dtoTask.LabelType) // Dto String to Enum Value

                    };
                    validTasks.Add(newTask); //add ne task to the List of Tasks
                }
                newProject.Tasks = validTasks; //add to NewProjectList collection of Tasks => (the list of the Tasks)
                projects.Add(newProject); //main list add the result objects (The new Project obj with the new tasks)
                sb.AppendLine(String.Format(SuccessfullyImportedProject, newProject.Name, newProject.Tasks.Count));
            }

            context.AddRange(projects);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            //deserialize to to C# type
            EmployeeInputModel[] dtoEmployees = JsonConvert.DeserializeObject<EmployeeInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<Employee> employeeList = new List<Employee>();

            foreach (var dtoEmpl in dtoEmployees)
            {
                // If any validation errors occur (such as invalid username, email or phone),
                //do not import any part of the entity and append an error message to the method output.
                if (!IsValid(dtoEmpl))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newEmployee = new Employee()
                {
                    Username = dtoEmpl.Username,
                    Email = dtoEmpl.Email,
                    Phone = dtoEmpl.Phone,
                };

                //•	Take only the unique tasks.
                var validTaskList = context.Tasks.Select(x => x.Id).ToList();
                foreach (var dtoTaskId in dtoEmpl.Tasks.Distinct())
                {
                    //•	If a task does not exist in the database, append an error message to the method output and continue with the next task.
                    if (!validTaskList.Contains(dtoTaskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    //Add task to the new Employee across the map class
                    newEmployee.EmployeesTasks.Add(new EmployeeTask
                    {
                        //dto task => map TaskId
                        TaskId = dtoTaskId
                    });
                }

                employeeList.Add(newEmployee);
                sb.AppendLine(String.Format(SuccessfullyImportedEmployee, newEmployee.Username, newEmployee.EmployeesTasks.Count));
            }

            context.Employees.AddRange(employeeList);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}