using MovieManagerMicroService.DataContext;
using MovieManagerMicroService.DBEntities;
using MovieManagerMicroService.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagerMicroService.ServiceProvider
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext _movieContext;
        public MovieRepository(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        /// <summary>
        /// Get a list of all cities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<City> GetCities()
        {
            return _movieContext.Cities.OrderBy(x => x.CityName).AsEnumerable();
        }

        /// <summary>
        /// Get a list of multiplexes based on selected city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<Multiplex> GetMultiplexes(int cityId)
        {
            return _movieContext.Multiplexes.Where(x => x.CityId == cityId).AsEnumerable();
        }

        /// <summary>
        /// Get a list of movies based on selected multiplex
        /// </summary>
        /// <param name="multiplexId"></param>
        /// <returns></returns>
        public IEnumerable<Movie> GetMovies(int multiplexId)
        {
            return _movieContext.Movies.Where(x => x.MultiplexId == multiplexId).AsEnumerable();
        }

        /// <summary>
        /// Get a list of movies based on selected language
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public IEnumerable<Movie> GetMovies(string language)
        {
            return _movieContext.Movies.Where(x => x.MovieLanguage.ToLower() == language.ToLower()).AsEnumerable();
        }

        /// <summary>
        /// Get the movie detail for a movie
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public IEnumerable<Movie> GetMoviesByGenre(string genre)
        {
            return _movieContext.Movies.Where(x => x.Genre == genre);
        }

        public int AddMovies(MovieDTO movieDto)
        {
            Movie newMovie = new Movie
            {
                Movie_Name = movieDto.Movie_Name,
                MovieLanguage = movieDto.MovieLanguage,
                Movie_Description = movieDto.Movie_Description,
                DateAndTime = Convert.ToDateTime(movieDto.DateAndTime),
                MultiplexId = movieDto.MultiplexId,
                Genre = movieDto.Genre

            };
            _movieContext.Movies.Add(newMovie);
            _movieContext.SaveChanges();
            return newMovie.Id;
        }
    }
}
