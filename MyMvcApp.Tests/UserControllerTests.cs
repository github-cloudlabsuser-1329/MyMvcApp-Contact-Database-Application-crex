using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using System.Collections.Generic;

namespace MyMvcApp.Tests
{
    public class UserControllerTests
    {
        public UserControllerTests()
        {
            // Reset the static userlist before each test
            UserController.userlist = new List<User>();
        }

        [Fact]
        public void Index_ReturnsViewWithUserList()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test User" });
            var controller = new UserController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<User>>(result.Model);
        }

        [Fact]
        public void Details_UserExists_ReturnsViewWithUser()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test User" };
            UserController.userlist.Add(user);
            var controller = new UserController();

            // Act
            var result = controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user, result.Model);
        }

        [Fact]
        public void Details_UserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_Get_ReturnsView()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Create_Post_ValidUser_RedirectsToIndex()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Id = 1, Name = "Test User" };

            // Act
            var result = controller.Create(user) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Contains(user, UserController.userlist);
        }

        [Fact]
        public void Create_Post_InvalidModel_ReturnsViewWithUser()
        {
            // Arrange
            var controller = new UserController();
            controller.ModelState.AddModelError("Name", "Required");
            var user = new User { Id = 1 };

            // Act
            var result = controller.Create(user) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user, result.Model);
        }

        [Fact]
        public void Edit_Get_UserExists_ReturnsViewWithUser()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test User" };
            UserController.userlist.Add(user);
            var controller = new UserController();

            // Act
            var result = controller.Edit(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user, result.Model);
        }

        [Fact]
        public void Edit_Get_UserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Edit(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_ValidUser_RedirectsToIndex()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Old Name" };
            UserController.userlist.Add(user);
            var controller = new UserController();
            var updatedUser = new User { Id = 1, Name = "New Name" };

            // Act
            var result = controller.Edit(1, updatedUser) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("New Name", user.Name);
        }

        [Fact]
        public void Edit_Post_InvalidModel_ReturnsViewWithUser()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Old Name" };
            UserController.userlist.Add(user);
            var controller = new UserController();
            controller.ModelState.AddModelError("Name", "Required");
            var updatedUser = new User { Id = 1, Name = "New Name" };

            // Act
            var result = controller.Edit(1, updatedUser) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedUser, result.Model);
        }

        [Fact]
        public void Edit_Post_UserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var controller = new UserController();
            var user = new User { Id = 1, Name = "Test User" };

            // Act
            var result = controller.Edit(1, user);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Get_UserExists_ReturnsViewWithUser()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test User" };
            UserController.userlist.Add(user);
            var controller = new UserController();

            // Act
            var result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user, result.Model);
        }

        [Fact]
        public void Delete_Get_UserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Post_UserExists_RedirectsToIndex()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test User" };
            UserController.userlist.Add(user);
            var controller = new UserController();
            var form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());

            // Act
            var result = controller.Delete(1, form) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.DoesNotContain(user, UserController.userlist);
        }

        [Fact]
        public void Delete_Post_UserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var controller = new UserController();
            var form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());

            // Act
            var result = controller.Delete(99, form);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Search_ReturnsEmptyList_WhenQueryIsNullOrWhitespace()
        {
            // Arrange
            var controller = new UserController();

            // Act
            var result = controller.Search("") as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<List<User>>(result.Model);
            Assert.Empty(model);
        }

        [Fact]
        public void Search_ReturnsMatchingUsers_ByNameOrEmail()
        {
            // Arrange
            var user1 = new User { Id = 1, Name = "Alice", Email = "alice@example.com" };
            var user2 = new User { Id = 2, Name = "Bob", Email = "bob@example.com" };
            UserController.userlist.AddRange(new[] { user1, user2 });
            var controller = new UserController();

            // Act
            var result = controller.Search("alice") as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<List<User>>(result.Model);
            Assert.Single(model);
            Assert.Equal(user1, model[0]);

            // Act (search by email)
            result = controller.Search("bob@EXAMPLE.com") as ViewResult;
            model = Assert.IsAssignableFrom<List<User>>(result.Model);
            Assert.Single(model);
            Assert.Equal(user2, model[0]);
        }
    }
}
