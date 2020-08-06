﻿using System;
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
        public IActionResult AvailableSeats(int movieId, string date)
        {
            //Fetch the existing bookings for the movieI
            List<string> bookedSeats = seatBookingRepository.GetBookings(movieId, date);

            //Get avaiable seats for the movie before booking
            string availableSeats = seatBookingRepository.AvailableSeats(seatBookingRepository.GetBookedSeats(bookedSeats));
            
            return Ok(availableSeats);
        }

        [HttpPost]
        [Route("book")]
        public IActionResult BookMovie([FromBody] BookingDTO booking)
        {
            if (booking == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            if (booking.SeatNo.Split(',').Count() > 5)
                return StatusCode(405, new { message = "More than 5 seats are not allowed per booking." });

            int bookedId = seatBookingRepository.BookMovieInMultiplex(booking);

            if (bookedId <= 0)
                return StatusCode(500, "Erro occured while booking. Try again.");

            return Created("bookingDetails", new { id = bookedId });

        }

        [HttpGet]
        [Route("bookingDetails")]
        public IActionResult BookingDetails(int id)
        {
            if (id <= 0)
                return StatusCode(400, "Invalid/missing id");

            var booking = seatBookingRepository.GetBooking(id);

            if (booking == null)
                return NotFound("No Bookings found");

            BookingDTO results = new BookingDTO 
                { Amount = booking.Amount, DateToPresent = Convert.ToString(booking.DateToPresent), MovieId = booking.MovieId, 
                  SeatNo = booking.SeatNo, UserId = booking.SeatNo };
            
            return Ok(results);
        }
    }
}
