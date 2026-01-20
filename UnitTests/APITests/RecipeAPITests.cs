using Microsoft.AspNetCore.Mvc;
using Moq;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;
using PrepperApi.Controllers;

namespace UnitTests.APITests
{
    [TestClass]
    public class RecipeApiTests
    {
        private Mock<IRepositoryDB<Recipe>> _mockRepo;
        private RecipesController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IRepositoryDB<Recipe>>();
            _controller = new RecipesController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsOkResult_WithListOfRecipes()
        {
            // Arrange
            var recipes = new List<Recipe>
            {
                new Recipe { Id = 1, Title = "Test Recipe 1" },
                new Recipe { Id = 2, Title = "Test Recipe 2" }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(recipes);

            // Act
            var result = await _controller.GetAll(string.Empty, true);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedRecipes = okResult.Value as IEnumerable<RecipeDTO>;
            Assert.IsNotNull(returnedRecipes);
            Assert.AreEqual(2, returnedRecipes.Count());
        }

        [TestMethod]
        public async Task GetAll_ArgumentException_fails()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<bool>())).ThrowsAsync(new ArgumentException("Invalid argument"));
            // Act
            var result = await _controller.GetAll("invalid", true);
            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Invalid argument", badRequestResult.Value);
        }

        [TestMethod]
        public async Task Get_ReturnsOkResult_WithRecipe()
        {
            // Arrange
            var recipe = new Recipe { Id = 1, Title = "Test Recipe" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(recipe);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedRecipe = okResult.Value as RecipeDTO;
            Assert.IsNotNull(returnedRecipe);
            Assert.AreEqual(1, returnedRecipe.Id);
        }

        [TestMethod]
        public async Task Get_ReturnsNotFound_WhenRecipeDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Recipe)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Create_ReturnsCreatedAtActionResult_WithRecipe()
        {
            // Arrange
            var recipeDto = new RecipeDTO { Title = "New Recipe" };
            var recipe = new Recipe { Id = 1, Title = "New Recipe" };
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Recipe>())).ReturnsAsync(recipe);

            // Act
            var result = await _controller.Create(recipeDto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            var returnedRecipe = createdResult.Value as RecipeDTO;
            Assert.IsNotNull(returnedRecipe);
            Assert.AreEqual(1, returnedRecipe.Id);
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
        public async Task Update_ReturnsOkResult_WithUpdatedRecipe()
        {
            // Arrange
            var recipeDto = new RecipeDTO { Title = "Updated Recipe" };
            var existingRecipe = new Recipe { Id = 1, Title = "Old Recipe" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingRecipe);
            _mockRepo.Setup(repo => repo.UpdateAsync(1, It.IsAny<Recipe>())).ReturnsAsync(existingRecipe);

            // Act
            var result = await _controller.Update(1, recipeDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedRecipe = okResult.Value as RecipeDTO;
            Assert.IsNotNull(returnedRecipe);
            Assert.AreEqual("Updated Recipe", returnedRecipe.Title);
        }

        [TestMethod]
        public async Task Update_ReturnsBadRequest_WhenRecipeDTOIsNull()
        {
            // Act
            var result = await _controller.Update(1, null);
            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Update_ReturnsNotFound_WhenRecipeDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Recipe)null);
            // Act
            var result = await _controller.Update(1, new RecipeDTO { Title = "Updated Recipe" });
            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Delete_ReturnsOkResult_WithDeletedRecipe()
        {
            // Arrange
            var recipe = new Recipe { Id = 1, Title = "Test Recipe" };
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(recipe);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedRecipe = okResult.Value as RecipeDTO;
            Assert.IsNotNull(returnedRecipe);
            Assert.AreEqual(1, returnedRecipe.Id);
        }

        [TestMethod]
        public async Task Delete_ReturnsNotFound_WhenRecipeDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync((Recipe)null);
            // Act
            var result = await _controller.Delete(1);
            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}
