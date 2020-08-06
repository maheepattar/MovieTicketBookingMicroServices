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
        Task<IEnumerable<City>> GetCities();

        Task<IEnumerable<Multiplex>> GetMultiplexes(int cityId);

        Task<IEnumerable<Movie>> GetMovies(int multiplexId);

        Task<IEnumerable<Movie>> GetMovies(string language);

        Task<int> AddMovies(MovieDTO movieDto);

        Task<IEnumerable<Movie>> GetMoviesByGenre(string genre);
    }
}
