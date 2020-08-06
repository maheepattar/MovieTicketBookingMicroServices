using Microsoft.EntityFrameworkCore;
using SeatBookingMicroService.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeatBookingMicroService.DataContext
{
    public class SeatBookingContext : DbContext
    {
        public SeatBookingContext(DbContextOptions<SeatBookingContext> options) : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Booking> Bookings { get; set; }
    }
}
