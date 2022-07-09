using System;
using System.Linq;
using System.Text;
using SoftUni.Data;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var softUniContext = new SoftUniContext();
            var result = GetEmployeesFullInformation(softUniContext);
            //var result = GetEmployeesWithSalaryOver50000(softUniContext);
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
    }
}

