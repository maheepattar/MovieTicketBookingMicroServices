using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieManagerMicroService.Controllers;
using MovieManagerMicroService.DBEntities;
using MovieManagerMicroService.DTO;
using MovieManagerMicroService.Repository;
using MovieManagerMicroService.ServiceProvider;
using MovieManagerMicroService.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagerMicroServiceUnitTest
{
    public class AdminControllerTest
    {
        private AdminController _adminController;
        private Mock<IMovieService> _movieServiceMock;

        private MovieService _movieService;
        private Mock<IMovieRepository> _movieRepositoryMock;

        [SetUp]
        public void Setup()
        {
            Initialization();
            _adminController = new AdminController(_movieServiceMock.Object);
            _movieService = new MovieService(_movieRepositoryMock.Object);
        }

        private void Initialization()
        {
            _movieServiceMock = new Mock<IMovieService>();
            _movieRepositoryMock = new Mock<IMovieRepository>();

            List<Movie> movies = new List<Movie>()
            {
                new Movie{ Id = 1, MultiplexId = 1, DateAndTime = DateTime.Now.AddDays(1), Movie_Name = "Raj" },
                new Movie{ Id =21, MultiplexId = 1, DateAndTime = DateTime.Now.AddDays(1), Movie_Name = "Harry" }
            };

            _movieRepositoryMock.Setup(x => x.GetMovies(It.IsAny<int>())).ReturnsAsync(movies);

            List<City> cities = new List<City>()
            {
                new City{ Id = 1, CityName = "Chennai"},
                new City{ Id =21, CityName = "Hyderbad"}
            };

            _movieRepositoryMock.Setup(x => x.GetCities()).ReturnsAsync(cities);
            _movieRepositoryMock.Setup(a => a.AddCity(new City())).ReturnsAsync(new City { Id = 5, CityName = "Jaipur" });
        }
       
        [Test]
        public async Task WhenMovieObject_IsNull_ReturnsBadRequest()
        {
            // Arrange
            MovieDTO movieDTO = null;

            // Act
            var result = (ObjectResult) await _adminController.AddMovies(movieDTO);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void WhenMovieDataValid_OnlyOneShowPerDay_InEachMultiplex_ElseReturnsBadRequest_()
        {
            // Arrange
            MovieDTO movieDTO = new MovieDTO() { 
            Movie_Name = "Power", MovieLanguage = "Kannada", DateAndTime = DateTime.Now.AddDays(1), MultiplexId = 1
            };

            // Act
            // var ex = Assert.ThrowsAsync<CustomException>(() =>  _movieService.AddMovies(movieDTO));
            var ex =  _movieService.AddMovies(movieDTO);

            // Assert
            Assert.AreEqual("Another movie in this multiplex has been scheduled at the same time", ex.Exception.InnerException.Message);
        }

        [Test]
        public async Task WhenCityObject_IsNull_ReturnsBadRequest()
        {
            // Arrange
            CityDTO cityDTO = null;

            // Act
            var result = (ObjectResult)await _adminController.AddCity(cityDTO);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void WhenCityDataValid_IfCityAlreadyAdded_ReturnsBadRequest()
        {
            // Arrange
            CityDTO cityDTO = new CityDTO()
            {
                CityName = "Chennai"
            };

            // Act
            var ex = _movieService.AddCity(cityDTO);

            // Assert
            Assert.AreEqual($"City with the name {cityDTO.CityName} already added.", ex.Exception.InnerException.Message);
        }

    }
}
