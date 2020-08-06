using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatBookingMicroService.Utilities
{
    public class Constants
    {
        public const int MaxSeatsAllowed = 100;

        public const string NoBookings = "No Bookings found for given id ";

        public const string InvalidId = "Invalid or missing id.";

        public const string MaxBooking = "Maximum 5 seats are allowed in an single booking";

        public const string UnknownErrors = "Erro occured while your request. Try again.";

        public static string NullObject(string objectName)
        { 
            return $"Invalid or missing {objectName}";
        }
    }
}