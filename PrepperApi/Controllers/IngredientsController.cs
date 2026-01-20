using Microsoft.AspNetCore.Mvc;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;
using Prepper.Repositories;

namespace PrepperApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class IngredientsController(IRepositoryDB<Ingredient> ingrediantRepo) : Controller
    {
        /// <summary>
        /// Retrieves all ingredients, optionally sorted by the specified property and order.
        /// </summary>
        /// <param name="sortBy">The name of the property to sort the results by. If null or empty, the default sort order is applied.</param>
        /// <param name="ascending">A value indicating whether to sort the results in ascending order. Set to <see langword="true"/> for
        /// ascending order; otherwise, <see langword="false"/> for descending order.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of ingredient data transfer objects (DTOs) with HTTP
        /// status code 200 (OK) if successful, or 400 (Bad Request) if the sort parameter is invalid.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] string sortBy = "name", [FromQuery] bool ascending = true)
        {
            try
            {
                var ingredients = await ingrediantRepo.GetAllAsync(sortBy, ascending);
                var ingredientDTOs = ingredients.Select(i => new IngredientDTO
                {
                    Id = i.Id,
                    Name = i.Name,
                    CreatedAt = i.CreatedAt
                });

                return Ok(ingredientDTOs);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a collection of ingredients along with their associated nutritional profiles.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing a list of ingredients with their nutritional profiles if the
        /// operation is successful. Returns a status code 200 (OK) with the data.</returns>
        [HttpGet("GetNutritionalProfiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetNutritionalProfiles()
        {
            var repo = (IngredientDBRepo)ingrediantRepo;
            var ingredientsWithNutritionalProfiles = await repo.GetAllIngredientsWithNutritionalProfilesAsync();
            return Ok(ingredientsWithNutritionalProfiles);
        }


        /// <summary>
        /// Retrieves the nutritional profile information for the specified ingredient.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient for which to retrieve the nutritional profile. Must be a non-zero
        /// value.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of nutritional profile data for the specified
        /// ingredient with a status code 200 (OK) if found; 400 (Bad Request) if <paramref name="id"/> is zero; or 404
        /// (Not Found) if no nutritional profile exists for the given ingredient.</returns>
        [HttpGet("{id}/GetNutritionalProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNutritionalProfile(int id)
        {
            if (id == 0)
            {
                return BadRequest("Ingredient ID is required.");
            }

            var repo = (IngredientDBRepo)ingrediantRepo;

            var nutritionalProfile = await repo.GetNutritionalProfilesByIdAsync(id);

            if (nutritionalProfile == null || !nutritionalProfile.Any())
            {
                return NotFound($"Nutritional profile for Ingredient ID {id} not found.");
            }

            var nutritionalProfileDTOs = nutritionalProfile.Select(np => new NutritionalProfileDTO
            {
                Id = np.Id,
                IngredientId = np.IngredientId,
                UnitAmount = np.UnitAmount,
                BaseUnit = np.BaseUnit,
                Kcal = np.Kcal,
                Kj = np.Kj,
                FatTotal = np.FatTotal,
                FatSaturated = np.FatSaturated,
                CarbohydrateTotal = np.CarbohydrateTotal,
                CarbohydrateSugars = np.CarbohydrateSugars,
                Fiber = np.Fiber,
                Protein = np.Protein,
                Salt = np.Salt,
                CreatedAt = np.CreatedAt
            });

            return Ok(nutritionalProfileDTOs);
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
            var ingredient = await ingrediantRepo.GetByIdAsync(id);
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
            // Map DTO to Model - don't set CreatedAt, let database handle it
            var ingredient = new Ingredient
            {
                Name = ingredientDTO.Name
                // CreatedAt will be set by the database default
            };

            // Add the new ingredient to the repository
            var createdIngredient = await ingrediantRepo.AddAsync(ingredient);

            // Map Model back to a new object that only contains Id and Name
            var createdIngredientResponse = new
            {
                Id = createdIngredient.Id,
                Name = createdIngredient.Name
            };

            // Return a CreatedAtAction response with the location of the new resource
            return CreatedAtAction(nameof(Get), new { id = createdIngredientResponse.Id }, createdIngredientResponse);
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
            var deletedIngredient = await ingrediantRepo.DeleteAsync(id);
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
            var updatedIngredient = await ingrediantRepo.UpdateAsync(id, ingredient);
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
