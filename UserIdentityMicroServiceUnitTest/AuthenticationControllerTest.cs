using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UserIdentityMicroService;
using UserIdentityMicroService.Controllers;
using UserIdentityMicroService.DataProvides;
using UserIdentityMicroService.DTO;
using UserIdentityMicroService.Utilities;

namespace UserIdentityMicroServiceUnitTest
{
    public class AuthenticationControllerTest
    {
        private AuthenticationController authController;
        private Mock<IUserService> userServiceMock;
        private UserLoginDTO userLoginDTO;
        private UserDTO userDTO;

        [SetUp]
        public void Setup()
        {
            userServiceMock = new Mock<IUserService>();
            authController = new AuthenticationController(userServiceMock.Object);

            this.userLoginDTO = new UserLoginDTO() { Username = "mahesh", Password = "Pattar" };
        }

        [Test]
        public async Task WhenUserInfo_IsNull_ReturnBadRequest()
        {
            // Arrange
            UserLoginDTO userLoginDTO = null;

            // Act
            var result = (ObjectResult) await authController.AuthenticateUser(userLoginDTO);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task WhenUserInfoIsValid_ReturnBadRequest_IfNotRegistered()
        {
            // Arrange            
            userServiceMock.Setup(a => a.Authenticate(It.IsAny<string>(), It.IsAny<string>())).Returns(() => null);

            // Act
            var result = (ObjectResult) await authController.AuthenticateUser(userLoginDTO);

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task WhenUserInfoIsValid_AuthenticatedUser_IfRegistered()
        {
            // Arrange
            userServiceMock.Setup(a => a.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => new UserDTO 
                {
                    Username = "Mahesh", Password = "Pattar"
                });

            // Act
            var result = (ObjectResult) await authController.AuthenticateUser(userLoginDTO);
            
            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }
    }
}
