using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagerMicroService.DBEntities;
using MovieManagerMicroService.DTO;
using MovieManagerMicroService.ServiceProvider;
using MovieManagerMicroService.Utilities;

namespace MovieManagerMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepo;
        public MovieController(IMovieRepository movieRepository)
        {
            _movieRepo = movieRepository;
        }

        /// <summary>
        /// Get all the cities
        /// </summary>
        /// <returns>Cities</returns>
        [HttpGet]
        [Route("city")]
        public async Task<IActionResult> Cities()
        {
            var results = new List<CityDTO>();
            var cities = await _movieRepo.GetCities();

            if (cities == null || cities.Count() <= 0)
                return BadRequest(new { message =  Constants.NoCities});

            foreach (City city in cities)
            {
                results.Add(new CityDTO
                {
                    CityId = city.Id,
                    CityName = city.CityName
                });
            }

            return Ok(cities);
        }

        /// <summary>
        /// Get all multiplexes belongs the city
        /// </summary>
        /// <param name="cityId">cityId</param>
        /// <returns>list of multiplexes</returns>
        [HttpGet]
        [Route("multiplex/{cityId}")]
        public async Task<IActionResult> Multiplex(int cityId)
        {
            if (cityId <= 0)
                return StatusCode(400, new { message = Constants.InvalidId("cityId") });

            var results = new List<MultiplexDTO>();
            var multiplexes = await _movieRepo.GetMultiplexes(cityId);

            if (multiplexes != null && multiplexes.Count() <= 0)
                return NotFound(new { message = Constants.NoMultiplexes });

            foreach (Multiplex multiplex in multiplexes)
            {
                results.Add(new MultiplexDTO
                {
                    MultiplexName = multiplex.MultiplexName,

                });
            }
            return Ok(results);
        }

        /// <summary>
        /// Gets all movies are running in the given multiplex
        /// </summary>
        /// <param name="multiplexId">multiplexId</param>
        /// <returns>list of Multiplex</returns>
        [HttpGet]
        [Route("movie/multiplex/{multiplexId}")]
        public async Task<IActionResult> Movies(int multiplexId)
        {
            if (multiplexId <= 0)
                return StatusCode(400, new { message = Constants.InvalidId("multiplexId") });

            var results = new List<MovieDTO>();
            var movies = await _movieRepo.GetMovies(multiplexId);

            if (movies != null && movies.Count() <= 0)
                return NotFound(new { message = Constants.NoMovies });

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
            return Ok(results);
        }

        /// <summary>
        /// Gets the movies by language
        /// </summary>
        /// <param name="language">language</param>
        /// <returns>list of movies</returns>
        [HttpGet]
        [Route("movie/language/{language}")]
        public async Task<IActionResult> Movies(string language)
        {
            if (!string.IsNullOrWhiteSpace(language))
                return StatusCode(400, new { message = Constants.InvalidId("language") });

            var results = new List<MovieDTO>();
            var movies = await _movieRepo.GetMovies(language);

            if (movies != null && movies.Count() <= 0)
                return NotFound(new { message = Constants.NoMoviesByLanguage });

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
            return Ok(results);
        }

        /// <summary>
        /// Gets the movies by Genre
        /// </summary>
        /// <param name="genre">genre</param>
        /// <returns>list of movies</returns>
        [HttpGet]
        [Route("movie/genre/{genre}")]
        public async Task<IActionResult> MoviesByGenre(string genre)
        {
            if (!string.IsNullOrWhiteSpace(genre))
                return StatusCode(400, new { message = Constants.InvalidId("genre") });

            var results = new List<MovieDTO>();
            var movies = await _movieRepo.GetMoviesByGenre(genre);

            if (movies != null && movies.Count() <= 0)
                return NotFound(new { message = Constants.NoMoviesByGenre });

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
            return Ok(results);
        }
    }
}
