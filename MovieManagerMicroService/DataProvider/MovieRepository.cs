using Microsoft.EntityFrameworkCore;
using MovieManagerMicroService.DataContext;
using MovieManagerMicroService.DBEntities;
using MovieManagerMicroService.DTO;
using MovieManagerMicroService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.ServiceProvider
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext _movieContext;
        public MovieRepository(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        /// <summary>
        /// Get a list of all cities
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<City>> GetCities()
        {
            return await _movieContext.Cities.OrderBy(x => x.CityName).ToListAsync();
        }

        /// <summary>
        /// Get a list of multiplexes based on selected city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Multiplex>> GetMultiplexes(int cityId)
        {
            return await _movieContext.Multiplexes.Where(x => x.CityId == cityId).ToListAsync();
        }

        /// <summary>
        /// Get a list of movies based on selected multiplex
        /// </summary>
        /// <param name="multiplexId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Movie>> GetMovies(int multiplexId)
        {
            return await _movieContext.Movies.Where(x => x.MultiplexId == multiplexId).ToListAsync();
        }

        /// <summary>
        /// Get a list of movies based on selected language
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Movie>> GetMovies(string language)
        {
            return await _movieContext.Movies.Where(x => x.MovieLanguage.ToLower() == language.ToLower()).ToListAsync();
        }

        /// <summary>
        /// Get the movie detail for a movie
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Movie>> GetMoviesByGenre(string genre)
        {
            return await _movieContext.Movies.Where(x => x.Genre == genre).ToListAsync();
        }

        public async Task<int> AddMovies(MovieDTO movieDto)
        {
            // Check if there is already a show scheduled at the same time & location
            Movie showExist = _movieContext.Movies.Where(a => a.MultiplexId == movieDto.MultiplexId && 
                                                        a.DateAndTime.Date == movieDto.DateAndTime.Date).FirstOrDefault();

            if (showExist != null)
                throw new CustomException(Constants.MovieExist);

            Movie newMovie = new Movie
            {
                Movie_Name = movieDto.Movie_Name,
                MovieLanguage = movieDto.MovieLanguage,
                Movie_Description = movieDto.Movie_Description,
                DateAndTime = Convert.ToDateTime(movieDto.DateAndTime),
                MultiplexId = movieDto.MultiplexId,
                Genre = movieDto.Genre

            };

            await _movieContext.Movies.AddAsync(newMovie);
            _movieContext.SaveChanges();
            return newMovie.Id;
        }
    }
}
