using Microsoft.AspNetCore.Mvc;
using Moq;
using Prepper;
using Prepper.Models;
using PrepperApi.Controllers;


namespace UnitTests;

[TestClass]
public class InventoryItemAPITest
{
    private Mock<IRepositoryDB<InventoryItemDTO>> _mockRepo;
    private InventoryItemsController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IRepositoryDB<InventoryItemDTO>>();
        _controller = new InventoryItemController(_mockRepo.Object);
    }

    [TestMethod]
    public async Task GetAllTest()
    {
        // Arrange
        var InventoryItems = new List<InventoryItemDTO>
        {
            new InventoryItem { Id = 1, IngredientId = 1, Quantity = 2.0f, Unit = "kg", LocationId = 1, UserId = 1 },
            new InventoryItem { Id = 2, IngredientId = 2, Quantity = 1.5f, Unit = "liters", LocationId = 1, UserId = 1 },
            new InventoryItem { Id = 3, RecipeId = 3, Quantity = 1, Unit = "portion", LocationId = 2, UserId = 1 }
        };
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(InventoryItems);

        // Act
        var result = await _controller.GetAll();
        var okResult = result as OkObjectResult;
        var returnedInventoryItems = okResult?.Value as IEnumerable<InventoryItemDTO>;

        // Assert
        Assert.IsNotNull(okResult);
        Assert.IsNotNull(returnedInventoryItems);
        Assert.AreEqual(3, returnedInventoryItems.Count());
    }

    [TestMethod]
    public void GetByIdTest()
    {

    }

    [TestMethod]
    public void CreateTest()
    {
    }

    [TestMethod]
    public void UpdateTest()
    {
    }

    [TestMethod]
    public void DeleteTest()
    {
    }
}
