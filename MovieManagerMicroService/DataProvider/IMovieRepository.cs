using MovieManagerMicroService.DBEntities;
using MovieManagerMicroService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.ServiceProvider
{
    public interface IMovieRepository
    {
        IEnumerable<City> GetCities();

        IEnumerable<Multiplex> GetMultiplexes(int cityId);

        IEnumerable<Movie> GetMovies(int multiplexId);

        IEnumerable<Movie> GetMovies(string language);

        int AddMovies(MovieDTO movieDto);

        IEnumerable<Movie> GetMoviesByGenre(string genre);
    }
}
