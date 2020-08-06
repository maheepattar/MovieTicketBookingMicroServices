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

        [Route("availableSeats")]
        public async Task<IActionResult> AvailableSeats(int movieId, string date)
        {
            //Fetch the existing bookings for the movie
            List<string> bookedSeats = await this.seatBookingRepository.GetBookings(movieId, date);

            //Get avaiable seats for the movie before booking
            string availableSeats = this.seatBookingRepository.AvailableSeats(seatBookingRepository.GetBookedSeats(bookedSeats));
            
            return Ok(availableSeats);
        }

        [HttpPost]
        [Route("book")]
        public async Task<IActionResult> BookMovie([FromBody] BookingDTO booking)
        {
            int totalSeatsSelected = booking.SeatNo.Split(',').Count();

            if (booking == null)
                return StatusCode(400, Constants.NullObject("booking"));

            if (!ModelState.IsValid)
                return StatusCode(400, Constants.NullObject("booking"));

            if (totalSeatsSelected  > 5)
                return StatusCode(405, Constants.MaxBooking);

            int bookedId = await this.seatBookingRepository.BookMovieInMultiplex(booking);

            if (bookedId <= 0)
                return StatusCode(500, Constants.UnknownErrors);

            return Created("bookingDetails", new 
                    { id = bookedId, Seats = booking.SeatNo, Date = booking.DateToPresent, Amount = booking.Amount });

        }

        [HttpGet]
        [Route("bookingDetails")]
        public async Task<IActionResult> BookingDetails(int id)
        {
            if (id <= 0)
                return StatusCode(400, Constants.InvalidId);

            var booking = await this.seatBookingRepository.GetBookingDetailsById(id);

            if (booking == null)
                return StatusCode(404, Constants.NoBookings);

            BookingDTO results = new BookingDTO 
                { Amount = booking.Amount, DateToPresent = Convert.ToString(booking.DateToPresent), MovieId = booking.MovieId, 
                  SeatNo = booking.SeatNo, UserId = booking.SeatNo };
            
            return Ok(results);
        }
    }
}
