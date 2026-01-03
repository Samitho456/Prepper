using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;

namespace PrepperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController(IRepositoryDB<Recipe> recipeRepo) : ControllerBase
    {

        /// <summary>
        /// Retrieves all recipes, optionally sorted by the specified field and order.
        /// </summary>
        /// <param name="sortBy">The name of the field to sort the results by. If null or empty, the default sorting is applied.</param>
        /// <param name="ascending">Specifies whether the results should be sorted in ascending order. Set to <see langword="true"/> for
        /// ascending; otherwise, results are sorted in descending order.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of recipes with a status code of 200 (OK) if
        /// successful, or 400 (Bad Request) if the sorting field is invalid.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] string sortBy, bool ascending)
        {
            try
            {
                var recipes = await recipeRepo.GetAllAsync(sortBy, ascending);
                return Ok(recipes);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the details of a recipe with the specified identifier.
        /// </summary>
        /// <remarks>Returns a 200 OK response with the recipe data if a recipe with the specified ID
        /// exists. If no recipe is found, returns a 404 Not Found response with an error message.</remarks>
        /// <param name="id">The unique identifier of the recipe to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing a <see cref="RecipeDTO"/> with the recipe details if found;
        /// otherwise, a 404 Not Found response.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var recipe = await recipeRepo.GetByIdAsync(id);
            if (recipe == null)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }

            var recipeDTO = new RecipeDTO
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Servings = recipe.Servings,
                MealType = recipe.MealType,
                PreparationTimeMinutes = recipe.PreparationTimeMinutes,
                Description = recipe.Description,
            };
            return Ok(recipeDTO);
        }

        /// <summary>
        /// Creates a new recipe using the provided recipe data.
        /// </summary>
        /// <remarks>The response includes the location of the newly created recipe in the Location
        /// header. The request body must contain all required fields for a recipe.</remarks>
        /// <param name="recipeDTO">The recipe data to create. Must not be null.</param>
        /// <returns>A 201 Created response containing the created recipe if successful; otherwise, a 400 Bad Request response if
        /// the input is invalid.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] RecipeDTO recipeDTO)
        {
            // Return 400 Bad Request if the recipeDTO is null
            if (recipeDTO == null)
            {
                return BadRequest("Recipe data is required.");
            }

            // Map RecipeDTO to Recipe model
            var recipe = new Recipe
            {
                Title = recipeDTO.Title,
                Servings = recipeDTO.Servings,
                MealType = recipeDTO.MealType,
                PreparationTimeMinutes = recipeDTO.PreparationTimeMinutes,
                Description = recipeDTO.Description,
            };

            // Create the recipe in the repository
            var createdRecipe = await recipeRepo.AddAsync(recipe);

            // Map the created Recipe model back to RecipeDTO
            var createdRecipeDTO = new RecipeDTO
            {
                Id = createdRecipe.Id,
                Title = createdRecipe.Title,
                Servings = createdRecipe.Servings,
                MealType = createdRecipe.MealType,
                PreparationTimeMinutes = createdRecipe.PreparationTimeMinutes,
                Description = createdRecipe.Description,
            };

            // Return 201 Created with the location of the new recipe
            return CreatedAtAction(nameof(Get), new { id = createdRecipeDTO.Id }, createdRecipeDTO);
        }

        /// <summary>
        /// Updates an existing recipe with the specified identifier using the provided recipe data.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe to update.</param>
        /// <param name="recipeDTO">The updated recipe data. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the update operation. Returns <see
        /// cref="OkObjectResult"/> with the updated recipe data if the update is successful; <see
        /// cref="BadRequestObjectResult"/> if the input data is invalid; or <see cref="NotFoundObjectResult"/> if the
        /// recipe does not exist.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] RecipeDTO recipeDTO)
        {
            // Return 400 Bad Request if the recipeDTO is null
            if (recipeDTO == null)
            {
                return BadRequest("Recipe data is required.");
            }

            // Check if the recipe exists
            var existingRecipe = await recipeRepo.GetByIdAsync(id);
            if (existingRecipe == null)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }

            // Update the existing recipe with new values
            existingRecipe.Title = recipeDTO.Title;
            existingRecipe.Servings = recipeDTO.Servings;
            existingRecipe.MealType = recipeDTO.MealType;
            existingRecipe.PreparationTimeMinutes = recipeDTO.PreparationTimeMinutes;
            existingRecipe.Description = recipeDTO.Description;

            // Update the recipe in the repository and get the updated recipe
            var updatedRecipe = await recipeRepo.UpdateAsync(id, existingRecipe);
            if (updatedRecipe == null)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }

            // Map the updated Recipe model back to RecipeDTO
            var updatedRecipeDTO = new RecipeDTO
            {
                Id = updatedRecipe.Id,
                Title = updatedRecipe.Title,
                Servings = updatedRecipe.Servings,
                MealType = updatedRecipe.MealType,
                PreparationTimeMinutes = updatedRecipe.PreparationTimeMinutes,
                Description = updatedRecipe.Description,
            };
            return Ok(updatedRecipeDTO);
        }

        /// <summary>
        /// Deletes the recipe with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe to delete.</param>
        /// <returns>An <see cref="OkObjectResult"/> containing the deleted recipe if the operation is successful; otherwise, a
        /// <see cref="NotFoundObjectResult"/> if the recipe does not exist.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            // Delete the recipe from the repository
            var deletedRecipe = await recipeRepo.DeleteAsync(id);

            // If the recipe was not found, return 404 Not Found
            if (deletedRecipe == null)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }

            // Map the deleted Recipe model back to RecipeDTO
            var deletedRecipeDTO = new RecipeDTO
            {
                Id = deletedRecipe.Id,
                Title = deletedRecipe.Title,
                Servings = deletedRecipe.Servings,
                MealType = deletedRecipe.MealType,
                PreparationTimeMinutes = deletedRecipe.PreparationTimeMinutes,
                Description = deletedRecipe.Description,
            };

            return Ok(deletedRecipeDTO);
        }
    }
}
