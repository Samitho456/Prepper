using Prepper.Models;

namespace UnitTests;

[TestClass]
public class InventoryItemModelTest
{
    [TestMethod]
    public void CreateInventoryItem()
    {
        // Arrange
        var id = 1;
        var name = "Test Item";
        var quantity = 5;
        var unit = "kg";
        var createdAt = DateTimeOffset.Now;
        var ingredientId = 10;
        var recipeId = 20;
        var expirationDate = DateTimeOffset.Now.AddDays(10);
        var locationId = 2;
        var userId = 3;
        // Act
        InventoryItemDTO item = new InventoryItem
        {
            Id = id,
            CreatedAt = createdAt,
            IngredientId = ingredientId,
            RecipeId = recipeId,
            Quantity = quantity,
            Unit = unit,
            ExpirationDate = expirationDate,
            LocationId = locationId,
            UserId = userId
        };
        // Assert
        Assert.AreEqual(id, item.Id);
        Assert.AreEqual(createdAt, item.CreatedAt);
        Assert.AreEqual(ingredientId, item.IngredientId);
        Assert.AreEqual(recipeId, item.RecipeId);
        Assert.AreEqual(quantity, item.Quantity);
        Assert.AreEqual(unit, item.Unit);
        Assert.AreEqual(expirationDate, item.ExpirationDate);
        Assert.AreEqual(locationId, item.LocationId);
        Assert.AreEqual(userId, item.UserId);
        Assert.AreEqual(createdAt, item.CreatedAt);
    }

    [TestMethod]
    public void CreateInventoryItem_DefaultConstructor()
    {
        // Act
        InventoryItemDTO item = new InventoryItem();
        // Assert
        Assert.IsNotNull(item);
    }

    [TestMethod]
    public void Correct_ToString_Output()
    {
        // Arrange
        var item = new InventoryItem
        {
            Id = 1,
            CreatedAt = DateTimeOffset.Now,
            IngredientId = 10,
            RecipeId = 20,
            Quantity = 5,
            Unit = "kg",
            ExpirationDate = DateTimeOffset.Now.AddDays(10),
            LocationId = 2,
            UserId = 3
        };
        var expectedString = $"Id: {item.Id}, Created at: {item.CreatedAt}, IngredientId: {item.IngredientId}, RecipeId: {item.RecipeId}, Quantity: {item.Quantity}, Unit: {item.Unit}, ExpirationDate: {item.ExpirationDate}, LocationId: {item.LocationId}, UserId: {item.UserId}";
        // Act
        var toStringOutput = item.ToString();
        // Assert
        Assert.AreEqual(expectedString, toStringOutput);
    }
}
