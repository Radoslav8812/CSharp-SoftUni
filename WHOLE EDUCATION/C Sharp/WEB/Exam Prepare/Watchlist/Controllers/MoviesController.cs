using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Watchlist.Contracts;
using Watchlist.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Watchlist.Controllers
{
    
    public class MoviesController : Controller
    {
        private readonly IMovieService movieService;

        public MoviesController(IMovieService _movieService)
        {
            movieService = _movieService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await movieService.GetAllAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddMovieViewModel()
            {
                Genres = await movieService.GetGenresAsync(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddMovieViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await movieService.AddMovieAsync(model);
                return RedirectToAction(nameof(All));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong!");
                return View(model);
            }      
        }

        [HttpPost]
        public async Task<IActionResult> AddToCollection(int movieId)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                await movieService.AddMovieToCollectionAsync(movieId, userId);
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Watched()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var model = await movieService.GetWatchedAsync(userId);

            return View("Watched", model);
        }

        public async Task<IActionResult> RemoveFromCollection(int movieId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            await movieService.RemoveMovieFromCollectionAsync(movieId, userId);

            return RedirectToAction(nameof(Watched));
        }
    }
}

