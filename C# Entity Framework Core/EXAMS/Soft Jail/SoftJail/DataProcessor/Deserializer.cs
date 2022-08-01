namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            ImportDepartmentWithCells[] departmentDtos = JsonConvert.DeserializeObject<ImportDepartmentWithCells[]>(jsonString);

            ICollection<Department> validDepartmetsList = new List<Department>();

            var sb = new StringBuilder();

            foreach (var item in departmentDtos)
            {
                if (!IsValid(item))
                {
                    sb.AppendLine($"Invalid Data");
                    continue;
                }

                if (!item.Cells.Any())
                {
                    sb.AppendLine($"Invalid Data");
                    continue;
                }

                var department = new Department()
                {
                    Name = item.Name
                };

                if (item.Cells.Any(x => !IsValid(x)))
                {
                    sb.AppendLine($"Invalid Data");
                    continue;
                }

                foreach (var itemCell in item.Cells)
                {
                    var cell = Mapper.Map<Cell>(itemCell);
                    department.Cells.Add(cell);
                }

                validDepartmetsList.Add(department);
                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.Departments.AddRange(validDepartmetsList);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}