using Microsoft.EntityFrameworkCore;
using SeatBookingMicroService.DataContext;
using SeatBookingMicroService.DBEntities;
using SeatBookingMicroService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeatBookingMicroService.Utilities;

namespace SeatBookingMicroService.DataProviders
{
    public class SeatBookingRepository : ISeatBookingRepository
    {
        private readonly SeatBookingContext seatBookingContext;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="_seatBookingContext"></param>
        public SeatBookingRepository(SeatBookingContext _seatBookingContext)
        {
            this.seatBookingContext = _seatBookingContext;
        }

        /// <summary>
        /// New ticket booking
        /// </summary>
        /// <param name="booking">booking</param>
        /// <returns><ID/returns>
        public async Task<int> SubmitBooking(Booking booking)
        {
            await seatBookingContext.Bookings.AddAsync(booking);
            await seatBookingContext.SaveChangesAsync();
            return booking.Id;
        }

        /// <summary>
        /// Get Booking Details By Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Booking</returns>
        public async Task<Booking> GetBookingDetailsById(int id)
        {
            return await this.seatBookingContext.Bookings.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the seats booked for the given movie and date
        /// </summary>
        /// <param name="movieId">movie id</param>
        /// <param name="date">date</param>
        /// <returns>Seats</returns>
        public async Task<List<string>> GetBookings(int movieId, string date)
        {
            return await this.seatBookingContext.Bookings.Where(x => x.MovieId == movieId && 
                    x.DateToPresent.Date == Convert.ToDateTime(date))
                    .Select(c => c.SeatNo).ToListAsync();
        }
    }
}
