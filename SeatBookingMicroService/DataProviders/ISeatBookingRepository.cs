using SeatBookingMicroService.DBEntities;
using SeatBookingMicroService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatBookingMicroService.DataProviders
{
    public interface ISeatBookingRepository
    {
        Task<int> BookMovieInMultiplex(BookingDTO bookingDto);

        Task<Booking> GetBooking(int id);

        Task<List<string>> GetBookings(int movieId, string date);
        Task<bool> Save();

        List<int> GetBookedSeats(List<string> bookedSeats);

        string AvailableSeats(List<int> bookedNumbers);
    }
}
