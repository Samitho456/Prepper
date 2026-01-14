using Microsoft.AspNetCore.Mvc;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;

namespace PrepperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeIngredientsController(IRepositoryDB<RecipeIngredient> recipeIngredientRepo) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] string sortBy = "recipeid", [FromQuery] bool ascending = true)
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
                    Unit = ri.Unit,
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
                Unit = recipeIngredient.Unit,
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

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recipeIngredient = new RecipeIngredient
            {
                RecipeId = recipeIngredientDTO.RecipeId,
                IngredientId = recipeIngredientDTO.IngredientId,
                Quantity = recipeIngredientDTO.Quantity,
                Unit = recipeIngredientDTO.Unit
            };

            var createdRecipeIngredient = await recipeIngredientRepo.AddAsync(recipeIngredient);

            var createdRecipeIngredientResponse = new RecipeIngredientDTO
            {
                Id = createdRecipeIngredient.Id,
                RecipeId = createdRecipeIngredient.RecipeId,
                IngredientId = createdRecipeIngredient.IngredientId,
                Quantity = createdRecipeIngredient.Quantity,
                Unit = createdRecipeIngredient.Unit,
                CreatedAt = createdRecipeIngredient.CreatedAt
            };

            return CreatedAtAction(nameof(Get), new { id = createdRecipeIngredientResponse.Id }, createdRecipeIngredientResponse);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] RecipeIngredientDTO recipeIngredientDTO)
        {
            if (recipeIngredientDTO == null)
            {
                return BadRequest("RecipeIngredient data is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recipeIngredient = new RecipeIngredient
            {
                RecipeId = recipeIngredientDTO.RecipeId,
                IngredientId = recipeIngredientDTO.IngredientId,
                Quantity = recipeIngredientDTO.Quantity,
                Unit = recipeIngredientDTO.Unit
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
                Unit = updatedRecipeIngredient.Unit,
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
                Unit = deletedRecipeIngredient.Unit,
                CreatedAt = deletedRecipeIngredient.CreatedAt
            };
            return Ok(deletedRecipeIngredientDTO);
        }
    }
}
