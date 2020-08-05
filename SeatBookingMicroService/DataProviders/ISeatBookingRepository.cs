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
        int BookMovieInMultiplex(BookingDTO bookingDto);

        Booking GetBooking(int id);

        List<string> GetBookings(int movieId, string date);
        bool Save();

        List<int> GetBookedSeats(List<string> bookedSeats);

        String AvailableSeats(List<int> bookedNumbers);
    }
}
