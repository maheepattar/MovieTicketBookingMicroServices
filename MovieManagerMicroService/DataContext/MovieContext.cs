using Microsoft.EntityFrameworkCore;
using MovieManagerMicroService.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.DataContext
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
           // Database.Migrate();
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Multiplex> Multiplexes { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
