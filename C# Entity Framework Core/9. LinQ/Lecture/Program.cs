using System;
using System.Linq;
using LINQ.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                //var employeeList = context.Employees
                //    .Where(x => x.EmployeeId < 50)
                //    .Include(x => x.Address)
                //    .ThenInclude(x => x.Town)
                //    .Select(x => new
                //{
                //    Id = x.EmployeeId,
                //    DepID = x.DepartmentId,
                //    FullName = x.FirstName + " " + x.LastName,
                //    Job = x.JobTitle,
                //    Address = x.AddressId,
                //    Town = x.Address.Town.Name

                //}).ToList();

                //var count = 0;
                //foreach (var e in employeeList)
                //{
                //    Console.WriteLine($"ID: {e.Id} ///  Name: {e.FullName}  /// Job: {e.Job} /// AddressID: {e.Address}  /// Town: {e.Town}");
                //    count++;
                //}
                //Console.WriteLine("People count: " + count);


                // join
                //var emp = context.Employees.Join(context.Departments, e => e.DepartmentId, d => d.DepartmentId, (e, d) => new
                //{
                //    FullName = $"{e.FirstName} {e.LastName}",
                //    Department = d.Name
                //}).ToList();


                //group by
                var empl = context.Employees
                    .Include(x => x.Department)
                    .GroupBy(x => new
                    {
                        x.Department.Name,
                        x.DepartmentId
                    })
                    .Select(grp => new
                    {
                        Department = grp.Key.Name,
                        EmployeeCount = grp.Count()
                    }).ToList();


                //
                var people = new List<Person>()
                {
                    new Person()
                    {
                        Name = "Tisho",
                        phoneNumberList = new List<PhoneNumber>()
                        {
                            new PhoneNumber()
                            {
                                Code = "395", Number = "23132321123",
                            },
                            new PhoneNumber()
                            {
                                Code = "395", Number = "23322311231",
                            },
                            new PhoneNumber()
                            {
                                Code = "395", Number = "21332322223",
                            }
                        }
                    },

                    new Person()
                    {
                        Name = "Spiro",
                        phoneNumberList = new List<PhoneNumber>()
                        {
                            new PhoneNumber()
                            {
                                Code = "395", Number = "233223",
                            },
                            new PhoneNumber()
                            {
                                Code = "395", Number = "231232",
                            },
                            new PhoneNumber()
                            {
                                Code = "395", Number = "231123",
                            }
                        }
                    },

                    new Person()
                    {
                        Name = "Pyro",
                        phoneNumberList = new List<PhoneNumber>()
                        {
                            new PhoneNumber()
                            {
                                Code = "395", Number = "232312435452",
                            },
                            new PhoneNumber()
                            {
                                Code = "395", Number = "454545455434",
                            },
                            new PhoneNumber()
                            {
                                Code = "395", Number = "453435434534",
                            }
                        }
                    }
                };

               
                var phones = people.SelectMany(x => x.phoneNumberList).ToList();
                for (int i = 0; i < phones.Count; i++)
                {
                    Console.WriteLine(phones[i].Number + " " + phones[i].Code);
                }

                Console.WriteLine();

                var phone = people.Select(x => x.phoneNumberList).ToList();
                var count = 0;
                foreach (var item in phone)
                {
                    foreach (var it in item)
                    {
                        Console.WriteLine(it.Number + " " + it.Code);
                        count++;
                        if (count == 3)
                        {
                            count = 0;
                        }
                    }
                }
            }
        }
    }
}

