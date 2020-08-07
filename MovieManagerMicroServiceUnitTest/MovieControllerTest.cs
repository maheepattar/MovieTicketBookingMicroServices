using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieManagerMicroService.Controllers;
using MovieManagerMicroService.DBEntities;
using MovieManagerMicroService.Repository;
using MovieManagerMicroService.ServiceProvider;
using NUnit.Framework;


namespace MovieManagerMicroServiceUnitTest
{
    public class MovieControllerTest
    {
        // private MovieService _movieService;
        // private Mock<IMovieRepository> _movieRepositoryMock;

        private MovieController _movieController;
        private Mock<IMovieService> _movieServiceMock;

        [SetUp]
        public void Setup()
        {
            InitMovieRepository();
            _movieController = new MovieController(_movieServiceMock.Object);
        }

        private void InitMovieRepository()
        {
            _movieServiceMock = new Mock<IMovieService>();
            _movieServiceMock.Setup(a => a.GetCities())
                .ReturnsAsync(() => new List<City>(new List<City>
                {
                    new City
                    {
                        Id = 1,
                        CityName = "Bengaluru"
                    },
                    new City
                    {
                        Id = 1,
                        CityName = "Bengaluru"
                    }
                }));
        }

        [Test]
        public async Task WhenRepositoryReturnEmtyList_ShouldReturn_NoContent()
        {
            // Arrange
            _movieServiceMock.Setup(a => a.GetCities())
               .ReturnsAsync(() => new List<City>(new List<City> { }));

            // Act
            var result =  (NoContentResult) await _movieController.Cities();

            var a = HttpStatusCode.NoContent;
            // Action
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public async Task WhenRepositoryReturnResult_ShouldReturn_Cities()
        {
            // Act
            var result = (OkObjectResult) await _movieController.Cities();

            // Action
            var cities = (List<City>)result.Value;
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(2, cities.Count);
        }

    }
}