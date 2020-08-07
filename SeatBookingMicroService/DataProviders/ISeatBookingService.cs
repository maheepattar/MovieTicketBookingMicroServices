using SeatBookingMicroService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatBookingMicroService.DataProviders
{
    public interface ISeatBookingService
    {
        Task<int> BookMovie(BookingDTO bookingDto);

        Task<BookingDTO> GetBookingDetailsById(int id);

        Task<List<string>> GetBookings(int movieId, string date);

        List<int> GetBookedSeats(List<string> bookedSeats);

        string AvailableSeats(List<int> bookedNumbers);
    }
}
