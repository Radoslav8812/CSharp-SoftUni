using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Library.Models;
using Library.Services;
using Library.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Library.Controllers
{

    public class BooksController : Controller
    {
        private readonly IBooksService booksService;

        public BooksController(IBooksService _booksService)
        {
            booksService = _booksService;
        }

        public async Task<IActionResult> All()
        {
            var model = await booksService.GetAllBooksAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddBookViewModel();
            var categories = await booksService.GetAllCategoriesAsync();

            model.Categories = categories.ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await booksService.AddBookAsync(model);
                return RedirectToAction("All", "Books");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong!");

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCollection(int bookId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            try
            {
                await booksService.AddToCollectionAsync(bookId, userId);
            }
            catch (Exception)
            {
            }

            return RedirectToAction("All", "Books");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int bookId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            try
            {
                await booksService.RemoveFromCollectionAsync(bookId, userId);
                return RedirectToAction("Mine", "Books");
            }
            catch
            {
                return RedirectToAction("Mine", "Books");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            try
            {
                var books = await booksService.GetUserBooksAsync(userId);

                return View(books);

            }
            catch (Exception)
            {
                return RedirectToAction("All", "Books");
            }
        }
    }
}

