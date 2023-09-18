using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EducationalNewsletterDelivery.API.Controllers;
using EducationalNewsletterDelivery.DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EducationalNewsletterDelivery.Tests
{
    [TestClass]
    public class UsersControllerTests
    {
        [TestMethod]
        public async Task PromoteUserToAdminAsync_UserIdISDoesNotExist_ReturnsBadRequest()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UsersController>>();

            var controller = new UsersController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

            unitOfWorkMock.Setup(u => u.UserRepository.ExistUserByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await controller.PromoteUserToAdminAsync(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsInstanceOfType(badRequestResult.Value, typeof(string));
        }

        [TestMethod]
        public async Task PromoteUserToAdminAsync_PromoteUser_ReturnsOk()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UsersController>>();

            var controller = new UsersController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

            unitOfWorkMock.Setup(u => u.UserRepository.ExistUserByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            unitOfWorkMock.Setup(u => u.UserRepository.PromoteUserToAdminRoleAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.PromoteUserToAdminAsync(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task DemoteUserToUserAsync_UserIdISDoesNotExist_ReturnsBadRequest()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UsersController>>();

            var controller = new UsersController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

            unitOfWorkMock.Setup(u => u.UserRepository.ExistUserByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await controller.DemoteUserToUserAsync(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsInstanceOfType(badRequestResult.Value, typeof(string));
        }

        [TestMethod]
        public async Task DemoteUserToUserAsync_DemoteUser_ReturnsOk()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<UsersController>>();

            var controller = new UsersController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

            unitOfWorkMock.Setup(u => u.UserRepository.ExistUserByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            unitOfWorkMock.Setup(u => u.UserRepository.DemoteUserToUserRoleAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.DemoteUserToUserAsync(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}