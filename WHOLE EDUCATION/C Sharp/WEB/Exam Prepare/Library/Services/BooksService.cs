using System;
using Library.Data;
using Library.Data.Models;
using Library.Models;
using Library.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BooksService : IBooksService
    {
        private readonly LibraryDbContext context;

        public BooksService(LibraryDbContext _context)
        {
            context = _context;
        }

        public async Task AddBookAsync(AddBookViewModel model)
        {
            var book = new Book()
            {
                Title = model.Title,
                Author = model.Author,
                CategoryId = model.CategoryId,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating,
                Description = model.Description,
            };

            var category = await context.Categories
                .Where(x => x.Id == model.CategoryId)
                .FirstOrDefaultAsync();

            category.Books.Add(book);
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();
        }

        public async Task AddToCollectionAsync(int bookId, string userId)
        {
            var book = context.Books.FirstOrDefaultAsync(x => x.Id == bookId);
            if (book == null)
            {
                throw new ArgumentException("Invalid book Id!");
            }

            var user = await context.Users
                .Where(x => x.Id == userId)
                .Include(x => x.ApplicationUsersBooks)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentException("Invalid user Id!");
            }

            if (user.ApplicationUsersBooks.Any(x => x.BookId == bookId))
            {
                throw new ArgumentException("The book is already added!");
            }

            var userBook = new ApplicationUserBook()
            {
                BookId = bookId,
                ApplicationUserId = userId
            };

            user.ApplicationUsersBooks.Add(userBook);
            await context.SaveChangesAsync();
        }

        public async Task<ICollection<BookViewModel>> GetAllBooksAsync()
        {
            return await context.Books
                .Include(x => x.Category)
                .Select(x => new BookViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    Category = x.Category.Name,
                    ImageUrl = x.ImageUrl,
                    Rating = x.Rating
                }).ToListAsync();
        }

        public async Task<ICollection<Category>> GetAllCategoriesAsync()
        {
            var categories = await context.Categories.ToListAsync();

            return categories;
        }

        public async Task<ICollection<MineBooksViewModel>> GetUserBooksAsync(string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user Id!");
            }

            var books = await context.ApplicationUserBooks
                 .Where(x => x.ApplicationUserId == userId)
                 .Select(x => x.Book)
                 .Select(b => new MineBooksViewModel()
                 {
                     Id = b.Id,
                     Title = b.Title,
                     ImageUrl = b.ImageUrl,
                     Author = b.Author,
                     Category = b.Category.Name,
                     Description = b.Description
                 })
                 .ToListAsync();

            return books;
        }

        public async Task RemoveFromCollectionAsync(int bookId, string userId)
        {
            var user = await context.Users
                .Include(x => x.ApplicationUsersBooks)
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user Id!");
            }

            if (!user.ApplicationUsersBooks.Any(x => x.BookId == bookId))
            {
                throw new ArgumentException("Invalid book Id!");
            }

            var bookToRemove = user.ApplicationUsersBooks.Where(x => x.BookId == bookId).FirstOrDefault();

            user.ApplicationUsersBooks.Remove(bookToRemove);
            await context.SaveChangesAsync();
        }
    }
}

