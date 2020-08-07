using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagerMicroService.DBEntities;
using MovieManagerMicroService.DTO;
using MovieManagerMicroService.ServiceProvider;
using MovieManagerMicroService.Utilities;

namespace MovieManagerMicroService.Controllers
{
    /// <summary>
    /// Movie Controller
    /// </summary>
    [Route("api/movie")]
    [Authorize(Roles = Role.Customer)]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieRepo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movieRepository"></param>
        public MovieController(IMovieService movieRepository)
        {
            _movieRepo = movieRepository;
        }

        /// <summary>
        /// Get all the cities
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="204">NoContent</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>Cities</returns>
        [HttpGet]
        [Route("city")]
        public async Task<IActionResult> Cities()
        {
            var results = new List<CityDTO>();
            var cities = await _movieRepo.GetCities();

            if (cities == null || cities.Count() <= 0)
                return NoContent();

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
        /// <response code="200">Success</response>
        /// <response code="204">NoContent</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>list of multiplexes</returns>
        [HttpGet]
        [Route("multiplex/{cityId}")]
        public async Task<IActionResult> MultiplexByCityId(int cityId)
        {
            if (cityId <= 0)
                return StatusCode(400, new { message = Constants.InvalidId("cityId") });

            var results = new List<MultiplexDTO>();

            try
            {
                var multiplexes = await _movieRepo.GetMultiplexesByCity(cityId);

                if (multiplexes != null && multiplexes.Count() <= 0)
                    return StatusCode(204, new { message = Constants.NoMultiplexes });

                foreach (Multiplex multiplex in multiplexes)
                {
                    results.Add(new MultiplexDTO
                    {
                        MultiplexName = multiplex.MultiplexName,

                    });
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = Constants.UnknownError + " Error: " + ex.Message});
            }
        }

        /// <summary>
        /// Gets all movies are running in the given multiplex
        /// </summary>
        /// <param name="multiplexId">multiplexId</param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>list of Multiplex</returns>
        [HttpGet]
        [Route("movie/multiplex/{multiplexId}")]
        public async Task<IActionResult> MoviesByMultiplexId(int multiplexId)
        {
            if (multiplexId <= 0)
                return StatusCode(400, new { message = Constants.InvalidId("multiplexId") });

            var results = new List<MovieDTO>();
            try
            {
                var movies = await _movieRepo.GetMoviesByMultiplexId(multiplexId);

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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = Constants.UnknownError + " Error: " + ex.Message });
            }
        }

        /// <summary>
        /// Gets the movies by language
        /// </summary>
        /// <param name="language">language</param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>list of movies</returns>
        [HttpGet]
        [Route("movie/language/{language}")]
        [EnableQuery()]
        public async Task<IActionResult> MoviesByLanguage(string language)
        {
            if (!string.IsNullOrWhiteSpace(language))
                return StatusCode(400, new { message = Constants.InvalidId("language") });

            var results = new List<MovieDTO>();
            try
            {
                var movies = await _movieRepo.GetMoviesByLanguage(language);
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = Constants.UnknownError + " Error: " + ex.Message });
            }
        }

        /// <summary>
        /// Gets the movies by Genre
        /// </summary>
        /// <param name="genre">genre</param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>list of movies</returns>
        [EnableQuery()]
        [HttpGet]
        [Route("movie/genre/{genre}")]
        public async Task<IActionResult> MoviesByGenre(string genre)
        {
            if (!string.IsNullOrWhiteSpace(genre))
                return StatusCode(400, new { message = Constants.InvalidId("genre") });
            var results = new List<MovieDTO>();

            try
            {
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = Constants.UnknownError + " Error: " + ex.Message });
            }
        }
    }
}
