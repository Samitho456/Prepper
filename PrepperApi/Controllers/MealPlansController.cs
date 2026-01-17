using Microsoft.AspNetCore.Mvc;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;
using Prepper.Repositories;

namespace PrepperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealPlansController(IRepositoryDB<MealPlan> mealPlanRepo) : ControllerBase
    {
        /// <summary>
        /// Retrieves all meal plans, optionally sorted by the specified field and order.
        /// </summary>
        /// <param name="sortBy">The name of the field to sort the results by. If null or empty, the default sorting is applied.</param>
        /// <param name="ascending">Specifies whether the results should be sorted in ascending order. Set to <see langword="true"/> for
        /// ascending; otherwise, results are sorted in descending order.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of meal plans with a status code of 200 (OK) if
        /// successful, or 400 (Bad Request) if the sorting field is invalid.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] string sortBy = "date", [FromQuery] bool ascending = true)
        {
            try
            {
                var mealPlans = await mealPlanRepo.GetAllAsync(sortBy, ascending);
                var mealPlanDTOs = mealPlans.Select(mp => new MealPlanDTO
                {
                    Id = mp.Id,
                    CreatedAt = mp.CreatedAt,
                    IsConsumed = mp.IsConsumed,
                    UserId = mp.UserId,
                    RecipeId = mp.RecipeId,
                    MealType = mp.MealType,
                    Date = mp.Date
                }).ToList();
                return Ok(mealPlanDTOs);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("week")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMealPlansForWeek([FromQuery] DateOnly weekStart)
        {
            var repo = (MealPlanDBRepo)mealPlanRepo;
            var mealPlansWithRecipes = await repo.GetMealPlansForWeekWithRecipes(weekStart);

            return Ok(mealPlansWithRecipes);
        }

        /// <summary>
        /// Retrieves the details of a meal plan with the specified identifier.
        /// </summary>
        /// <remarks>Returns a 200 OK response with the meal plan data if a meal plan with the specified ID
        /// exists. If no meal plan is found, returns a 404 Not Found response with an error message.</remarks>
        /// <param name="id">The unique identifier of the meal plan to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing a <see cref="MealPlanDTO"/> with the meal plan details if found;
        /// otherwise, a 404 Not Found response.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var mealPlan = await mealPlanRepo.GetByIdAsync(id);
            if (mealPlan == null)
            {
                return NotFound($"Meal plan with ID {id} not found.");
            }

            var mealPlanDTO = new MealPlanDTO
            {
                Id = mealPlan.Id,
                CreatedAt = mealPlan.CreatedAt,
                IsConsumed = mealPlan.IsConsumed,
                UserId = mealPlan.UserId,
                RecipeId = mealPlan.RecipeId,
                MealType = mealPlan.MealType,
                Date = mealPlan.Date
            };
            return Ok(mealPlanDTO);
        }

        /// <summary>
        /// Creates a new meal plan using the provided meal plan data.
        /// </summary>
        /// <remarks>The response includes the location of the newly created meal plan in the Location
        /// header. The request body must contain all required fields for a meal plan.</remarks>
        /// <param name="mealPlanDTO">The meal plan data to create. Must not be null.</param>
        /// <returns>A 201 Created response containing the created meal plan if successful; otherwise, a 400 Bad Request response if
        /// the input is invalid.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] MealPlanDTO mealPlanDTO)
        {
            if (mealPlanDTO == null)
            {
                return BadRequest("Meal plan data is required.");
            }

            var mealPlan = new MealPlan
            {
                IsConsumed = mealPlanDTO.IsConsumed,
                UserId = mealPlanDTO.UserId,
                RecipeId = mealPlanDTO.RecipeId,
                MealType = mealPlanDTO.MealType,
                Date = mealPlanDTO.Date
            };

            var createdMealPlan = await mealPlanRepo.AddAsync(mealPlan);

            var createdMealPlanDTO = new MealPlanDTO
            {
                Id = createdMealPlan.Id,
                CreatedAt = createdMealPlan.CreatedAt,
                IsConsumed = createdMealPlan.IsConsumed,
                UserId = createdMealPlan.UserId,
                RecipeId = createdMealPlan.RecipeId,
                MealType = createdMealPlan.MealType,
                Date = createdMealPlan.Date
            };

            return CreatedAtAction(nameof(Get), new { id = createdMealPlanDTO.Id }, createdMealPlanDTO);
        }

        /// <summary>
        /// Updates an existing meal plan with the specified identifier using the provided meal plan data.
        /// </summary>
        /// <param name="id">The unique identifier of the meal plan to update.</param>
        /// <param name="mealPlanDTO">The updated meal plan data. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the update operation. Returns <see
        /// cref="OkObjectResult"/> with the updated meal plan data if the update is successful; <see
        /// cref="BadRequestObjectResult"/> if the input data is invalid; or <see cref="NotFoundObjectResult"/> if the
        /// meal plan does not exist.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] MealPlanDTO mealPlanDTO)
        {
            if (mealPlanDTO == null)
            {
                return BadRequest("Meal plan data is required.");
            }

            var existingMealPlan = await mealPlanRepo.GetByIdAsync(id);
            if (existingMealPlan == null)
            {
                return NotFound($"Meal plan with ID {id} not found.");
            }

            existingMealPlan.IsConsumed = mealPlanDTO.IsConsumed;
            existingMealPlan.UserId = mealPlanDTO.UserId;
            existingMealPlan.RecipeId = mealPlanDTO.RecipeId;
            existingMealPlan.MealType = mealPlanDTO.MealType;
            existingMealPlan.Date = mealPlanDTO.Date;

            var updatedMealPlan = await mealPlanRepo.UpdateAsync(id, existingMealPlan);

            var updatedMealPlanDTO = new MealPlanDTO
            {
                Id = updatedMealPlan.Id,
                CreatedAt = updatedMealPlan.CreatedAt,
                IsConsumed = updatedMealPlan.IsConsumed,
                UserId = updatedMealPlan.UserId,
                RecipeId = updatedMealPlan.RecipeId,
                MealType = updatedMealPlan.MealType,
                Date = updatedMealPlan.Date
            };
            return Ok(updatedMealPlanDTO);
        }

        /// <summary>
        /// Deletes the meal plan with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the meal plan to delete.</param>
        /// <returns>An <see cref="OkObjectResult"/> containing the deleted meal plan if the operation is successful; otherwise, a
        /// <see cref="NotFoundObjectResult"/> if the meal plan does not exist.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedMealPlan = await mealPlanRepo.DeleteAsync(id);

            if (deletedMealPlan == null)
            {
                return NotFound($"Meal plan with ID {id} not found.");
            }

            var deletedMealPlanDTO = new MealPlanDTO
            {
                Id = deletedMealPlan.Id,
                CreatedAt = deletedMealPlan.CreatedAt,
                IsConsumed = deletedMealPlan.IsConsumed,
                UserId = deletedMealPlan.UserId,
                RecipeId = deletedMealPlan.RecipeId,
                MealType = deletedMealPlan.MealType,
                Date = deletedMealPlan.Date
            };

            return Ok(deletedMealPlanDTO);
        }
    }
}
