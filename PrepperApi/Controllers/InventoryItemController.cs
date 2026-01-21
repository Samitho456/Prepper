using Microsoft.AspNetCore.Mvc;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;

namespace PrepperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemController(IRepositoryDB<InventoryItem> repository) : Controller
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
        public async Task<IActionResult> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryItemDTO item)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] InventoryItemDTO item)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
