using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.DTOs.User;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            //Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));

            var dbContext = new ProductShopContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            string usersData = File.ReadAllText("../../../Datasets/users.json");
            string productsData = File.ReadAllText("../../../Datasets/products.json");
            string categoriesData = File.ReadAllText("../../../Datasets/categories.json");
            string categoryProductsData = File.ReadAllText("../../../Datasets/categories-products.json");

            var result = ImportUsers(dbContext, usersData);
            var result1 = ImportProducts(dbContext, productsData);
            var result2 = ImportCategories(dbContext, categoriesData);
            var result3 = ImportCategoryProducts(dbContext, categoryProductsData);

            //var result4 = GetProductsInRange(dbContext);
            //var result5 = GetSoldProducts(dbContext);
            //var result6 = GetCategoriesByProductsCount(dbContext);
            //var result7 = GetUsersWithProducts(dbContext);
            var result8 = GetUsersWithProducts(dbContext);
            Console.WriteLine(result8);
        }


        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);
            var users = new List<User>();

            foreach (var item in userDtos)
            {
                var user = new User
                {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Age = item.Age
                };

                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var dtoProducts = JsonConvert.DeserializeObject<List<Product>>(inputJson);
            var resultList = new List<Product>();

            foreach (var dtoItem in dtoProducts)
            {
                var product = new Product
                {
                    Name = dtoItem.Name,
                    Price = dtoItem.Price,
                    SellerId = dtoItem.SellerId,
                    BuyerId = dtoItem.BuyerId
                };

                resultList.Add(product);
            }

            context.Products.AddRange(resultList);
            context.SaveChanges();
            return $"Successfully imported {resultList.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var dtoCategories = JsonConvert.DeserializeObject<IEnumerable<CategoryImportModel>>(inputJson);
            var resultList = new List<Category>();

            foreach (var dto in dtoCategories)
            {
                if (dto.Name == null)
                {
                    continue;
                }

                var category = new Category
                {
                    Name = dto.Name
                };

                resultList.Add(category);
            }

            context.Categories.AddRange(resultList);
            context.SaveChanges();
            return $"Successfully imported {resultList.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var dtoCategoryProduct = JsonConvert.DeserializeObject<IEnumerable<CategoryProductImportModel>>(inputJson);
            var resultList = new List<CategoryProduct>();

            foreach (var dto in dtoCategoryProduct)
            {
                var categProd = new CategoryProduct
                {
                    CategoryId = dto.CategoryId,
                    ProductId = dto.ProductId
                };

                resultList.Add(categProd);
            }

            context.CategoryProducts.AddRange(resultList);
            context.SaveChanges();
            return $"Successfully imported {resultList.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .ToList();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(x => x.BuyerId != null))
                .Select(x => new
                {
                    firstname = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold.Where(x => x.BuyerId != null)
                    .Select(x => new
                    {
                        name = x.Name,
                        price = x.Price,
                        buyerFirstName = x.Buyer.FirstName,
                        buyerLastName = x.Buyer.LastName
                    })
                    .ToList()
                })
                .OrderBy(x => x.lastName)
                .ThenBy(x => x.firstname)
                .ToList();

            return JsonConvert.SerializeObject(users, Formatting.Indented);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    category = x.Name,
                    productsCount = x.CategoryProducts.Count,
                    averagePrice = x.CategoryProducts.Average(x => x.Product.Price).ToString("F2"),
                    totalRevenue = x.CategoryProducts.Sum(x => x.Product.Price).ToString("F2")
                })
                .OrderByDescending(x => x.productsCount)
                .ToList();

            return JsonConvert.SerializeObject(categories, Formatting.Indented);
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {

            var users = context.Users
                .Include(x => x.ProductsSold)
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .ToList()
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Where(p => p.BuyerId != null).Count(),
                        products = u.ProductsSold.Where(p => p.BuyerId != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                    }
                })
                .OrderByDescending(u => u.soldProducts.count)
                .ToList();

            var newObjectOfUsers = new
            {
                usersCount = users.Count,
                users = users

            };

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

            return JsonConvert.SerializeObject(newObjectOfUsers, settings);

        }
    }
}