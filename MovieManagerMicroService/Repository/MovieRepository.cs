using Microsoft.EntityFrameworkCore;
using MovieManagerMicroService.DataContext;
using MovieManagerMicroService.DBEntities;
using MovieManagerMicroService.DTO;
using MovieManagerMicroService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.Repository
{
    /// <summary>
    /// Movie Context
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        private MovieContext movieContext;
        
        /// <summary>
        /// Ctor - Movie Conext
        /// </summary>
        /// <param name="movieContext"></param>
        public MovieRepository(MovieContext movieContext)
        {
            this.movieContext = movieContext;
        }

        /// <summary>
        /// Adds Movies
        /// </summary>
        /// <param name="movieDto">movieDto</param>
        /// <returns>Integer value</returns>
        public async Task<int> AddMovies(MovieDTO movieDto)
        {
            // Check if there is already a show scheduled at the same time & location
            List<Movie> movies = movieContext.Movies.Where(x => x.MultiplexId == movieDto.MultiplexId).ToList();

            foreach (Movie item in movies)
            {
                if(item.DateAndTime.Date.ToShortDateString() == movieDto.DateAndTime.Date.ToShortDateString())
                    throw new CustomException(Constants.MovieExist);
            }

            Movie newMovie = new Movie
            {
                Movie_Name = movieDto.Movie_Name,
                MovieLanguage = movieDto.MovieLanguage,
                Movie_Description = movieDto.Movie_Description,
                DateAndTime = Convert.ToDateTime(movieDto.DateAndTime),
                MultiplexId = movieDto.MultiplexId,
                Genre = movieDto.Genre

            };

            await movieContext.Movies.AddAsync(newMovie);
            await movieContext.SaveChangesAsync();
            return newMovie.Id;
        }

        /// <summary>
        /// Gets the cities
        /// </summary>
        /// <returns>Cities</returns>
        public async Task<List<City>> GetCities()
        {
            return await movieContext.Cities.ToListAsync();
        }

        /// <summary>
        /// Gets the movies by multiplex id
        /// </summary>
        /// <returns>Movies</returns>
        public async Task<List<Movie>> GetMovies(int multiplexId)
        {
            return await movieContext.Movies.Where(x => x.MultiplexId == multiplexId).ToListAsync();
        }

        /// <summary>
        /// Gets the movies by Language
        /// </summary>
        /// <param name="language">language</param>
        /// <returns>Movies</returns>
        public async Task<List<Movie>> GetMoviesByLanguage(string language)
        {
            return await movieContext.Movies.Where(x => x.MovieLanguage.ToLower() == language.ToLower()).ToListAsync();
        }

        /// <summary>
        /// Gets the movies by genre
        /// </summary>
        /// <param name="genre">genre</param>
        /// <returns>Movies</returns>
        public async Task<List<Movie>> GetMoviesByGenre(string genre)
        {
            return await movieContext.Movies.Where(x => x.Genre == genre).ToListAsync();
        }

        /// <summary>
        /// Gets the multiplexes by city id
        /// </summary>
        /// <param name="cityId">cityId</param>
        /// <returns>Multiplexes</returns>
        public async Task<List<Multiplex>> GetMultiplexes(int cityId)
        {
            return await movieContext.Multiplexes.Where(x => x.CityId == cityId).ToListAsync();
        }
    }
}
