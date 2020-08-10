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
        /// <param name="movie">movie</param>
        /// <returns>Integer value</returns>
        public async Task<int> AddMovies(Movie movie)
        {
            await movieContext.Movies.AddAsync(movie);
            await movieContext.SaveChangesAsync();
            return movie.Id;
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

        /// <summary>
        /// Adds City
        /// </summary>
        /// <param name="city">city</param>
        /// <returns>City</returns>
        public async Task<City> AddCity(City city)
        {
            await movieContext.Cities.AddAsync(city);
            await movieContext.SaveChangesAsync();
            return city;
        }

        /// <summary>
        /// Adds multiplex
        /// </summary>
        /// <param name="multiplex">multiplex</param>
        /// <returns></returns>
        public async Task<Multiplex> AddMultiplex(Multiplex multiplex)
        {
            await movieContext.Multiplexes.AddAsync(multiplex);
            await movieContext.SaveChangesAsync();
            return multiplex;
        }
    }
}
