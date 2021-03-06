﻿using MovieManagerMicroService.DBEntities;
using MovieManagerMicroService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.Repository
{
    /// <summary>
    /// Movie Repository
    /// </summary>
    public interface IMovieRepository
    {
        /// <summary>
        /// Gets the cities
        /// </summary>
        /// <returns></returns>
        Task<List<City>> GetCities();

        /// <summary>
        /// Gets Multiplexes by city id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task<List<Multiplex>> GetMultiplexes(int cityId);

        /// <summary>
        /// Gets movies by muultiplex id
        /// </summary>
        /// <param name="multiplexId">multiplexId</param>
        /// <returns><movies/returns>
        Task<List<Movie>> GetMovies(int multiplexId);

        /// <summary>
        /// Gets the movies by Language
        /// </summary>
        /// <param name="language">language</param>
        /// <returns>Movies</returns>
        Task<List<Movie>> GetMoviesByLanguage(string language);

        /// <summary>
        /// Adds Movies
        /// </summary>
        /// <param name="movie">movie</param>
        /// <returns>Integer value</returns>
        Task<int> AddMovies(Movie movie);

        /// <summary>
        /// Adds City
        /// </summary>
        /// <param name="city">city</param>
        /// <returns>Integer value</returns>
        Task<City> AddCity(City city);

        /// <summary>
        /// Adds Multiplex
        /// </summary>
        /// <param name="multiplex">multiplex</param>
        /// <returns>Integer value</returns>
        Task<Multiplex> AddMultiplex(Multiplex multiplex);

        /// <summary>
        /// Gets the movies by genre
        /// </summary>
        /// <param name="genre">genre</param>
        /// <returns>Movies</returns>
        Task<List<Movie>> GetMoviesByGenre(string genre);
    }
}
