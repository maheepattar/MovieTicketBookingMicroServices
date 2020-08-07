using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SeatBookingMicroService.Controllers;
using SeatBookingMicroService.DataProviders;
using SeatBookingMicroService.DTO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SeatBookingMicroServiceUnitTest
{
    public class SeatBookingControllerTest
    {
        private SeatBookingController seatBookingController;
        private Mock<ISeatBookingService> seatBookingServiceMock;

        [SetUp]
        public void Setup()
        {
            InitSeatBookingService();
            seatBookingController = new SeatBookingController(seatBookingServiceMock.Object);
        }

        private void InitSeatBookingService()
        {
            seatBookingServiceMock = new Mock<ISeatBookingService>();
        }

        [Test]
        public async Task WhenBookingId_Invalid_Returns_BadRequest()
        {
            // Arrange
            int bookingId = 0;

            // Act
            var result = (ObjectResult) await seatBookingController.BookingDetails(bookingId);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task WhenBookingId_Valid_Returns_BookingDetails()
        {
            // Arrange
            int bookingId = 1;
            seatBookingServiceMock.Setup(a => a.GetBookingDetailsById(It.IsAny<int>()))
                .ReturnsAsync(() => new BookingDTO
                {
                    Amount = 100,
                    BookingDate = DateTime.Now,
                    MovieId = 1
                });

            // Act
            var result = (OkObjectResult)await seatBookingController.BookingDetails(bookingId);

            // Assert
            var returnedDTO = (BookingDTO) result.Value;
            Assert.AreEqual(100, returnedDTO.Amount);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public async Task WhenSeats_MoreThanFive_Returns_BadRequest()
        {
            // Arrange
            BookingDTO bookingDTO = new BookingDTO()
            {
                SeatNo = "1,2,3,4,5,6,7"
            };

            // Act
            var result = (ObjectResult)await seatBookingController.BookMovie(bookingDTO);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNotNull(result.Value);
        }
    }
}
