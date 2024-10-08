﻿namespace TeisterMask.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            //NOTE: You may need to call .ToArray() function before the selection in order to detach entities from the database and avoid runtime errors (EF Core bug). 

            //var dtoProjects = context.Projects
            //    .Where(x => x.Tasks.Any()) //Export all projects that have at least one task
            //    .ToArray() // for Judge
            //    .Select(x => new ProjectOutputModel
            //    {
            //        TasksCount = x.Tasks.Count,
            //        ProjectName = x.Name,
            //        HasEndDate = x.DueDate.HasValue ? "Yes" : "No", //if it has end (due) date which is represented like "Yes" and "No". 
            //        Tasks = x.Tasks //For each task, export its name and label type. 
            //        .Select(x => new TaskOutputModel
            //        {
            //            Name = x.Name,
            //            Label = x.LabelType.ToString(),
            //        })
            //        .OrderBy(x => x.Name) //Order the tasks by name (ascending). 
            //        .ToArray()
            //    })
            //    .OrderByDescending(x => x.TasksCount) //Order the projects by tasks count (descending), then by name (ascending).
            //    .ThenBy(x => x.ProjectName)
            //    .ToArray();

            ProjectOutputModel[] dtoProjects = context.Projects.Where(x=>x.Tasks.Any()).ToArray()
                .Select(x=> new ProjectOutputModel
                {
                    TasksCount = x.Tasks.Count,  
                    ProjectName = x.Name,
                    HasEndDate = x.DueDate.HasValue ? "Yes" : "No",
                    Tasks = x.Tasks.Select(t=> new TaskOutputModel
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString(),    

                    })
                    .OrderBy(x=>x.Name)
                    .ToArray() 
                })
                .OrderByDescending(x=>x.TasksCount)
                .ThenBy(x=>x.ProjectName)
                .ToArray();

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Projects");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProjectOutputModel[]), xmlRootAttribute);
            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");
            xmlSerializer.Serialize(writer, dtoProjects, xmlSerializerNamespaces);

            return writer.ToString();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var dtoEmployees = context.Employees
                .Where(x => x.EmployeesTasks.Any(x => x.Task.OpenDate >= date))
                .ToList() // Select the top 10 employees who have at least one task that its open date is after or equal to the given date with their tasks that meet the same requirement (to have their open date after or equal to the giver date). 
                .Select(x => new //For each employee, export their username and their tasks. 
                {
                    Username = x.Username,
                    Tasks = x.EmployeesTasks.Where(x => x.Task.OpenDate >= date)
                    .OrderByDescending(x => x.Task.DueDate) //Order the tasks by due date (descending), then by name (ascending).
                    .ThenBy(x => x.Task.Name)
                    .Select(x => new //For each task, export its name and open date (must be in format "d"), due date (must be in format "d"), label and execution type. 
                    {
                        TaskName = x.Task.Name,
                        OpenDate = x.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = x.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = x.Task.LabelType.ToString(),
                        ExecutionType = x.Task.ExecutionType.ToString()
                    })
                    .ToList()
                })
                .OrderByDescending(x => x.Tasks.Count) // Order the employees by all tasks (meeting above condition) count (descending), then by username (ascending).
                .ThenBy(x => x.Username)
                .Take(10)
                .ToList();

            return JsonConvert.SerializeObject(dtoEmployees, Formatting.Indented);
        }
    }
}
