using Microsoft.AspNetCore.Mvc;
using Prepper;
using Prepper.Models;
using Prepper.DTOs;

namespace PrepperApi.Controllers
{
    [Route("api/[Controller]")]
    public class IngredientController : Controller
    {
        // Using the database repository
        private readonly IRepositoryDB<Ingredient> _ingredientsRepo;

        // Constructor injection of the database repository
        public IngredientController(IRepositoryDB<Ingredient> ingrediantRepo)
        {
            _ingredientsRepo = ingrediantRepo;
        }

        /// <summary>
        /// Retrieves all ingredients.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing a collection of all ingredients with a status code of 200 (OK).</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var ingredients = await _ingredientsRepo.GetAllAsync();
            var ingredientDTOs = ingredients.Select(i => new IngredientDTO
            {
                Id = i.Id,
                Name = i.Name,
                CreatedAt = i.CreatedAt
            });

            return Ok(ingredientDTOs);
        }

        /// <summary>
        /// Retrieves the ingredient with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the ingredient data with status code 200 (OK) if found; otherwise,
        /// a 404 (Not Found) result if no ingredient with the specified identifier exists.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var ingredient = await _ingredientsRepo.GetByIdAsync(id);
            if (ingredient == null)
            {
                return NotFound($"Ingredient with ID {id} not found.");
            }
            var ingredientDTO = new IngredientDTO
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                CreatedAt = ingredient.CreatedAt
            };
            return Ok(ingredientDTO);
        }

        /// <summary>
        /// Creates a new ingredient and returns a response with the location of the created resource.
        /// </summary>
        /// <param name="ingredient">The ingredient to add. The ingredient object must not be null and should contain all required fields.</param>
        /// <returns>A 201 Created response containing the newly created ingredient and a Location header with the URI of the new
        /// resource.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] IngredientDTO ingredientDTO)
        {
            if (ingredientDTO == null)
            {
                return BadRequest("Ingredient data is required.");
            }
            // Map DTO to Model
            var ingredient = new Ingredient
            {
                Name = ingredientDTO.Name
            };

            // Add the new ingredient to the repository
            var createdIngredient = await _ingredientsRepo.AddAsync(ingredient);

            // Map Model back to DTO
            var createdIngredientDTO = new IngredientDTO
            {
                Id = createdIngredient.Id,
                Name = createdIngredient.Name,
                CreatedAt = createdIngredient.CreatedAt
            };

            // Return a CreatedAtAction response with the location of the new resource
            return CreatedAtAction(nameof(Get), new { id = createdIngredientDTO.Id }, createdIngredientDTO);
        }

        /// <summary>
        /// Deletes the ingredient with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient to delete.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the delete operation. Returns a 200 OK response
        /// with the deleted ingredient if successful; otherwise, returns a 404 Not Found response if the ingredient
        /// does not exist.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedIngredient = await _ingredientsRepo.DeleteAsync(id);
            if (deletedIngredient == null)
            {
                return NotFound($"Ingredient with ID {id} not found.");
            }
            var deletedIngredientDTO = new IngredientDTO
            {
                Id = deletedIngredient.Id,
                Name = deletedIngredient.Name,
                CreatedAt = deletedIngredient.CreatedAt
            };
            return Ok(deletedIngredientDTO);
        }

        /// <summary>
        /// Updates the ingredient with the specified identifier using the provided ingredient data.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient to update.</param>
        /// <param name="ingredient">The updated ingredient data to apply. The request body must contain a valid ingredient object.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the operation. Returns <see
        /// cref="OkObjectResult"/> with the updated ingredient if the update is successful; otherwise, <see
        /// cref="NotFoundResult"/> if the ingredient does not exist.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] IngredientDTO ingredientDto)
        {
            var ingredient = new Ingredient
            {
                Name = ingredientDto.Name
            };
            var updatedIngredient = await _ingredientsRepo.UpdateAsync(id, ingredient);
            if (updatedIngredient == null)
            {
                return NotFound($"Ingredient with ID {id} not found.");
            }
            var updatedIngredientDTO = new IngredientDTO
            {
                Id = updatedIngredient.Id,
                Name = updatedIngredient.Name,
                CreatedAt = updatedIngredient.CreatedAt
            };
            return Ok(updatedIngredientDTO);
        }
    }
}
