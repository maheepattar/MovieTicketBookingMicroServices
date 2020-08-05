using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagerMicroService.DTO;
using MovieManagerMicroService.ServiceProvider;

namespace MovieManagerMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        public AdminController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [HttpPost]
        [Route("addMovie")]
        public IActionResult AddMovies([FromBody] MovieDTO movieInfo)
        {
            if (movieInfo == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();

            int newId = _movieRepository.AddMovies(movieInfo);
            if (newId <= 0)
                return StatusCode(500, "A problem happened while handling your request");

            return Created("bookingDetails", new { id = newId });
        }
    }
}
