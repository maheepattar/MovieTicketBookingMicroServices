using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatBookingMicroService.Utilities
{
    /// <summary>
    /// Defines Routes
    /// </summary>
    public class Routes
    {
        public const string AvailableSeats = "emptySeats";

        public const string SubmitBooking = "newBooking";

        public const string BookingById = "bookingDetails/{id}";
    }
}
