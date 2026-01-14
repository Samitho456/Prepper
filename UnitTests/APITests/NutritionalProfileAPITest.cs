using Microsoft.AspNetCore.Mvc;
using Moq;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;
using PrepperApi.Controllers;

namespace UnitTests.APITests;

[TestClass]
public class NutritionalProfileAPITest
{
    private Mock<IRepositoryDB<NutritionalProfile>> _mockRepo;
    private NutritionalProfilesController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IRepositoryDB<NutritionalProfile>>();
        _controller = new NutritionalProfilesController(_mockRepo.Object);
    }


    [TestMethod]
    public async Task GetAll_ReturnsOkResult_WithListOfNutritionalProfilesDTOs()
    {
        // Arrange
        var profiles = new List<NutritionalProfile>
        {
            new NutritionalProfile(1, DateTime.UtcNow, 1, 100, "g", 1.0F, 0.0F, 10F, 1.2F, 1.0F, 1.0F, 2.5F, 2.1F, 3.4F),
            new NutritionalProfile(2, DateTime.UtcNow, 2, 200, "ml", 2.0F, 0.5F, 20F, 2.2F, 2.0F, 2.0F, 3.5F, 3.1F, 4.4F)
        };

        _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<bool>()))
                 .ReturnsAsync(profiles);
        // Act
        var result = await _controller.GetAll(null, false);
        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnValue = okResult.Value as IEnumerable<NutritionalProfileDTO>;
        Assert.IsNotNull(returnValue);
        Assert.AreEqual(2, returnValue.Count());
    }

    [TestMethod]
    public async Task GetAll_ReturnsOkResult_WithEmptyList_WhenNoProfilesExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<bool>()))
                 .ReturnsAsync(new List<NutritionalProfile>());
        // Act
        var result = await _controller.GetAll(null, false);
        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnValue = okResult.Value as IEnumerable<NutritionalProfileDTO>;
        Assert.IsNotNull(returnValue);
        Assert.AreEqual(0, returnValue.Count());
    }

    [TestMethod]
    public async Task GetAll_ArgumentException_fails()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<string>(), It.IsAny<bool>()))
                 .ThrowsAsync(new ArgumentException("Invalid sort property"));
        // Act
        var result = await _controller.GetAll("invalid", false);
        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task Get_ReturnsOkResult_WithNutritionalProfileDTO()
    {
        // Arrange
        var profile = new NutritionalProfile(1, DateTime.UtcNow, 1, 100, "g", 1.0F, 0.0F, 10F, 1.2F, 1.0F, 1.0F, 2.5F, 2.1F, 3.4F);
        _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(profile);

        // Act
        var result = await _controller.Get(1);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnValue = okResult.Value as NutritionalProfileDTO;
        Assert.IsNotNull(returnValue);
        Assert.AreEqual(profile.Id, returnValue.Id);
    }

    [TestMethod]
    public async Task Get_ReturnsNotFound_WhenProfileDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((NutritionalProfile)null);

        // Act
        var result = await _controller.Get(1);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task Create_ReturnsCreatedAtActionResult_WithNutritionalProfileDTO()
    {
        // Arrange
        var profileDTO = new NutritionalProfileDTO
        {
            IngredientId = 1,
            UnitAmount = 100,
            BaseUnit = "g",
            Kcal = 1.0F,
            Kj = 0.0F,
            FatTotal = 10F,
            FatSaturated = 1.2F,
            CarbohydrateTotal = 1.0F,
            CarbohydrateSugars = 1.0F,
            Fiber = 2.5F,
            Protein = 2.1F,
            Salt = 3.4F
        };

        var createdProfile = new NutritionalProfile
        {
            Id = 1,
            CreatedAt = DateTime.UtcNow,
            IngredientId = profileDTO.IngredientId,
            UnitAmount = profileDTO.UnitAmount,
            BaseUnit = profileDTO.BaseUnit,
            Kcal = profileDTO.Kcal,
            Kj = profileDTO.Kj,
            FatTotal = profileDTO.FatTotal,
            FatSaturated = profileDTO.FatSaturated,
            CarbohydrateTotal = profileDTO.CarbohydrateTotal,
            CarbohydrateSugars = profileDTO.CarbohydrateSugars,
            Fiber = profileDTO.Fiber,
            Protein = profileDTO.Protein,
            Salt = profileDTO.Salt
        };

        _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<NutritionalProfile>())).ReturnsAsync(createdProfile);

        // Act
        var result = await _controller.Create(profileDTO);

        // Assert
        var createdAtActionResult = result as CreatedAtActionResult;
        Assert.IsNotNull(createdAtActionResult);
        Assert.AreEqual("Get", createdAtActionResult.ActionName);
        var returnValue = createdAtActionResult.Value as NutritionalProfileDTO;
        Assert.IsNotNull(returnValue);
        Assert.AreEqual(createdProfile.Id, returnValue.Id);
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
    public async Task Update_ReturnsOkResult_WithUpdatedNutritionalProfile()
    {
        // Arrange
        var profileDTO = new NutritionalProfileDTO
        {
            IngredientId = 1,
            UnitAmount = 150,
            BaseUnit = "g"
        };

        var updatedProfile = new NutritionalProfile
        {
            Id = 1,
            IngredientId = profileDTO.IngredientId,
            UnitAmount = profileDTO.UnitAmount,
            BaseUnit = profileDTO.BaseUnit
        };

        _mockRepo.Setup(repo => repo.UpdateAsync(1, It.IsAny<NutritionalProfile>())).ReturnsAsync(updatedProfile);

        // Act
        var result = await _controller.Update(1, profileDTO);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var returnValue = okResult.Value as NutritionalProfile;
        Assert.IsNotNull(returnValue);
        Assert.AreEqual(updatedProfile.UnitAmount, returnValue.UnitAmount);
    }

    [TestMethod]
    public async Task Update_ReturnsNotFound_WhenProfileDoesNotExist()
    {
        // Arrange
        var profileDTO = new NutritionalProfileDTO { IngredientId = 1 };
        _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<NutritionalProfile>())).ReturnsAsync((NutritionalProfile)null);

        // Act
        var result = await _controller.Update(1, profileDTO);

        // Assert
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task Update_ReturnsBadRequest_WhenDTOIsNull()
    {
        // Act
        var result = await _controller.Update(1, null);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }
}
