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
        public SeatBookingRepository(SeatBookingContext _seatBookingContext)
        {
            this.seatBookingContext = _seatBookingContext;
        }

        /// <summary>
        /// Available seats for the movie
        /// </summary>
        /// <param name="bookedNumbers"></param>
        /// <returns></returns>
        public string AvailableSeats(List<int> bookedNumbers)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= Constants.MaxSeatsAllowed; i++)
            {
                if (!bookedNumbers.Contains(i))
                {
                    sb.Append(Convert.ToString(i));
                    if (i != 100)
                        sb.Append(',');
                }
            }

            return sb.ToString();
        }

        public async Task<int> BookMovieInMultiplex(BookingDTO bookingDto)
        {            
            Booking newBooking = new Booking 
                    { MovieId = bookingDto.MovieId, Amount = bookingDto.Amount, 
                      SeatNo = bookingDto.SeatNo, UserId = bookingDto.UserId, 
                      DateToPresent = Convert.ToDateTime(bookingDto.DateToPresent) 
                    };

            await seatBookingContext.Bookings.AddAsync(newBooking);
            await seatBookingContext.SaveChangesAsync();
            return newBooking.Id;
        }

        public List<int> GetBookedSeats(List<string> bookedSeats)
        {
            List<int> lstBooked = new List<int>();
            foreach (string str in bookedSeats)
            {
                string[] booked = str.Split(',');
                foreach (string s in booked)
                {
                    lstBooked.Add(Convert.ToInt32(s));
                }
            }
            return lstBooked;
        }

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
