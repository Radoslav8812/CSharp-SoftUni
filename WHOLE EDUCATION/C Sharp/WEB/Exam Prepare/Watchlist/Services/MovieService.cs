using System;
using Microsoft.EntityFrameworkCore;
using Watchlist.Contracts;
using Watchlist.Data;
using Watchlist.Data.Models;
using Watchlist.Models;

namespace Watchlist.Services
{
	public class MovieService : IMovieService
	{
        private readonly WatchlistDbContext context;

		public MovieService(WatchlistDbContext _context)
		{
            context = _context;
		}

        public async Task AddMovieAsync(AddMovieViewModel model)
        {
            var entity = new Movie()
            {
                Director = model.Director,
                GenreId = model.GenreId,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating,
                Title = model.Title
            };

            await context.Movies.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task AddMovieToCollectionAsync(int movieId, string userId)
        {
            var user = await context.Users
                .Where(x => x.Id == userId)
                .Include(x => x.UsersMovies)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID!");
            }

            var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == movieId);
            if (movie == null)
            {
                throw new ArgumentException("Invalid movie ID!");
            }

            if (!user.UsersMovies.Any(x => x.MovieId == movieId))
            {
                user.UsersMovies.Add(new UserMovie()
                {
                    MovieId = movie.Id,
                    UserId = user.Id,
                    Movie = movie,
                    User = user
                });
            }     

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MovieViewModel>> GetAllAsync()
        {
            var entities = await context.Movies
                .Include(x => x.Genre)
                .ToListAsync();

            return entities
                .Select(x => new MovieViewModel()
                {
                    Id = x.Id,
                    Director = x.Director,
                    Genre = x?.Genre?.Name,
                    ImageUrl = x.ImageUrl,
                    Rating = x.Rating,
                    Title = x.Title
                });
        }

        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            return await context.Genres.ToListAsync();
        }

        public async Task<IEnumerable<MovieViewModel>> GetWatchedAsync(string userId)
        {
            var user = await context.Users
                .Where(x => x.Id == userId)
                .Include(x => x.UsersMovies)
                .ThenInclude(um => um.Movie)
                .ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID!");
            }

            return user.UsersMovies
                .Select(x => new MovieViewModel()
                {
                    Director = x.Movie.Director,
                    Genre = x.Movie.Genre?.Name,
                    Id = x.MovieId,
                    ImageUrl = x.Movie.ImageUrl,
                    Rating = x.Movie.Rating,
                    Title = x.Movie.Title
                });
        }

        public async Task RemoveMovieFromCollectionAsync(int movieId, string userId)
        {
            var user = await context.Users
                .Where(x => x.Id == userId)
                .Include(x => x.UsersMovies)
                .ThenInclude(um => um.Movie)
                .ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID!");
            }

            var movieToRemove = user.UsersMovies.FirstOrDefault(x => x.MovieId == movieId);

            if (movieToRemove != null)
            {
                user.UsersMovies.Remove(movieToRemove);
                await context.SaveChangesAsync();
            }
        }
    }
}

