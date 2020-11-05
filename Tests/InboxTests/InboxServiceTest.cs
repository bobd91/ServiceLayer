using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using System.Collections.Generic;
using WebApp.Controllers;
using WebApp.Models;
using WebApp.Services;
using WebApp.Migrations;
using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Tests
{
    public class InboxServiceTest : DbTest
    {

        [Fact]
        public async void GetAllReturnsAllInboxes()
        {
            await using (var inbox = new InboxService(MakeInMemoryContext()))
            {
                // Act
                var inboxes = await inbox.GetAll();

                // Assert
                Assert.Collection(inboxes,
                    item => Assert.Equal(1, item.Id),
                    item => Assert.Equal(2, item.Id));
            }
        }

        [Fact]
        public async void GetById2ReturnsSecondInbox()
        {
            await using (var inbox = new InboxService(MakeInMemoryContext()))
            {
                // Act
                var item = await inbox.GetById(2);

                // Assert
                Assert.NotNull(item);
                Assert.Equal(2, item.Id);
                Assert.Equal("Do second", item.Value);
            }
        }

        [Fact]
        public async void GetById4ReturnsNull()
        {
            await using (var inbox = new InboxService(MakeInMemoryContext()))
            {
                // Act
                var item = await inbox.GetById(4);

                // Assert
                Assert.Null(item);
            }
        }

        [Fact]
        public async void CreateAddsInbox3()
        {
            await using (var inbox = new InboxService(MakeInMemoryContext()))
            {
                // Arrange
                var item = new Inbox() { Value = "Do third" };

                // Act
                item = await inbox.Create(item);

                // Assert
                Assert.NotNull(item);
                Assert.Equal(3, item.Id);
                Assert.Equal("Do third", item.Value);
                Assert.True(IsAboutNow(item.CreatedAt));
                Assert.True(IsAboutNow(item.UpdatedAt));
            }
        }

        [Fact]
        public async void CreateIgnoresEverythingExceptValue()
        {
            await using (var inbox = new InboxService(MakeInMemoryContext()))
            {
                // Arrange
                var item = new Inbox()
                {
                    Id = 1,
                    Value = "Do third",
                    CreatedAt = DateTime.Today.AddDays(-2),
                    UpdatedAt = DateTime.Today.AddDays(-1)
                };

                // Act
                item = await inbox.Create(item);

                // Assert
                Assert.NotNull(item);
                Assert.Equal(3, item.Id);
                Assert.Equal("Do third", item.Value);
                Assert.True(IsAboutNow(item.CreatedAt));
                Assert.True(IsAboutNow(item.UpdatedAt));
            }
        }


        private bool IsAboutNow(DateTime dt)
        {
            return 1 > dt.Subtract(DateTime.UtcNow).TotalMinutes;
        }
    }
}
