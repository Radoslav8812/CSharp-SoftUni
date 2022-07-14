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
            //var result = GetEmployeesInPeriod(softUniContext);
            //var result = GetAddressesByTown(softUniContext);
            //var result = GetEmployee147(softUniContext);
            //var result = GetDepartmentsWithMoreThan5Employees(softUniContext);
            //var result = GetLatestProjects(softUniContext);
            //var result = IncreaseSalaries(softUniContext);
            //var result = DeleteProjectById(softUniContext);
            //var result = GetEmployeesByFirstNameStartingWithSa(softUniContext);
            var result = DeleteProjectById(softUniContext);
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
                    var endDate = "not finished";
                    if (proj.ProjectEndDate != null)
                    {
                        endDate = $"{proj.ProjectEndDate:M/d/yyyy h:mm:ss tt}";
                    }
                    sb.AppendLine($"--{proj.ProjectName} - {proj.ProjectStartDate:M/d/yyyy h:mm:ss tt} - {endDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }


        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses.
                OrderByDescending(x => x.Employees.Count).
                ThenBy(x => x.Town.Name).
                ThenBy(x => x.AddressText).
                Take(10).
                Select(x => new
                {
                    text = x.AddressText,
                    town = x.Town.Name,
                    empCount = x.Employees.Count()
                }).ToList();

            var sb = new StringBuilder();
            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.text}, {address.town} - {address.empCount} employees");
            }

            return sb.ToString().TrimEnd();
        }


        public static string GetEmployee147(SoftUniContext context)
        {
            var emp = context.Employees.Where(x => x.EmployeeId == 147).
                Select(x => new
                {
                    firstName = x.FirstName,
                    lastname = x.LastName,
                    job = x.JobTitle,
                    projects = x.EmployeesProjects.Select(x => new
                    {
                        projName = x.Project.Name
                    }).
                    OrderBy(x => x.projName).ToList()
                }).FirstOrDefault();

            var sb = new StringBuilder();

            sb.AppendLine($"{emp.firstName} {emp.lastname} - {emp.job}");
            foreach (var ep in emp.projects)
            {
                sb.AppendLine($"{ep.projName}");
            }

            return sb.ToString().TrimEnd();
        }


        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var dep = context.Departments.
                Where(x => x.Employees.Count > 5).
                OrderBy(x => x.Employees.Count).
                ThenBy(x => x.Name).
                Select(x => new
                {
                    depName = x.Name,
                    mFirstName = x.Manager.FirstName,
                    mLastName = x.Manager.LastName,
                    depEmployees = x.Employees.Select(x => new
                    {
                        empFirstName = x.FirstName,
                        empLastName = x.LastName,
                        empJob = x.JobTitle
                    }).
                    OrderBy(x => x.empFirstName).
                    ThenBy(x => x.empLastName).ToList()
                }).ToList();

            var sb = new StringBuilder();
            foreach (var department in dep)
            {
                sb.AppendLine($"{department.depName} - {department.mFirstName} {department.mLastName}");

                foreach (var empl in department.depEmployees)
                {
                    sb.AppendLine($"{empl.empFirstName} {empl.empLastName} - {empl.empJob}");
                }
            }

            return sb.ToString().TrimEnd();
        }


        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    x.StartDate
                })
                .OrderBy(x => x.Name)
                .ToList();

            var sb = new StringBuilder();
            foreach (var proj in projects)
            {
                sb.AppendLine($"{proj.Name}" + Environment.NewLine +
                    $"{proj.Description}" + Environment.NewLine +
                    $"{proj.StartDate.ToString("M/d/yyyy h:mm:ss tt")}");
            }

            return sb.ToString().TrimEnd();
        }


        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.Department.Name == "Engineering" || x.Department.Name == "Tool Design" || x.Department.Name == "Marketing" || x.Department.Name == "Information Services")
                .ToList();

            foreach (var empl in employees)
            {
                empl.Salary *= 1.12m;
            }
            context.SaveChanges();


            var sb = new StringBuilder();
            foreach (var empl in employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
            {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} (${empl.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.FirstName.StartsWith("Sa"))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary
                })
                .ToList();

            var sb = new StringBuilder();
            foreach (var empl in employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
            {
                sb.AppendLine($"{empl.FirstName} {empl.LastName} - {empl.JobTitle} - (${empl.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }


        public static string DeleteProjectById(SoftUniContext context)
        {
            var emlpProjects = context.EmployeesProjects
                .Where(x => x.ProjectId == 2)
                .ToList();
            context.EmployeesProjects.RemoveRange(emlpProjects);

            var projToDelete = context.Projects.Find(2);
            context.Projects.Remove(projToDelete);
            context.SaveChanges();

            var projList = context.Projects
                .Take(10)
                .ToList();


            var sb = new StringBuilder();

            foreach (var proj in projList)
            {
                sb.AppendLine(proj.Name);
            }
            return sb.ToString().TrimEnd();
        }
    }
}

