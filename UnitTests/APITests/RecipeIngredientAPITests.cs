using Microsoft.AspNetCore.Mvc;
using Moq;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;
using PrepperApi.Controllers;

namespace UnitTests.APITests
{
    [TestClass]
    public class RecipeIngredientAPITests
    {
        private Mock<IRepositoryDB<RecipeIngredients>> _mockRepo;
        private RecipeIngredientController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IRepositoryDB<RecipeIngredients>>();
            _controller = new RecipeIngredientController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsOkResult_WithListOfRecipeIngredients()
        {
            // Arrange
            var recipeIngredients = new List<RecipeIngredients>
            {
                new RecipeIngredients { Id = 1, RecipeId = 1, IngredientId = 1, Quantity = "1", Unit = "cup" },
                new RecipeIngredients { Id = 2, RecipeId = 1, IngredientId = 2, Quantity = "2", Unit = "tbsp" }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(recipeIngredients);

            // Act
            var result = await _controller.GetAll(null, false);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as IEnumerable<RecipeIngredientDTO>;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(2, returnValue.Count());
        }

        [TestMethod]
        public async Task GetAll_ReturnsBadRequest_WhenInvalidSortBy()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync("InvalidField", true)).ThrowsAsync(new ArgumentException("Invalid sort field"));
            // Act
            var result = await _controller.GetAll("InvalidField", true);
            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Invalid sort field", badRequestResult.Value);
        }

        [TestMethod]
        public async Task Get_ReturnsNotFound_WhenRecipeIngredientDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((RecipeIngredients)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Get_ReturnsOkResult_WithRecipeIngredient()
        {
            // Arrange
            var recipeIngredient = new RecipeIngredients { Id = 1, RecipeId = 1, IngredientId = 1, Quantity = "1", Unit = "cup" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(recipeIngredient);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as RecipeIngredientDTO;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(1, returnValue.Id);
        }

        [TestMethod]
        public async Task Create_ReturnsCreatedAtActionResult_WithNewRecipeIngredient()
        {
            // Arrange
            var recipeIngredientDTO = new RecipeIngredientDTO { RecipeId = 1, IngredientId = 1, Quantity = "1", Unit = "cup" };
            var recipeIngredient = new RecipeIngredients { Id = 1, RecipeId = 1, IngredientId = 1, Quantity = "1", Unit = "cup" };
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<RecipeIngredients>())).ReturnsAsync(recipeIngredient);

            // Act
            var result = await _controller.Create(recipeIngredientDTO);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            var returnValue = createdAtActionResult.Value as RecipeIngredientDTO;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(1, returnValue.Id);
        }

        [TestMethod]
        public async Task Create_ReturnsBadRequest_WhenDTOIsNull()
        {
            // Act
            var result = await _controller.Create(null);
            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Update_ReturnsNotFound_WhenRecipeIngredientDoesNotExist()
        {
            // Arrange
            var recipeIngredientDTO = new RecipeIngredientDTO { Id = 1, RecipeId = 1, IngredientId = 1, Quantity = "1.5", Unit = "cups" };
            _mockRepo.Setup(repo => repo.UpdateAsync(1, It.IsAny<RecipeIngredients>())).ReturnsAsync((RecipeIngredients)null);

            // Act
            var result = await _controller.Update(1, recipeIngredientDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Update_ReturnsOkResult_WithUpdatedRecipeIngredient()
        {
            // Arrange
            var recipeIngredientDTO = new RecipeIngredientDTO { Id = 1, RecipeId = 1, IngredientId = 1, Quantity = "1.5", Unit = "cups" };
            var recipeIngredient = new RecipeIngredients { Id = 1, RecipeId = 1, IngredientId = 1, Quantity = "1.5", Unit = "cups" };
            _mockRepo.Setup(repo => repo.UpdateAsync(1, It.IsAny<RecipeIngredients>())).ReturnsAsync(recipeIngredient);

            // Act
            var result = await _controller.Update(1, recipeIngredientDTO);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as RecipeIngredientDTO;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual("1.5", returnValue.Quantity);
        }

        [TestMethod]
        public async Task Delete_ReturnsNotFound_WhenRecipeIngredientDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync((RecipeIngredients)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Delete_ReturnsOkResult_WithDeletedRecipeIngredient()
        {
            // Arrange
            var recipeIngredient = new RecipeIngredients { Id = 1, RecipeId = 1, IngredientId = 1, Quantity = "1", Unit = "cup" };
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(recipeIngredient);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as RecipeIngredientDTO;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(1, returnValue.Id);
        }
    }
}
