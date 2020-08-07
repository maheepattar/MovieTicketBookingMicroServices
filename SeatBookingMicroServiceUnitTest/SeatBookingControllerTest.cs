using Moq;
using NUnit.Framework;
using SeatBookingMicroService.Controllers;
using SeatBookingMicroService.DataProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeatBookingMicroServiceUnitTest
{
    public class SeatBookingControllerTest
    {
        private SeatBookingController seatBookingController;
        private Mock<ISeatBookingRepository> seatBookingRepositoryMock;

        [SetUp]
        public void Setup()
        {
            seatBookingController = new SeatBookingController(seatBookingRepositoryMock.Object);
        }
    }
}
