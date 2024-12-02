using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using WEB_API_JWT_AUTH.Controllers;
using WEB_API_JWT_AUTH.DTO;
using WEB_API_JWT_AUTH.Models;

namespace WEB_API_JWT_AUTH_TEST
{
    public class UserControllerTest
    {
        [Fact]
        public void Register_ShouldReturnBadRequest_WhenUserDataIsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<JWTDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using var context = new JWTDBContext(options);

            var configuration = new ConfigurationBuilder().Build();
            var controller = new UsersController(context, configuration);

            // Act
            var result = controller.Register(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User data is null", badRequestResult.Value);
        }

        [Fact]
        public void Register_ShouldAddUserAndReturnOk_WhenDataIsValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<JWTDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using var context = new JWTDBContext(options);

            var configuration = new ConfigurationBuilder().Build();
            var controller = new UsersController(context, configuration);

            var userDto = new UserDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password123"
            };

            // Act
            var result = controller.Register(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User registered successfully", okResult.Value);

            // Verify the user was added to the database
            var user = context.Users.SingleOrDefault(u => u.Email == "john.doe@example.com");
            Assert.NotNull(user);
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
            Assert.Equal("password123", user.Password);
        }

    }
}