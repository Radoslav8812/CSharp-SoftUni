using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs.User;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));

            var dbContext = new ProductShopContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var inputJson = File.ReadAllText("../../../Datasets/users.json");

            var result = ImportUsers(dbContext, inputJson);
            Console.WriteLine(result);

        }


        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

            var users = new List<User>();

            foreach (var item in userDtos)
            {
                var user = Mapper.Map<User>(item);
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count}";
        }
    }
}