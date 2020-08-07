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
    [Route("api/[controller]")]
    [Authorize(Roles = Role.Customer)]
    [ApiController]
    public class SeatBookingController : ControllerBase
    {
        private readonly ISeatBookingRepository seatBookingRepository;

        public SeatBookingController(ISeatBookingRepository _seatBookingRepository)
        {
            this.seatBookingRepository = _seatBookingRepository;
        }

        /// <summary>
        /// Gets the available seats for given moviea and date
        /// </summary>
        /// <param name="movieId">movieId</param>
        /// <param name="date">date</param>
        /// <returns>Available seats</returns>
        [HttpGet]
        [Route("availableSeats")]
        public async Task<IActionResult> AvailableSeats(int movieId, string date)
        {
            if (movieId <= 0)
                return StatusCode(400, new { message = Constants.InvalidInput("movieId") });

            if (!string.IsNullOrWhiteSpace(date))
                return StatusCode(400, new { message = Constants.InvalidInput("date") });

            //Fetch the existing bookings for the movie
            List<string> bookedSeats = await this.seatBookingRepository.GetBookings(movieId, date);

            //Get avaiable seats for the movie before booking
            string availableSeats = this.seatBookingRepository.AvailableSeats(seatBookingRepository.GetBookedSeats(bookedSeats));
            
            return Ok(availableSeats);
        }

        /// <summary>
        /// Book movie
        /// </summary>
        /// <param name="booking">booking</param>
        /// <returns>Booking Details</returns>
        [HttpPost]
        [Route("book")]
        public async Task<IActionResult> BookMovie([FromBody] BookingDTO booking)
        {
            if (booking == null)
                return StatusCode(400, new { message = Constants.InvalidInput("booking") });

            if (!ModelState.IsValid)
                return StatusCode(400, new { message = Constants.InvalidInput("booking") });
            
            int totalSeatsSelected = booking.SeatNo.Split(',').Count();
            if (totalSeatsSelected > 5)
                return StatusCode(405, new { message = Constants.MaxBooking });

            int bookedId = await this.seatBookingRepository.BookMovieInMultiplex(booking);

            if (bookedId <= 0)
                return StatusCode(500, new { message = Constants.UnknownErrors });

            return Created("bookingDetails", new 
                    { id = bookedId, Seats = booking.SeatNo, Date = booking.DateToPresent, Amount = booking.Amount });

        }

        /// <summary>
        /// Gets the booking details by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Booked details</returns>
        [HttpGet]
        [Route("bookingDetails/{id}")]
        public async Task<IActionResult> BookingDetails(int id)
        {
            if (id <= 0)
                return StatusCode(400, new { message = Constants.InvalidInput("id") });

            var booking = await this.seatBookingRepository.GetBookingDetailsById(id);

            if (booking == null)
                return StatusCode(404, new { message = Constants.NoBookings });

            BookingDTO results = new BookingDTO 
                { Amount = booking.Amount, DateToPresent = Convert.ToString(booking.DateToPresent), MovieId = booking.MovieId, 
                  SeatNo = booking.SeatNo, UserId = booking.SeatNo };
            
            return Ok(results);
        }
    }
}
