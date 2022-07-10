using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var softUniContext = new SoftUniContext();
            //var result = GetEmployeesFullInformation(softUniContext);
            //var result = GetEmployeesWithSalaryOver50000(softUniContext);
            //var result = GetEmployeesFromResearchAndDevelopment(softUniContext);
            //var result = AddNewAddressToEmployee(softUniContext);
            var result = GetEmployeesInPeriod(softUniContext);
            Console.WriteLine(result);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var employees = context.Employees.Select(x => new
            {
                x.EmployeeId,
                x.FirstName,
                x.LastName,
                x.MiddleName,
                x.JobTitle,
                x.Salary
            }).OrderBy(x => x.EmployeeId).ToList();

            var sb = new StringBuilder();

            foreach (var person in employees)
            {
                sb.AppendLine($"{person.FirstName} {person.LastName} {person.MiddleName} {person.JobTitle} {person.Salary:f2}");
            }

            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees.Where(x => x.Salary > 50000).Select(x => new
            {
                x.FirstName,
                x.Salary
            }).OrderBy(x => x.FirstName).ToList();

            var sb = new StringBuilder();
            foreach (var person in employees)
            {
                sb.AppendLine($"{person.FirstName} - {person.Salary:f2}");
            }
            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees.Where(x => x.Department.Name == "Research and Development").Select(x => new
            {
                x.FirstName,
                x.LastName,
                x.Department.Name,
                x.Salary
            }).OrderBy(x => x.Salary).ThenByDescending(x => x.FirstName).ToList();

            var sb = new StringBuilder();
            foreach (var person in employees)
            {
                sb.AppendLine($"{person.FirstName} {person.LastName} from Research and Development - ${person.Salary:f2}");
            }
            var result = sb.ToString().TrimEnd();
            return result;
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var nakovAddress = new Address { AddressText = "Vitoshka 15", TownId = 4 };
            var nakov = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");
            context.Addresses.Add(nakovAddress);
            nakov.Address = nakovAddress;
            context.SaveChanges();

            var employees = context.Employees.OrderByDescending(x => x.AddressId).Select(x => new
            {
                x.Address.AddressText,

            }).Take(10).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var employee in employees)
            {
                sb.AppendLine(employee.AddressText);
            }
            return sb.ToString().TrimEnd();
        }


        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees.
               Include(x => x.EmployeesProjects).
               ThenInclude(x => x.Project).
               Where(x => x.EmployeesProjects.Any(x => x.Project.StartDate.Year >= 2001 && x.Project.StartDate.Year <= 2003)).
               Select(x => new
               {
                   x.FirstName,
                   x.LastName,
                   ManagerName = x.Manager.FirstName + " " + x.Manager.LastName,
                   Projects = x.EmployeesProjects.Select(x => new
                   {
                       ProjectName = x.Project.Name,
                       ProjectStartDate = x.Project.StartDate,
                       ProjectEndDate = x.Project.EndDate
                   })
               }).Take(10).ToList();

            var sb = new StringBuilder();
            foreach (var person in employees)
            {
                sb.AppendLine($"{person.FirstName} {person.LastName} - Manager: {person.ManagerName}");

                foreach (var proj in person.Projects)
                {
                    var endDate = proj.ProjectEndDate.HasValue ? proj.ProjectEndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished";
                    sb.AppendLine($"-- {proj.ProjectName} - {proj.ProjectStartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {endDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}

