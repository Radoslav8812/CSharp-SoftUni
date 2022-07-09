using System;
using System.Linq;
using System.Text;
using SoftUni.Data;

namespace EFhomework
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            var softUniContext = new SoftUniContext();
            var result = GetEmployeesFullInformation(softUniContext);

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
    }
}

