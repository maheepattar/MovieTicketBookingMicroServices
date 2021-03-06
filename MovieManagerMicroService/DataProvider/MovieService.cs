﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieManagerMicroService.DataContext;
using MovieManagerMicroService.DBEntities;
using MovieManagerMicroService.DTO;
using MovieManagerMicroService.Repository;
using MovieManagerMicroService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieManagerMicroService.ServiceProvider
{
    /// <summary>
    /// Movie Service
    /// </summary>
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        /// <summary>
        /// Ctor - Movie Service
        /// </summary>
        /// <param name="movieRepository">movieRepository</param>
        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        /// <summary>
        /// Get a list of all cities
        /// </summary>
        /// <returns></returns>
        public async Task<List<CityDTO>> GetCities()
        {
            var cities = new List<CityDTO>();
            var results = await _movieRepository.GetCities();
            
            foreach (City city in results)
            {
                cities.Add(new CityDTO
                {
                    CityId = city.Id,
                    CityName = city.CityName
                });
            }

            return cities;
        }

        /// <summary>
        /// Get a list of multiplexes based on selected city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public async Task<List<Multiplex>> GetMultiplexesByCity(int cityId)
        {
            return await _movieRepository.GetMultiplexes(cityId);
        }

        /// <summary>
        /// Get a list of movies based on selected multiplex
        /// </summary>
        /// <param name="multiplexId"></param>
        /// <returns></returns>
        public async Task<List<Movie>> GetMoviesByMultiplexId(int multiplexId)
        {
            return await _movieRepository.GetMovies(multiplexId);
        }

        /// <summary>
        /// Get a list of movies based on selected language
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<List<MovieDTO>> GetMoviesByLanguage(string language)
        {
            var results = new List<MovieDTO>();
            var movies = await _movieRepository.GetMoviesByLanguage(language);
            foreach (Movie movie in movies)
            {
                results.Add(new MovieDTO
                {
                    Movie_Name = movie.Movie_Name,
                    DateAndTime = movie.DateAndTime,
                    MovieLanguage = movie.MovieLanguage,
                    Movie_Description = movie.Movie_Description,
                    Genre = movie.Genre

                });
            }

            return results;
        }

        /// <summary>
        /// Get the moviea details by Genre
        /// </summary>
        /// <param name="genre">genre</param>
        /// <returns></returns>
        public async Task<List<Movie>> GetMoviesByGenre(string genre)
        {
            return await _movieRepository.GetMoviesByGenre(genre);
        }

        /// <summary>
        /// Adds the moview
        /// </summary>
        /// <param name="movieDto">movieDto</param>
        /// <returns></returns>
        public async Task<int> AddMovies(MovieDTO movieDto)
        {
            // Check if there is already a show scheduled at the same time & location
            List<Movie> movies = await this.GetMoviesByMultiplexId(movieDto.MultiplexId);

            foreach (Movie item in movies)
            {
                if (item.DateAndTime.Date.ToShortDateString() == movieDto.DateAndTime.Date.ToShortDateString())
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


            return await _movieRepository.AddMovies(newMovie);
        }

        /// <summary>
        /// Adds city
        /// </summary>
        /// <param name="cityDto">cityDto</param>
        /// <returns>City</returns>
        public async Task<CityDTO> AddCity(CityDTO cityDto)
        {
            // Check if Same has been already added into DB
            var result = await this.GetCities();
            if (result.Any(a => a.CityName == cityDto.CityName))
                throw new CustomException($"City with the name {cityDto.CityName} already added.");

            City city = new City
            {
                CityName = cityDto.CityName
            };

            var cityAdded = await _movieRepository.AddCity(city);
            cityDto.CityId = cityAdded.Id;
            return cityDto;
        }

        /// <summary>
        /// Adds multiplex
        /// </summary>
        /// <param name="multiplexDto">multiplex object</param>
        /// <returns></returns>
        public async Task<MultiplexDTO> AddMultiplex(MultiplexDTO multiplexDto)
        {
            // Check if Same has been already added into DB
            var result = await this.GetMultiplexesByCity(multiplexDto.CityId);
            if (result.Any(a => a.MultiplexName == multiplexDto.MultiplexName))
                throw new CustomException($"Multilex with the name {multiplexDto.MultiplexName} already added.");

            Multiplex multiplex = new Multiplex
            {
                MultiplexName = multiplexDto.MultiplexName,
                CityId = multiplexDto.CityId
            };

            var multiplexAdded = await _movieRepository.AddMultiplex(multiplex);
            multiplexDto.Id= multiplexAdded.Id;
            return multiplexDto;
        }
    }
}
