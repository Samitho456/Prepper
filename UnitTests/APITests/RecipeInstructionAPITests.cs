using Microsoft.AspNetCore.Mvc;
using Moq;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;
using PrepperApi.Controllers;

namespace UnitTests.APITests
{
    [TestClass]
    public class RecipeInstructionApiTests
    {
        private Mock<IRepositoryDB<RecipeInstruction>> _mockRepo;
        private RecipeInstructionController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IRepositoryDB<RecipeInstruction>>();
            _controller = new RecipeInstructionController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsOkResult_WithListOfRecipeInstructions()
        {
            // Arrange
            var instructions = new List<RecipeInstruction>
            {
                new RecipeInstruction { Id = 1, InstructionText = "Step 1" },
                new RecipeInstruction { Id = 2, InstructionText = "Step 2" }
            };
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(instructions);

            // Act
            var result = await _controller.GetAll(string.Empty, true);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedInstructions = okResult.Value as IEnumerable<RecipeInstruction>;
            Assert.IsNotNull(returnedInstructions);
            Assert.AreEqual(2, returnedInstructions.Count());
        }

        [TestMethod]
        public async Task Get_ReturnsOkResult_WithRecipeInstruction()
        {
            // Arrange
            var instruction = new RecipeInstruction { Id = 1, InstructionText = "Test Instruction" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(instruction);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedInstruction = okResult.Value as RecipeInstruction;
            Assert.IsNotNull(returnedInstruction);
            Assert.AreEqual(1, returnedInstruction.Id);
        }

        [TestMethod]
        public async Task Get_ReturnsNotFound_WhenInstructionDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((RecipeInstruction)null);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Create_ReturnsCreatedAtActionResult_WithRecipeInstruction()
        {
            // Arrange
            var instructionDto = new RecipeInstructionDTO { InstructionText = "New Instruction" };
            var instruction = new RecipeInstruction { Id = 1, InstructionText = "New Instruction" };
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<RecipeInstruction>())).ReturnsAsync(instruction);

            // Act
            var result = await _controller.Create(instructionDto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            var returnedInstruction = createdResult.Value as RecipeInstructionDTO;
            Assert.IsNotNull(returnedInstruction);
            Assert.AreEqual(1, returnedInstruction.Id);
        }

        [TestMethod]
        public async Task Update_ReturnsOkResult_WithUpdatedRecipeInstruction()
        {
            // Arrange
            var instructionDto = new RecipeInstructionDTO { Id = 1, InstructionText = "Updated Instruction" };
            var existingInstruction = new RecipeInstruction { Id = 1, InstructionText = "Old Instruction" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingInstruction);
            _mockRepo.Setup(repo => repo.UpdateAsync(1, It.IsAny<RecipeInstruction>())).ReturnsAsync(existingInstruction);

            // Act
            var result = await _controller.Update(1, instructionDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedInstruction = okResult.Value as RecipeInstructionDTO;
            Assert.IsNotNull(returnedInstruction);
            Assert.AreEqual("Updated Instruction", returnedInstruction.InstructionText);
        }

        [TestMethod]
        public async Task Delete_ReturnsOkResult_WithDeletedRecipeInstruction()
        {
            // Arrange
            var instruction = new RecipeInstruction { Id = 1, InstructionText = "Test Instruction" };
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(instruction);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedInstruction = okResult.Value as RecipeInstruction;
            Assert.IsNotNull(returnedInstruction);
            Assert.AreEqual(1, returnedInstruction.Id);
        }
    }
}
