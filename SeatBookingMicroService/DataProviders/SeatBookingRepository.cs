using SeatBookingMicroService.DataContext;
using SeatBookingMicroService.DBEntities;
using SeatBookingMicroService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatBookingMicroService.DataProviders
{
    public class SeatBookingRepository : ISeatBookingRepository
    {
        private readonly SeatBookingContext seatBookingContext;
        public SeatBookingRepository(SeatBookingContext _seatBookingContext)
        {
            this.seatBookingContext = _seatBookingContext;
        }

        public string AvailableSeats(List<int> bookedNumbers)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 100; i++)
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

        public int BookMovieInMultiplex(BookingDTO bookingDto)
        {
            Booking newBooking = new Booking { MovieId = bookingDto.MovieId, Amount = bookingDto.Amount, SeatNo = bookingDto.SeatNo, UserId = bookingDto.UserId, DateToPresent = Convert.ToDateTime(bookingDto.DateToPresent) };
            seatBookingContext.Bookings.Add(newBooking);
            seatBookingContext.SaveChanges();
            return newBooking.Id;
        }

        public List<int> GetBookedSeats(List<string> bookedSeats)
        {
            List<int> lstBooked = new List<int>();
            foreach (String str in bookedSeats)
            {
                string[] booked = str.Split(',');
                foreach (string s in booked)
                {
                    lstBooked.Add(Convert.ToInt32(s));
                }
            }
            return lstBooked;
        }

        public Booking GetBooking(int id)
        {
            return seatBookingContext.Bookings.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<string> GetBookings(int movieId, string date)
        {
            return seatBookingContext.Bookings.Where(x => x.MovieId == movieId
                        && x.DateToPresent.ToShortDateString() == Convert.ToDateTime(date).ToShortDateString()).Select(c => c.SeatNo).ToList();
        }

        public bool Save()
        {
            return (seatBookingContext.SaveChanges() >= 0);
        }
    }
}
