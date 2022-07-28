namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Z.EntityFramework.Plus;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();

            //DbInitializer.ResetDatabase(db);
            // Console.WriteLine(GetBooksByAgeRestriction(db, "miNor"));
            //Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetBooksNotReleasedIn(db, int.Parse(Console.ReadLine())));
            //Console.WriteLine(GetAuthorNamesEndingIn(db, Console.ReadLine()));
            //Console.WriteLine(CountCopiesByAuthor(db));
            //Console.WriteLine(GetBooksByPrice(db));
            //Console.WriteLine(GetBooksReleasedBefore(db, Console.ReadLine()));
            //Console.WriteLine(GetBooksByCategory(db, Console.ReadLine()));
            //Console.WriteLine(GetBookTitlesContaining(db, Console.ReadLine()));
            //Console.WriteLine(GetBooksByAuthor(db, Console.ReadLine()));
            //Console.WriteLine(CountBooks(db, int.Parse(Console.ReadLine())));
            //Console.WriteLine(GetTotalProfitByCategory(db));
            //Console.WriteLine(GetMostRecentBooks(db));
            //IncreasePrices(db);
            Console.WriteLine(RemoveBooks(db));
        }

        //2. Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                .Where(x => x.AgeRestriction == ageRestriction)
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            var result = string.Join(Environment.NewLine, books);
            return result;
        }


        //3. Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .Select(x => new
                {
                    x.BookId,
                    x.Title
                })
                .OrderBy(x => x.BookId)
                .ToList();

            var result = string.Join(Environment.NewLine, books.ConvertAll(x => x.Title));
            return result;
        }


        //4. Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(x => new
                {
                    x.Title,
                    x.Price
                })
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            var result = sb.ToString().TrimEnd();
            return result;
        }

        //5. Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var bookTitles = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            var result = string.Join(Environment.NewLine, bookTitles);
            return result;
        }

        //6. Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                .Where(x => x.BookCategories.Any(x => categories.Contains(x.Category.Name.ToLower())))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, books);

        }


        //7. Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var sb = new StringBuilder();
            var dateTimeInput = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(x => x.ReleaseDate.Value < dateTimeInput)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new
                {
                    x.Title,
                    x.EditionType,
                    x.Price
                })
                .ToList();

            foreach (var item in books)
            {
                sb.AppendLine($"{item.Title} - {item.EditionType.ToString()} - ${item.Price:f2}");
            }

            var result = sb.ToString().TrimEnd();
            return result;
        }


        //8. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var autorNames = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => $"{x.FirstName} {x.LastName}")
                .ToList()
                .OrderBy(x => x);

            var result = string.Join(Environment.NewLine, autorNames);
            return result;
        }

        //9. Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            foreach (var item in books)
            {
                sb.AppendLine($"{item}");
            }

            return sb.ToString().TrimEnd();
        }


        //10. Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var booksAutors = context.Books
                .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(x => x.BookId)
                .Select(x => new
                {
                    x.Title,
                    AutorName = x.Author.FirstName + " " + x.Author.LastName
                })
                .ToList();


            foreach (var item in booksAutors)
            {
                sb.AppendLine($"{item.Title} ({item.AutorName})");
            }

            var result = sb.ToString().TrimEnd();
            return result;
        }


        //11. Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .ToList();

            return books.Count();
        }

        //12. Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();

            var autorCopies = context.Authors
                .Select(x => new
                {
                    AutorName = $"{x.FirstName} {x.LastName}",
                    bookCopies = x.Books.Sum(x => x.Copies)
                })
                .OrderByDescending(x => x.bookCopies)
                .ToList();

            foreach (var item in autorCopies)
            {
                sb.AppendLine($"{item.AutorName} - {item.bookCopies}");
            }

            return sb.ToString().TrimEnd();
        }


        //13. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
           var totalProfit = context.Categories
               .Select(x => new
               {
                   CategoryName = x.Name,
                   ProfitCategory = x.CategoryBooks.Sum(x => x.Book.Price * x.Book.Copies)
               })
               .OrderByDescending(x => x.ProfitCategory)
               .ThenBy(x => x.CategoryName)
               .ToList();

            return string.Join(Environment.NewLine, totalProfit.Select(x => $"{x.CategoryName} ${x.ProfitCategory:f2}"));
        }


        //14. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var books = context.Categories
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    CategoryName = x.Name,
                    CategoryBook = x.CategoryBooks.Select(x => new
                    {
                        BookTitle = x.Book.Title,
                        ReleaseDate = x.Book.ReleaseDate
                    })
                    .OrderByDescending(x => x.ReleaseDate)
                    .Take(3)
                    .ToList()
                })
                .ToList();

            return string.Join(Environment.NewLine, books.Select(x => $"--{x.CategoryName}" + Environment.NewLine +
            string.Join(Environment.NewLine, x.CategoryBook.Select(x => $"{x.BookTitle} ({x.ReleaseDate.Value.Year})"))));
        }


        //15. Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var item in books)
            {
                item.Price += 5;
            }

            context.SaveChanges();
        }

        //16. Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            return context.Books.Where(x => x.Copies < 4200).DeleteFromQuery();
        }
    }
}