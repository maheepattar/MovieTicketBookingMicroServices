using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeatBookingMicroService.DataProviders;
using SeatBookingMicroService.DTO;
using SeatBookingMicroService.Utilities;

namespace SeatBookingMicroService.Controllers
{
    /// <summary>
    /// Seat Booking Controller
    /// </summary>
    [Route("api/[controller]")]
    // [Authorize(Roles = Role.Customer)]
    [ApiController]
    public class SeatBookingController : ControllerBase
    {
        private readonly ISeatBookingService seatBookingService;

        /// <summary>
        /// Ctor - Seat Booking Controller
        /// </summary>
        /// <param name="_seatBookingRepository"></param>
        public SeatBookingController(ISeatBookingService seatBookingService)
        {
            this.seatBookingService = seatBookingService;
        }

        /// <summary>
        /// Gets the available seats for given moviea and date
        /// </summary>
        /// <param name="movieId">movieId</param>
        /// <param name="date">date</param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>Available seats</returns>
        [HttpGet]
        [Route(Routes.AvailableSeats)]
        public async Task<IActionResult> EmptySeats(int movieId, string date)
        {
            if (movieId <= 0)
                return StatusCode(400, new { message = Constants.InvalidInput("movieId") });

            if (string.IsNullOrWhiteSpace(date))
                return StatusCode(400, new { message = Constants.InvalidInput("date") });

            //Fetch the existing bookings for the movie
            List<string> bookedSeats = await this.seatBookingService.GetBookings(movieId, date);

            //Get avaiable seats for the movie before booking
            string availableSeats = this.seatBookingService.AvailableSeats(seatBookingService.GetBookedSeats(bookedSeats));
            
            return Ok(availableSeats);
        }

        /// <summary>
        /// Book movie
        /// </summary>
        /// <param name="booking">booking</param>
        /// <response code="200">Success</response>
        /// <response code="201">Created</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>Booking Details</returns>
        [HttpPost]
        [Route(Routes.SubmitBooking)]
        public async Task<IActionResult> BookMovie([FromBody] BookingDTO booking)
        {
            if (booking == null)
                return StatusCode(400, new { message = Constants.InvalidInput("booking") });

            if (!ModelState.IsValid)
                return StatusCode(400, new { message = Constants.InvalidInput("booking") });
            
            int totalSeatsSelected = booking.SeatNo.Split(',').Count();

            if (totalSeatsSelected > 5)
                return StatusCode(400, new { message = Constants.MaxBooking });

            int bookedId = await this.seatBookingService.BookMovie(booking);

            if (bookedId <= 0)
                return StatusCode(500, new { message = Constants.UnknownErrors });

            return Created("bookingDetails", new 
                    { id = bookedId, Seats = booking.SeatNo, Date = booking.BookingDate, Amount = booking.Amount });

        }

        /// <summary>
        /// Gets the booking details by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        /// <returns>Booked details</returns>
        [HttpGet]
        [Route(Routes.BookingById)]
        public async Task<IActionResult> BookingDetails(int id)
        {
            if (id <= 0)
                return StatusCode(400, new { message = Constants.InvalidInput("id") });

            var booking = await this.seatBookingService.GetBookingDetailsById(id);

            if (booking == null)
                return StatusCode(404, new { message = Constants.NoBookings });

            BookingDTO results = new BookingDTO 
                { Amount = booking.Amount, BookingDate = booking.BookingDate, MovieId = booking.MovieId, 
                  SeatNo = booking.SeatNo, UserId = booking.SeatNo };
            
            return Ok(results);
        }
    }
}
