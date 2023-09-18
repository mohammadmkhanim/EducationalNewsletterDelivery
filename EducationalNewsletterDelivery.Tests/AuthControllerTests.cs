using AutoMapper;
using Microsoft.Extensions.Configuration;
using EducationalNewsletterDelivery.DataLayer.UnitOfWork;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using EducationalNewsletterDelivery.API.Controllers;
using EducationalNewsletterDelivery.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using EducationalNewsletterDelivery.DataLayer.Entities;
using EducationalNewsletterDelivery.API.Services;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace EducationalNewsletterDelivery.Tests;

[TestClass]
public class AuthControllerTests
{
    [TestMethod]
    public async Task RegisterAsync_UsernameExists_ReturnsBadRequest()
    {
        //Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<AuthController>>();
        var configurationMock = new Mock<IConfiguration>();

        var controller = new AuthController(
            unitOfWorkMock.Object,
            mapperMock.Object,
            loggerMock.Object,
            configurationMock.Object
        );

        var authUserDTO = new AuthUserDTO
        {
            Username = "testUser",
            Password = "13801380"
        };

        unitOfWorkMock.Setup(u => u.UserRepository.ExisUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(true);
        unitOfWorkMock.Setup(u => u.UserRepository.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
        mapperMock.Setup(m => m.Map<User>(authUserDTO)).Returns(new User { Id = 1, Username = authUserDTO.Username, Password = authUserDTO.Password });
        configurationMock.SetupGet(c => c["Jwt:expires"]).Returns("1");
        configurationMock.SetupGet(c => c["Jwt:Key"]).Returns("ThisismySecretKey");

        // Act
        var result = await controller.RegisterAsync(authUserDTO);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.IsInstanceOfType(badRequestResult.Value, typeof(string));
    }

    [TestMethod]
    public async Task RegisterAsync_UserRegistered_ReturnsOk()
    {
        //Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<AuthController>>();
        var configurationMock = new Mock<IConfiguration>();

        var controller = new AuthController(
            unitOfWorkMock.Object,
            mapperMock.Object,
            loggerMock.Object,
            configurationMock.Object
        );

        var authUserDTO = new AuthUserDTO
        {
            Username = "testUser",
            Password = "13801380"
        };

        unitOfWorkMock.Setup(u => u.UserRepository.ExisUserByUsernameAsync(It.IsAny<string>())).ReturnsAsync(false);
        unitOfWorkMock.Setup(u => u.UserRepository.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
        mapperMock.Setup(m => m.Map<User>(authUserDTO)).Returns(new User { Id = 1, Username = authUserDTO.Username, Password = authUserDTO.Password });
        configurationMock.SetupGet(c => c["Jwt:expires"]).Returns("1");
        configurationMock.SetupGet(c => c["Jwt:Key"]).Returns("ThisismySecretKey");

        // Act
        var result = await controller.RegisterAsync(authUserDTO);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.IsInstanceOfType(okResult.Value, typeof(string));
    }

    [TestMethod]
    public async Task LoginAsync_IncorrectUsernameAndPassword_ReturnsBadRequest()
    {
        //Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<AuthController>>();
        var configurationMock = new Mock<IConfiguration>();

        var controller = new AuthController(
            unitOfWorkMock.Object,
            mapperMock.Object,
            loggerMock.Object,
            configurationMock.Object
        );

        var authUserDTO = new AuthUserDTO
        {
            Username = "testUser",
            Password = "13801380"
        };

        unitOfWorkMock.Setup(u => u.UserRepository.GetUserByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(null as User);

        // Act
        var result = await controller.LoginAsync(authUserDTO);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.IsInstanceOfType(badRequestResult.Value, typeof(string));

    }

    [TestMethod]
    public async Task LoginAsync_UserLoggedIn_ReturnsOk()
    {
        //Arrange
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<IMapper>();
        var loggerMock = new Mock<ILogger<AuthController>>();
        var configurationMock = new Mock<IConfiguration>();

        var controller = new AuthController(
            unitOfWorkMock.Object,
            mapperMock.Object,
            loggerMock.Object,
            configurationMock.Object
        );

        var authUserDTO = new AuthUserDTO
        {
            Username = "testUser",
            Password = "13801380"
        };

        var fakeDeliveredNewsletters = new List<DeliveredNewsletter>()
        {
            new DeliveredNewsletter() { Id = 1,NewsletterId = 1,UserId = 1},
            new DeliveredNewsletter() { Id = 1,NewsletterId = 1,UserId = 1,SeenDateTime = DateTime.Now}
        };

        unitOfWorkMock.Setup(u => u.UserRepository.GetUserByUsernameAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new User() { Id = 1, Username = authUserDTO.Username, Password = authUserDTO.Password });
        unitOfWorkMock.Setup(u => u.DeliveredNewsletterRepository.GetAsync(It.IsAny<Expression<Func<DeliveredNewsletter, bool>>>(), null, null, null)).ReturnsAsync(fakeDeliveredNewsletters);
        unitOfWorkMock.Setup(u => u.DeliveredNewsletterRepository.IsDeliveredNewsletterReceivedAsync(It.IsAny<int>())).ReturnsAsync(true);
        unitOfWorkMock.Setup(u => u.DeliveredNewsletterRepository.MarkDeliveredNewsletterAsReceivedAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

        unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

        mapperMock.Setup(m => m.Map<User>(authUserDTO)).Returns(new User { Id = 1, Username = authUserDTO.Username, Password = authUserDTO.Password });
        configurationMock.SetupGet(c => c["Jwt:expires"]).Returns("1");
        configurationMock.SetupGet(c => c["Jwt:Key"]).Returns("ThisismySecretKey");

        // Act
        var result = await controller.LoginAsync(authUserDTO);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.IsInstanceOfType(okResult.Value, typeof(string));
    }
}