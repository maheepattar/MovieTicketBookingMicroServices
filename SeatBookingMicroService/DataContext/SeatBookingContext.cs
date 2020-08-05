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

        public DbSet<City> Cities { get; set; }
        public DbSet<Multiplex> Multiplexes { get; set; }
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Booking> Bookings { get; set; }
    }
}
