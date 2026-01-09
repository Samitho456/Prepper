using Microsoft.AspNetCore.Mvc;
using Moq;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;
using PrepperApi.Controllers;

namespace UnitTests.APITests
{
    [TestClass]
    public class IngredientAPITests
    {
        private Mock<IRepositoryDB<Ingredient>> _mockRepo;
        private IngredientController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IRepositoryDB<Ingredient>>();
            _controller = new IngredientController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsOkResult_WithListOfIngredientDTOs()
        {
            // Arrange
            var ingredients = new List<Ingredient>
            {
                new Ingredient { Id = 1, Name = "Salt", CreatedAt = DateTime.UtcNow },
                new Ingredient { Id = 2, Name = "Pepper", CreatedAt = DateTime.UtcNow }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<bool>()))
                     .ReturnsAsync(ingredients);

            // Act
            var result = await _controller.GetAll(null, false);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as IEnumerable<IngredientDTO>;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(2, returnValue.Count());
        }

        [TestMethod]
        public async Task GetAll_ReturnsBadRequest_OnArgumentException()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<bool>()))
                     .ThrowsAsync(new ArgumentException("Invalid sort property"));

            // Act
            var result = await _controller.GetAll("invalid", false);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Invalid sort property", badRequestResult.Value);
        }

        [TestMethod]
        public async Task Get_ReturnsOkResult_WithIngredientDTO()
        {
            // Arrange
            var ingredient = new Ingredient { Id = 1, Name = "Salt", CreatedAt = DateTime.UtcNow };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(ingredient);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as IngredientDTO;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual("Salt", returnValue.Name);
        }

        [TestMethod]
        public async Task Get_ReturnsNotFound_WhenIngredientDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Ingredient)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Create_ReturnsCreatedAtActionResult_WithIngredient()
        {
            // Arrange
            var ingredientDto = new IngredientDTO { Name = "Sugar" };
            var ingredient = new Ingredient { Id = 1, Name = "Sugar" };
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Ingredient>())).ReturnsAsync(ingredient);

            // Act
            var result = await _controller.Create(ingredientDto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("Get", createdResult.ActionName);
            var returnValue = createdResult.Value as dynamic;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(1, returnValue.GetType().GetProperty("Id").GetValue(returnValue, null));
            Assert.AreEqual("Sugar", returnValue.GetType().GetProperty("Name").GetValue(returnValue, null));
        }

        [TestMethod]
        public async Task Create_ReturnsBadRequest_WhenIngredientIsNull()
        {
            // Act
            var result = await _controller.Create(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Delete_ReturnsOkResult_WithDeletedIngredientDTO()
        {
            // Arrange
            var ingredient = new Ingredient { Id = 1, Name = "Salt", CreatedAt = DateTime.UtcNow };
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(ingredient);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as IngredientDTO;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(1, returnValue.Id);
        }

        [TestMethod]
        public async Task Delete_ReturnsNotFound_WhenIngredientDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync((Ingredient)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Update_ReturnsOkResult_WithUpdatedIngredientDTO()
        {
            // Arrange
            var ingredientDto = new IngredientDTO { Name = "Sea Salt" };
            var updatedIngredient = new Ingredient { Id = 1, Name = "Sea Salt", CreatedAt = DateTime.UtcNow };
            _mockRepo.Setup(repo => repo.UpdateAsync(1, It.IsAny<Ingredient>())).ReturnsAsync(updatedIngredient);

            // Act
            var result = await _controller.Update(1, ingredientDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as IngredientDTO;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual("Sea Salt", returnValue.Name);
        }

        [TestMethod]
        public async Task Update_ReturnsNotFound_WhenIngredientDoesNotExist()
        {
            // Arrange
            var ingredientDto = new IngredientDTO { Name = "Sea Salt" };
            _mockRepo.Setup(repo => repo.UpdateAsync(1, It.IsAny<Ingredient>())).ReturnsAsync((Ingredient)null);

            // Act
            var result = await _controller.Update(1, ingredientDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}
