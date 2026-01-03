using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;

namespace PrepperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeIngredientController(IRepositoryDB<RecipeIngredients> recipeIngredientRepo) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] string sortBy, bool ascending)
        {
            try
            {
                var recipeIngredients = await recipeIngredientRepo.GetAllAsync(sortBy, ascending);
                var recipeIngredientDTOs = recipeIngredients.Select(ri => new RecipeIngredientDTO
                {
                    Id = ri.Id,
                    RecipeId = ri.RecipeId,
                    IngredientId = ri.IngredientId,
                    Quantity = ri.Quantity,
                    CreatedAt = ri.CreatedAt
                });

                return Ok(recipeIngredientDTOs);
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
            var recipeIngredient = await recipeIngredientRepo.GetByIdAsync(id);
            if (recipeIngredient == null)
            {
                return NotFound($"RecipeIngredient with ID {id} not found.");
            }
            var recipeIngredientDTO = new RecipeIngredientDTO
            {
                Id = recipeIngredient.Id,
                RecipeId = recipeIngredient.RecipeId,
                IngredientId = recipeIngredient.IngredientId,
                Quantity = recipeIngredient.Quantity,
                CreatedAt = recipeIngredient.CreatedAt
            };
            return Ok(recipeIngredientDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] RecipeIngredientDTO recipeIngredientDTO)
        {
            if (recipeIngredientDTO == null)
            {
                return BadRequest("RecipeIngredient data is required.");
            }

            var recipeIngredient = new RecipeIngredients
            {
                RecipeId = recipeIngredientDTO.RecipeId,
                IngredientId = recipeIngredientDTO.IngredientId,
                Quantity = recipeIngredientDTO.Quantity
            };

            var createdRecipeIngredient = await recipeIngredientRepo.AddAsync(recipeIngredient);

            var createdRecipeIngredientResponse = new RecipeIngredientDTO
            {
                Id = createdRecipeIngredient.Id,
                RecipeId = createdRecipeIngredient.RecipeId,
                IngredientId = createdRecipeIngredient.IngredientId,
                Quantity = createdRecipeIngredient.Quantity,
                CreatedAt = createdRecipeIngredient.CreatedAt
            };

            return CreatedAtAction(nameof(Get), new { id = createdRecipeIngredientResponse.Id }, createdRecipeIngredientResponse);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] RecipeIngredientDTO recipeIngredientDTO)
        {
            var recipeIngredient = new RecipeIngredients
            {
                RecipeId = recipeIngredientDTO.RecipeId,
                IngredientId = recipeIngredientDTO.IngredientId,
                Quantity = recipeIngredientDTO.Quantity
            };

            var updatedRecipeIngredient = await recipeIngredientRepo.UpdateAsync(id, recipeIngredient);
            if (updatedRecipeIngredient == null)
            {
                return NotFound($"RecipeIngredient with ID {id} not found.");
            }
            var updatedRecipeIngredientDTO = new RecipeIngredientDTO
            {
                Id = updatedRecipeIngredient.Id,
                RecipeId = updatedRecipeIngredient.RecipeId,
                IngredientId = updatedRecipeIngredient.IngredientId,
                Quantity = updatedRecipeIngredient.Quantity,
                CreatedAt = updatedRecipeIngredient.CreatedAt
            };
            return Ok(updatedRecipeIngredientDTO);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedRecipeIngredient = await recipeIngredientRepo.DeleteAsync(id);
            if (deletedRecipeIngredient == null)
            {
                return NotFound($"RecipeIngredient with ID {id} not found.");
            }
            var deletedRecipeIngredientDTO = new RecipeIngredientDTO
            {
                Id = deletedRecipeIngredient.Id,
                RecipeId = deletedRecipeIngredient.RecipeId,
                IngredientId = deletedRecipeIngredient.IngredientId,
                Quantity = deletedRecipeIngredient.Quantity,
                CreatedAt = deletedRecipeIngredient.CreatedAt
            };
            return Ok(deletedRecipeIngredientDTO);
        }
    }
}
