using SeatBookingMicroService.DBEntities;
using SeatBookingMicroService.DTO;
using SeatBookingMicroService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatBookingMicroService.DataProviders
{
    /// <summary>
    /// 
    /// </summary>
    public class SeatBookingService : ISeatBookingService
    {
        private ISeatBookingRepository seatBookingRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seatBookingRepository"></param>
        public SeatBookingService(ISeatBookingRepository seatBookingRepository)
        {
            this.seatBookingRepository = seatBookingRepository;
        }

        /// <summary>
        /// AvailableSeats
        /// </summary>
        /// <param name="bookedNumbers">bookedNumbers</param>
        /// <returns>Seats</returns>
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

        /// <summary>
        /// Book Movie In Multiplex
        /// </summary>
        /// <param name="bookingDto"></param>
        /// <returns>Id</returns>
        public async Task<int> BookMovie(BookingDTO bookingDto)
        {
            Booking newBooking = new Booking
            {
                MovieId = bookingDto.MovieId,
                Amount = bookingDto.Amount,
                SeatNo = bookingDto.SeatNo,
                UserId = bookingDto.UserId,
                DateToPresent = Convert.ToDateTime(bookingDto.BookingDate)
            };

            return await seatBookingRepository.SubmitBooking(newBooking);
            
        }

        /// <summary>
        /// Ge tBooked Seats
        /// </summary>
        /// <param name="bookedSeats"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get Booking Details By Id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Booking</returns>
        public async Task<BookingDTO> GetBookingDetailsById(int id)
        {
            var booking =  await this.seatBookingRepository.GetBookingDetailsById(id);

            if (booking == null)
                return null;

            BookingDTO bookingDTO = new BookingDTO
            {
                Id = booking.Id,
                Amount = booking.Amount,
                BookingDate = booking.DateToPresent
            };

            return bookingDTO;
        }

        /// <summary>
        /// Get Bookings
        /// </summary>
        /// <param name="movieId">movieId</param>
        /// <param name="date">date</param>
        /// <returns>Booking Made</returns>
        public async Task<List<string>> GetBookings(int movieId, string date)
        {
            return await this.seatBookingRepository.GetBookings(movieId, date);
        }
    }
}
