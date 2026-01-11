using Microsoft.AspNetCore.Mvc;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;
using Prepper.Repositories;

namespace PrepperApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class NutritionalProfileController(IRepositoryDB<NutritionalProfile> nutritionalProfileRepo) : Controller
    {
        /// <summary>
        /// Retrieves all nutritional profiles, optionally sorted by the specified field and order.
        /// </summary>
        /// <param name="sortBy">The name of the property to sort the results by. Must correspond to a valid property of the nutritional
        /// profile. If null or empty, the default sort order is applied.</param>
        /// <param name="ascending">A value indicating whether the results should be sorted in ascending order. Set to <see langword="true"/>
        /// for ascending order; otherwise, <see langword="false"/> for descending order.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of nutritional profile data transfer objects with
        /// HTTP status code 200 (OK) if successful, or 400 (Bad Request) if the sort parameter is invalid.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] string sortBy = "createdat", [FromQuery] bool ascending = true)
        {
            try
            {
                // Retrieve all nutritional profiles from the repository
                var nutritionalProfiles = await nutritionalProfileRepo.GetAllAsync(sortBy, ascending);
                var nutritionalProfileDTOs = nutritionalProfiles.Select(np => new NutritionalProfileDTO
                {
                    Id = np.Id,
                    CreatedAt = np.CreatedAt,
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
                    Salt = np.Salt
                });
                return Ok(nutritionalProfileDTOs);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the nutritional profile with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the nutritional profile to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the nutritional profile if found; otherwise, a 404 Not Found
        /// result.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var nutritionalProfile = await nutritionalProfileRepo.GetByIdAsync(id);
            if (nutritionalProfile == null)
            {
                return NotFound();
            }
            var nutritionalProfileDTO = new NutritionalProfileDTO
            {
                Id = nutritionalProfile.Id,
                CreatedAt = nutritionalProfile.CreatedAt,
                IngredientId = nutritionalProfile.IngredientId,
                UnitAmount = nutritionalProfile.UnitAmount,
                BaseUnit = nutritionalProfile.BaseUnit,
                Kcal = nutritionalProfile.Kcal,
                Kj = nutritionalProfile.Kj,
                FatTotal = nutritionalProfile.FatTotal,
                FatSaturated = nutritionalProfile.FatSaturated,
                CarbohydrateTotal = nutritionalProfile.CarbohydrateTotal,
                CarbohydrateSugars = nutritionalProfile.CarbohydrateSugars,
                Fiber = nutritionalProfile.Fiber,
                Protein = nutritionalProfile.Protein,
                Salt = nutritionalProfile.Salt
            };
            return Ok(nutritionalProfileDTO);
        }

        /// <summary>
        /// Creates a new nutritional profile using the provided data and returns the result.
        /// </summary>
        /// <remarks>This action responds with HTTP 201 (Created) when the nutritional profile is
        /// successfully created, or HTTP 400 (Bad Request) if the input data is missing or invalid.</remarks>
        /// <param name="nutritionalProfileDTO">The nutritional profile data to create. Must not be null.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the creation operation. Returns <see
        /// cref="CreatedAtActionResult"/> with the created nutritional profile if successful; otherwise, returns <see
        /// cref="BadRequestObjectResult"/> if the input data is invalid.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] NutritionalProfileDTO nutritionalProfileDTO)
        {
            if (nutritionalProfileDTO == null)
            {
                return BadRequest("Nutritional profile data is required.");
            }

            var nutritionalProfile = new NutritionalProfile
            {
                IngredientId = nutritionalProfileDTO.IngredientId,
                UnitAmount = nutritionalProfileDTO.UnitAmount,
                BaseUnit = nutritionalProfileDTO.BaseUnit,
                Kcal = nutritionalProfileDTO.Kcal,
                Kj = nutritionalProfileDTO.Kj,
                FatTotal = nutritionalProfileDTO.FatTotal,
                FatSaturated = nutritionalProfileDTO.FatSaturated,
                CarbohydrateTotal = nutritionalProfileDTO.CarbohydrateTotal,
                CarbohydrateSugars = nutritionalProfileDTO.CarbohydrateSugars,
                Fiber = nutritionalProfileDTO.Fiber,
                Protein = nutritionalProfileDTO.Protein,
                Salt = nutritionalProfileDTO.Salt
            };

            var CreatedNutritionalProfile = await nutritionalProfileRepo.AddAsync(nutritionalProfile);

            var createdProfileDTO = new NutritionalProfileDTO
            {
                Id = CreatedNutritionalProfile.Id,
                CreatedAt = CreatedNutritionalProfile.CreatedAt,
                IngredientId = CreatedNutritionalProfile.IngredientId,
                UnitAmount = CreatedNutritionalProfile.UnitAmount,
                BaseUnit = CreatedNutritionalProfile.BaseUnit,
                Kcal = CreatedNutritionalProfile.Kcal,
                Kj = CreatedNutritionalProfile.Kj,
                FatTotal = CreatedNutritionalProfile.FatTotal,
                FatSaturated = CreatedNutritionalProfile.FatSaturated,
                CarbohydrateTotal = CreatedNutritionalProfile.CarbohydrateTotal,
                CarbohydrateSugars = CreatedNutritionalProfile.CarbohydrateSugars,
                Fiber = CreatedNutritionalProfile.Fiber,
                Protein = CreatedNutritionalProfile.Protein,
                Salt = CreatedNutritionalProfile.Salt
            };

            return CreatedAtAction(nameof(Get), new { id = createdProfileDTO.Id }, createdProfileDTO);
        }


        /// <summary>
        /// Updates the nutritional profile with the specified identifier using the provided data.
        /// </summary>
        /// <param name="id">The unique identifier of the nutritional profile to update.</param>
        /// <param name="nutritionalProfileDTO">The data transfer object containing the updated nutritional profile information. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation. Returns <see
        /// cref="OkObjectResult"/> with the updated profile if successful, <see cref="BadRequestObjectResult"/> if the
        /// input data is invalid, or <see cref="NotFoundResult"/> if the profile does not exist.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] NutritionalProfileDTO nutritionalProfileDTO)
        {
            if (nutritionalProfileDTO == null)
            {
                return BadRequest("Invalid nutritional profile data.");
            }

            var updatedNutritionalProfile = new NutritionalProfile
            {
                Id = id,
                IngredientId = nutritionalProfileDTO.IngredientId,
                UnitAmount = nutritionalProfileDTO.UnitAmount,
                BaseUnit = nutritionalProfileDTO.BaseUnit,
                Kcal = nutritionalProfileDTO.Kcal,
                Kj = nutritionalProfileDTO.Kj,
                FatTotal = nutritionalProfileDTO.FatTotal,
                FatSaturated = nutritionalProfileDTO.FatSaturated,
                CarbohydrateTotal = nutritionalProfileDTO.CarbohydrateTotal,
                CarbohydrateSugars = nutritionalProfileDTO.CarbohydrateSugars,
                Fiber = nutritionalProfileDTO.Fiber,
                Protein = nutritionalProfileDTO.Protein,
                Salt = nutritionalProfileDTO.Salt
            };
            var result = await nutritionalProfileRepo.UpdateAsync(id, updatedNutritionalProfile);

            if (result == null)
            {
                return NotFound();
            }

            var updatedNutritionalProfileDTO = new NutritionalProfileDTO
            {
                Id = result.Id,
                CreatedAt = result.CreatedAt,
                IngredientId = result.IngredientId,
                UnitAmount = result.UnitAmount,
                BaseUnit = result.BaseUnit,
                Kcal = result.Kcal,
                Kj = result.Kj,
                FatTotal = result.FatTotal,
                FatSaturated = result.FatSaturated,
                CarbohydrateTotal = result.CarbohydrateTotal,
                CarbohydrateSugars = result.CarbohydrateSugars,
                Fiber = result.Fiber,
                Protein = result.Protein,
                Salt = result.Salt
            };

            return Ok(updatedNutritionalProfileDTO);
        }
    }
}
