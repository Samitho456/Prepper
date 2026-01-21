using Microsoft.AspNetCore.Mvc;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;

namespace PrepperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemsController(IRepositoryDB<InventoryItem> repository) : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(string sortBy = null, bool ascending = true)
        {
            try
            {
                var result = await repository.GetAllAsync(sortBy, ascending);

                var resultList = result.Select(item => new InventoryItemDTO
                {
                    Id = item.Id,
                    CreatedAt = item.CreatedAt,
                    IngredientId = item.IngredientId,
                    RecipeId = item.RecipeId,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    ExpirationDate = item.ExpirationDate,
                    LocationId = item.LocationId,
                    UserId = item.UserId
                }).ToList();

                return Ok(resultList);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await repository.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound($"Inventory Item with ID: {id} not found");
            }

            var itemDto = new InventoryItemDTO
            {
                Id = result.Id,
                CreatedAt = result.CreatedAt,
                IngredientId = result.IngredientId,
                RecipeId = result.RecipeId,
                Quantity = result.Quantity,
                Unit = result.Unit,
                ExpirationDate = result.ExpirationDate,
                LocationId = result.LocationId,
                UserId = result.UserId
            };
            return Ok(itemDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] InventoryItemDTO item)
        {
            if(item == null)
            {
                return BadRequest("Inventory Item data is null");
            }
            if (item.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero");
            }
            if(string.IsNullOrWhiteSpace(item.Unit))
            {
                return BadRequest("Unit must be provided");
            }
            if(item.LocationId <= 0)
            {
                return BadRequest("LocationId must be a positive integer");
            }

            // Map DTO to Model
            var newItem = new InventoryItem
            {
                CreatedAt = item.CreatedAt,
                IngredientId = item.IngredientId,
                RecipeId = item.RecipeId,
                Quantity = item.Quantity,
                Unit = item.Unit,
                ExpirationDate = item.ExpirationDate,
                LocationId = item.LocationId,
                UserId = item.UserId
            };

            // Save to Database
            var createdItem = await repository.AddAsync(newItem);

            // Map Model back to DTO
            var createdItemDto = new InventoryItemDTO
            {
                Id = createdItem.Id,
                CreatedAt = createdItem.CreatedAt,
                IngredientId = createdItem.IngredientId,
                RecipeId = createdItem.RecipeId,
                Quantity = createdItem.Quantity,
                Unit = createdItem.Unit,
                ExpirationDate = createdItem.ExpirationDate,
                LocationId = createdItem.LocationId,
                UserId = createdItem.UserId
            };
            return CreatedAtAction(nameof(Get), new { id = createdItemDto.Id }, createdItemDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] InventoryItemDTO item)
        {
            if(item == null)
            {
                return BadRequest("Inventory Item data is null");
            }
            if (item.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero");
            }
            if (string.IsNullOrWhiteSpace(item.Unit))
            {
                return BadRequest("Unit must be provided");
            }
            if (item.LocationId <= 0)
            {
                return BadRequest("LocationId must be a positive integer");
            }

            // Map DTO to Model
            var updatedItem = new InventoryItem
            {
                Id = id,
                CreatedAt = item.CreatedAt,
                IngredientId = item.IngredientId,
                RecipeId = item.RecipeId,
                Quantity = item.Quantity,
                Unit = item.Unit,
                ExpirationDate = item.ExpirationDate,
                LocationId = item.LocationId,
                UserId = item.UserId
            };

            var result = await repository.UpdateAsync(id, updatedItem);
            if (result == null)
            {
                return NotFound($"Inventory Item with ID: {id} not found");
            }

            return Ok(new InventoryItemDTO
            {
                Id = result.Id,
                CreatedAt = result.CreatedAt,
                IngredientId = result.IngredientId,
                RecipeId = result.RecipeId,
                Quantity = result.Quantity,
                Unit = result.Unit,
                ExpirationDate = result.ExpirationDate,
                LocationId = result.LocationId,
                UserId = result.UserId
            });

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await repository.DeleteAsync(id);
            if (result == null)
            {
                return NotFound($"Inventory Item with ID: {id} not found");
            }
            return Ok(new InventoryItem
            {
                Id = result.Id,
                CreatedAt = result.CreatedAt,
                IngredientId = result.IngredientId,
                RecipeId = result.RecipeId,
                Quantity = result.Quantity,
                Unit = result.Unit,
                ExpirationDate = result.ExpirationDate,
                LocationId = result.LocationId,
                UserId = result.UserId
            });
        }
    }
}
