using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagerMicroService.DTO;
using MovieManagerMicroService.ServiceProvider;
using MovieManagerMicroService.Utilities;

namespace MovieManagerMicroService.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = Role.Admin)]
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
        public async Task<IActionResult> AddMovies([FromBody] MovieDTO movieInfo)
        {
            if (movieInfo == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            int newId = await _movieRepository.AddMovies(movieInfo);

            if (newId == 0)
                return StatusCode(400, "Movie already added for the same time in the multiplex");

            if (newId <= 0)
                return StatusCode(500, "Error occured while adding movie. Try again.");

            return Created("AddedMovie", new { id = newId });
        }
    }
}
