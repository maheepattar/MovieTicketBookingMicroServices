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
    /// <summary>
    /// Admin Controller
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = Role.Admin)]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMovieService _movieService;

        /// <summary>
        /// Ctor - Admin Controller
        /// </summary>
        /// <param name="movieService">movieService</param>
        public AdminController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// Add new movie
        /// </summary>
        /// <param name="movieInfo">movie object</param>
        /// <response code="200">Success</response>
        /// <response code="201">Created</response>
        /// <response code="204">No COntent</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
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
                int bookingResponse = await _movieService.AddMovies(movieInfo);
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

        /// <summary>
        /// Add new movie
        /// </summary>
        /// <param name="cityDTO">city data object</param>
        /// <response code="200">Success</response>
        /// <response code="201">Created</response>
        /// <response code="204">No COntent</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>added movie details</returns>
        [HttpPost]
        [Route("addCity")]
        public async Task<IActionResult> AddCity([FromBody] CityDTO cityDTO)
        {
            if (cityDTO == null)
                return StatusCode(400, new { message = Constants.MissingOrInvalidBody });

            if (!ModelState.IsValid)
                return StatusCode(400, new { message = Constants.MissingOrInvalidBody });

            try
            {
                var result = await _movieService.AddCity(cityDTO);
                return Created("AddedCity", new { id = result.CityId, Name = result.CityName });
            }
            catch (CustomException ex)
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
