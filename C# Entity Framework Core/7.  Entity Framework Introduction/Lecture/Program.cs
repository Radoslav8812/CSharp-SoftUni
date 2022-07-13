using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFCoreInto.Models;

namespace EFCoreInto
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var count = 0;

            using (var context = new SoftUniContext())
            {
                var firstEmployee = await context.Employees.Where(x => x.JobTitle.Contains("Tool Designer")).Select(x => x.FirstName + " " + x.LastName).ToArrayAsync();
                
                for (int i = 0; i < firstEmployee.Length; i++)
                {
                    Console.WriteLine(firstEmployee[i]);
                    count++;
                }
                Console.WriteLine();
                var checkQuery = context.Employees.Where(x => x.JobTitle.Contains("Tool Designer")).Select(x => x.FirstName + " " + x.LastName).ToQueryString();
                Console.WriteLine(checkQuery);
                Console.WriteLine();
            }
            Console.WriteLine(count);
        }
    }
}

