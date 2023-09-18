using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using AutoMapper;
using EducationalNewsletterDelivery.API.Controllers;
using EducationalNewsletterDelivery.API.Models;
using EducationalNewsletterDelivery.DataLayer.Entities;
using EducationalNewsletterDelivery.DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EducationalNewsletterDelivery.Tests
{
    [TestClass]
    public class NewsletterControllerTests
    {
        [TestMethod]
        public async Task GetAsync_GetNewsletters_ReturnsOk()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<NewsletterController>>();

            var controller = new NewsletterController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

            var fakeNewsletters = new List<Newsletter>()
            {
                new Newsletter() { Id = 2, Title = "1",Content = "1" },
                new Newsletter() { Id = 3, Title = "1",Content = "1"}
            };
            var fakeNewsletterDTOs = fakeNewsletters.Select(f => new NewsletterDTO() { Id = f.Id, Title = f.Title, Content = f.Content }).ToList();
            
            unitOfWorkMock.Setup(u => u.NewsletterRepository.GetAsync(null, null, null, null)).ReturnsAsync(fakeNewsletters);
            mapperMock.Setup(m => m.Map<List<NewsletterDTO>>(fakeNewsletters)).Returns(fakeNewsletterDTOs);

            // Act
            var result = await controller.GetAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(fakeNewsletterDTOs, okResult.Value);
        }

        [TestMethod]
        public async Task GetAsyncById_GetNewsletter_ReturnsOk()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<NewsletterController>>();

            var controller = new NewsletterController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

            var newsletter = new Newsletter() { Id = 1, Title = "1", Content = "1" };
            var newsletterDTO = new NewsletterDTO() { Id = newsletter.Id, Title = newsletter.Title, Content = newsletter.Content };

            unitOfWorkMock.Setup(u => u.NewsletterRepository.GetByIdAsync(1)).ReturnsAsync(newsletter);
            mapperMock.Setup(m => m.Map<NewsletterDTO>(newsletter)).Returns(newsletterDTO);

            // Act
            var result = await controller.GetAsync(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(newsletterDTO, okResult.Value);
        }

        [TestMethod]
        public async Task CreateAsync_CreateUser_ReturnsOk()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<NewsletterController>>();

            var controller = new NewsletterController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

            var createNewsletterDTO = new CreateNewsletterDTO() { Title = "1", Content = "1" };
            var newsletter = new Newsletter() { Id = 1, Title = "1", Content = "1" };
            var fakeNewsletters = new List<Newsletter>()
            {
                newsletter,
                new Newsletter() { Id = 2, Title = "1",Content = "1" },
                new Newsletter() { Id = 3, Title = "1",Content = "1"}
            };
            var fakeNewsletterDTOs = fakeNewsletters.Select(f => new NewsletterDTO() { Id = f.Id, Title = f.Title, Content = f.Content }).ToList();

            mapperMock.Setup(m => m.Map<Newsletter>(createNewsletterDTO)).Returns(newsletter);
            unitOfWorkMock.Setup(u => u.NewsletterRepository.AddAsync(newsletter)).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.NewsletterRepository.GetAsync(null, null, null, null)).ReturnsAsync(fakeNewsletters);
            mapperMock.Setup(m => m.Map<List<NewsletterDTO>>(fakeNewsletters)).Returns(fakeNewsletterDTOs);


            // Act
            var result = await controller.CreateAsync(createNewsletterDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(fakeNewsletterDTOs, okResult.Value);
        }

        [TestMethod]
        public async Task CreateAsync_UpdateUser_ReturnsOk()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<NewsletterController>>();

            var controller = new NewsletterController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

            var newsletterDTO = new NewsletterDTO() { Id = 1, Title = "1", Content = "1" };
            var newsletter = new Newsletter() { Id = 1, Title = "1", Content = "1" };
            var fakeNewsletters = new List<Newsletter>()
            {
                newsletter,
                new Newsletter() { Id = 2, Title = "1",Content = "1" },
                new Newsletter() { Id = 3, Title = "1",Content = "1"}
            };
            var fakeNewsletterDTOs = fakeNewsletters.Select(f => new NewsletterDTO() { Id = f.Id, Title = f.Title, Content = f.Content }).ToList();

            mapperMock.Setup(m => m.Map<Newsletter>(newsletterDTO)).Returns(newsletter);
            unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.NewsletterRepository.GetAsync(null, null, null, null)).ReturnsAsync(fakeNewsletters);
            mapperMock.Setup(m => m.Map<List<NewsletterDTO>>(fakeNewsletters)).Returns(fakeNewsletterDTOs);

            // Act
            var result = await controller.UpdateAsync(newsletterDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(fakeNewsletterDTOs, okResult.Value);
        }

        [TestMethod]
        public async Task DeleteAsync_DeleteUser_ReturnsOk()
        {
            //Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<NewsletterController>>();

            var controller = new NewsletterController(
                unitOfWorkMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

            var fakeNewsletters = new List<Newsletter>()
            {
                new Newsletter() { Id = 2, Title = "1",Content = "1" },
                new Newsletter() { Id = 3, Title = "1",Content = "1"}
            };
            var fakeNewsletterDTOs = fakeNewsletters.Select(f => new NewsletterDTO() { Id = f.Id, Title = f.Title, Content = f.Content }).ToList();

            unitOfWorkMock.Setup(u => u.NewsletterRepository.DeleteAsync(1)).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            mapperMock.Setup(m => m.Map<List<NewsletterDTO>>(fakeNewsletters)).Returns(fakeNewsletterDTOs);
            unitOfWorkMock.Setup(u => u.NewsletterRepository.GetAsync(null, null, null, null)).ReturnsAsync(fakeNewsletters);

            // Act
            var result = await controller.DeleteAsync(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(fakeNewsletterDTOs, okResult.Value);
        }
    }
}