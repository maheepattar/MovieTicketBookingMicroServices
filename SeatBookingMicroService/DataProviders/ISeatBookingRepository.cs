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
        Task<int> SubmitBooking(Booking booking);

        Task<Booking> GetBookingDetailsById(int id);

        Task<List<string>> GetBookings(int movieId, string date);        
    }
}
