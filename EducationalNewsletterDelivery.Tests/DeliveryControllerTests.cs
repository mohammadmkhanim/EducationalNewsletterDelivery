using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Configuration;
using EducationalNewsletterDelivery.API.Controllers;
using EducationalNewsletterDelivery.API.Models;
using EducationalNewsletterDelivery.DataLayer.Entities;
using EducationalNewsletterDelivery.DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EducationalNewsletterDelivery.Tests
{
    [TestClass]
    public class DeliveryControllerTests
    {
        [TestMethod]
        public async Task SendNewsletterToUsersAsync_NewsletterDoesNotExist_ReturnsBadRequest()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<DeliveryController>>();

            var controller = new DeliveryController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

            SendNewsletterDTO sendNewsletterDTO = new SendNewsletterDTO()
            {
                NewsletterId = 1,
                UserIds = new int[3] { 1, 2, 3 }
            };

            unitOfWorkMock.Setup(u => u.NewsletterRepository.ExistNewsletterByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await controller.SendNewsletterToUsersAsync(sendNewsletterDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsInstanceOfType(badRequestResult.Value, typeof(string));
        }

        [TestMethod]
        public async Task SendNewsletterToUsersAsync_UserDoesNotExist_ReturnsBadRequest()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<DeliveryController>>();

            var controller = new DeliveryController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

            SendNewsletterDTO sendNewsletterDTO = new SendNewsletterDTO()
            {
                NewsletterId = 1,
                UserIds = new int[3] { 1, 2, 3 }
            };

            unitOfWorkMock.Setup(u => u.NewsletterRepository.ExistNewsletterByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            unitOfWorkMock.Setup(u => u.UserRepository.ExistUserByIdAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await controller.SendNewsletterToUsersAsync(sendNewsletterDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsInstanceOfType(badRequestResult.Value, typeof(string));
        }

        [TestMethod]
        public async Task SendNewsletterToUsersAsync_SuccessDelivery_ReturnsOk()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<DeliveryController>>();

            var controller = new DeliveryController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

            SendNewsletterDTO sendNewsletterDTO = new SendNewsletterDTO()
            {
                NewsletterId = 1,
                UserIds = new int[3] { 1, 2, 3 }
            };

            unitOfWorkMock.Setup(u => u.NewsletterRepository.ExistNewsletterByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            unitOfWorkMock.Setup(u => u.UserRepository.ExistUserByIdAsync(It.IsAny<int>())).ReturnsAsync(true);
            unitOfWorkMock.Setup(u => u.DeliveredNewsletterRepository.AddAsync(It.IsAny<DeliveredNewsletter>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.SendNewsletterToUsersAsync(sendNewsletterDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task GetUserNewslettersAsync_GetUserNewsletters_ReturnsOk()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<DeliveryController>>();
            var httpContextMock = new Mock<HttpContext>();
            var claims = new[]
            {
                new Claim("id", "123"),
            };
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            httpContextMock.Setup(c => c.User).Returns(userPrincipal);
            var controller = new DeliveryController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };
            var fakeDeliveredNewsletters = new List<DeliveredNewsletter>()
            {
                new DeliveredNewsletter() { Id = 1, NewsletterId = 1,UserId = 1},
                new DeliveredNewsletter() { Id = 1, NewsletterId = 1,UserId = 1, SeenDateTime = DateTime.Now},
                new DeliveredNewsletter() { Id = 1, NewsletterId = 1,UserId = 1, SeenDateTime = DateTime.Now, ReceivedDateTime = DateTime.Now}
            };
            var fakeNewsletters = new List<Newsletter>()
            {
                new Newsletter() { Id = 1, Title = "1", Content = "1"},
                new Newsletter() { Id = 2, Title = "1",Content = "1" },
                new Newsletter() { Id = 3, Title = "1",Content = "1"}
            };
            var fakeNewsletterDTOs = new List<NewsletterDTO>()
            {
                new NewsletterDTO() { Id = 1, Title = "1", Content = "1"},
                new NewsletterDTO() { Id = 2, Title = "1",Content = "1" },
                new NewsletterDTO() { Id = 3, Title = "1",Content = "1"}
            };
            unitOfWorkMock.Setup(u => u.DeliveredNewsletterRepository.GetAsync(It.IsAny<Expression<Func<DeliveredNewsletter, bool>>>(), null, null, null)).ReturnsAsync(fakeDeliveredNewsletters);
            unitOfWorkMock.Setup(u => u.DeliveredNewsletterRepository.IsDeliveredNewsletterReceivedAsync(It.IsAny<int>())).ReturnsAsync(true);
            unitOfWorkMock.Setup(u => u.DeliveredNewsletterRepository.MarkDeliveredNewsletterAsReceivedAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.DeliveredNewsletterRepository.IsDeliveredNewsletterSeenAsync(It.IsAny<int>())).ReturnsAsync(true);
            unitOfWorkMock.Setup(u => u.DeliveredNewsletterRepository.MarkDeliveredNewsletterAsSeenAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.NewsletterRepository.GetUserNewslettersAsync(It.IsAny<int>())).ReturnsAsync(fakeNewsletters);
            unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            mapperMock.Setup(m => m.Map<List<NewsletterDTO>>(fakeNewsletters)).Returns(fakeNewsletterDTOs);

            // Act
            var result = await controller.GetUserNewslettersAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(fakeNewsletterDTOs, okResult.Value);
        }
    }
}



