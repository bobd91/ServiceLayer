using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using System.Collections.Generic;
using WebApp.Controllers;
using WebApp.Models;
using WebApp.Services;
using WebApp.Migrations;
using System;
using System.Threading.Tasks;

namespace Tests
{
    public class InboxControllerTest
    {
        private Mock<IInboxService> _inboxService;
        private InboxController _inboxController;

        public InboxControllerTest() {
            _inboxService = new Mock<IInboxService>();
            _inboxController = CreateInboxController(_inboxService.Object);
        }

        [Fact]
        public void GetReturnsAllInboxes()
        {
            // Arrange
            var expected = new List<Inbox>(new Inbox[] {
                new Inbox { Id = 1, Value="Number 1" },
                new Inbox { Id = 2, Value="Number 2" }
                });

            _inboxService.Setup(_ => _.GetAll()).ReturnsAsync(expected);

            // Act
            var result = _inboxController.Get().Result as OkObjectResult;

            // Assert
            // Will be null if result wasn't an OkObjectResult
            Assert.NotNull(result);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void GetIdReturnsInboxWithId()
        {
            // Arrange
            var expected = new Inbox() { Id = 5, Value = "Should be 5" };
            _inboxService.Setup(_ => _.GetById(5)).ReturnsAsync(expected);

            // Act
            var result = _inboxController.Get(5).Result as OkObjectResult;

            //Assert
            // Will be null if result wasn't an OkObjectResult
            Assert.NotNull(result);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public void GetNotFoundWhenNoInboxWithId()
        {
            // Arrange
            _inboxService.Setup(_ => _.GetById(6)).ReturnsAsync((Inbox)null);

            // Act
            var result = _inboxController.Get(6).Result as NotFoundResult;

            // Assert
            // Will be null if result wasn't a NotFoundResult
            Assert.NotNull(result);
        }

        [Fact]
        public void PostCreatesInbox()
        {
            // Arrange
            int id = 1;
            var input = new Inbox() { Value = "New item" };
            var expected = new Inbox { Id = id, Value = "New Item", CreatedAt = DateTime.Now };
            _inboxService.Setup(_ => _.Create(input)).ReturnsAsync(expected);

            //Act
            var result = _inboxController.Post(input).Result as CreatedAtRouteResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("inbox", result.RouteName);
            Assert.Equal(id, result.RouteValues.GetValueOrDefault("id", 0));
            Assert.Equal(expected, result.Value);
        }

        private InboxController CreateInboxController(IInboxService inboxService) {
            var serviceContext = new Mock<IServiceContext>();
            serviceContext.SetupGet(_ => _.Inbox).Returns(inboxService);

            return new InboxController(serviceContext.Object);
        }

    }
}
