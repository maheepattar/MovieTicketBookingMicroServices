using MovieManagerMicroService.DBEntities;
using MovieManagerMicroService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.ServiceProvider
{
    public interface IMovieService
    {
        Task<List<City>> GetCities();

        Task<List<Multiplex>> GetMultiplexesByCity(int cityId);

        Task<List<Movie>> GetMoviesByMultiplexId(int multiplexId);

        Task<List<MovieDTO>> GetMoviesByLanguage(string language);

        Task<int> AddMovies(MovieDTO movieDto);

        Task<List<Movie>> GetMoviesByGenre(string genre);
    }
}
