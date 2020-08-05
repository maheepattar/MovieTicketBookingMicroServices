using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagerMicroService.DBEntities;
using MovieManagerMicroService.DTO;
using MovieManagerMicroService.ServiceProvider;

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

        [HttpGet]
        [Route("city")]
        public IActionResult Cities()
        {
            var results = new List<CityDTO>();
            var cities = _movieRepo.GetCities();
            if (cities == null || cities.Count() <= 0)
                return BadRequest(new { message = "City information is not available." });

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

        [HttpGet]
        [Route("multiplex/{cityId}")]
        public IActionResult Multiplex(int cityId)
        {
            var results = new List<MultiplexDTO>();
            var multiplexes = _movieRepo.GetMultiplexes(cityId);
            if (multiplexes != null && multiplexes.Count() <= 0)
                return NotFound(new { message = "No multiplex found for this city." });

            foreach (Multiplex multiplex in multiplexes)
            {
                results.Add(new MultiplexDTO
                {
                    MultiplexName = multiplex.MultiplexName,

                });
            }
            return Ok(results);
        }

        [HttpGet]
        [Route("movie/multiplex/{multiplexId}")]
        public IActionResult Movies(int multiplexId)
        {
            var results = new List<MovieDTO>();
            var movies = _movieRepo.GetMovies(multiplexId);
            if (movies != null && movies.Count() <= 0)
                return NotFound(new { message = "No movie found for this multiplex." });

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

        [HttpGet]
        [Route("movie/language/{language}")]
        public IActionResult Movies(string language)
        {
            var results = new List<MovieDTO>();
            var movies = _movieRepo.GetMovies(language);
            if (movies != null && movies.Count() <= 0)
                return NotFound(new { message = "No movie found for this multiplex." });

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

        [HttpGet]
        [Route("movie/genre/{genre}")]
        public IActionResult MoviesByGenre(string genre)
        {
            var results = new List<MovieDTO>();
            var movies = _movieRepo.GetMoviesByGenre(genre);
            if (movies != null && movies.Count() <= 0)
                return NotFound(new { message = "No movie found for this genre." });

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
