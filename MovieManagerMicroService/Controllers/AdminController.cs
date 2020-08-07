using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Add new movie
        /// </summary>
        /// <param name="movieInfo">movie object</param>
        /// <returns>added movie details</returns>
        [HttpPost]
        [Route("addMovie")]
        public async Task<IActionResult> AddMovies([FromBody] MovieDTO movieInfo)
        {
            if (movieInfo == null)
                return StatusCode(400, new { message = Constants.MissingOrInvalidBody});

            if (!ModelState.IsValid)
                return StatusCode(400, new { message = Constants.MissingOrInvalidBody });

            try
            {
                int bookingResponse = await _movieRepository.AddMovies(movieInfo);
                return Created("AddedMovie", new { id = bookingResponse, Name = movieInfo.Movie_Name, Language = movieInfo.MovieLanguage });
            }
            catch(CustomException ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
