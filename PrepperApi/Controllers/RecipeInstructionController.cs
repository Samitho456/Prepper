using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;

namespace PrepperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeInstructionController(IRepositoryDB<RecipeInstruction> recipeInstructionRepo) : ControllerBase
    {
        /// <summary>
        /// Retrieves all recipe instructions, optionally sorted by the specified field and order.
        /// </summary>
        /// <param name="sortBy">The name of the field by which to sort the results. If null or empty, the default sort order is applied.</param>
        /// <param name="ascending">A value indicating whether the results should be sorted in ascending order. Set to <see langword="true"/>
        /// for ascending order; otherwise, <see langword="false"/> for descending order.</param>
        /// <returns>An <see cref="IActionResult"/> containing a collection of recipe instructions with HTTP status code 200 (OK)
        /// if successful; otherwise, 400 (Bad Request) if an error occurs.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] string sortBy, bool ascending)
        {
            try
            {
                var recipeInstructions = await recipeInstructionRepo.GetAllAsync(sortBy, ascending);
                return Ok(recipeInstructions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a recipe instruction by its unique identifier.
        /// </summary>
        /// <remarks>Returns a 200 OK response with the recipe instruction if it exists. If no instruction
        /// with the specified identifier is found, returns a 404 Not Found response.</remarks>
        /// <param name="id">The unique identifier of the recipe instruction to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the recipe instruction if found; otherwise, a 404 Not Found
        /// result.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            // Get the row by the id
            var result = await recipeInstructionRepo.GetByIdAsync(id);

            // If the instruction does not exist return not found
            if (result == null)
            {
                return NotFound($"Instruction with id {id} not found");
            }

            return Ok(result);
        }

        /// <summary>
        /// Creates a new recipe instruction using the provided data.
        /// </summary>
        /// <param name="recipeInstructionDTO">The data transfer object containing the details of the recipe instruction to create. Cannot be null.</param>
        /// <returns>A 201 Created response containing the created recipe instruction if successful; otherwise, a 400 Bad Request
        /// response if the input is invalid.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] RecipeInstructionDTO recipeInstructionDTO)
        {
            // Return 400 Bad Request if the recipeInstructionDTO is null
            if (recipeInstructionDTO == null)
            {
                return BadRequest("RecipeInstructionDTO cannot be null");
            }

            // Map DTO to Model
            var recipeInstruction = new RecipeInstruction
            {
                RecipeId = recipeInstructionDTO.RecipeId,
                StepNumber = recipeInstructionDTO.StepNumber,
                InstructionText = recipeInstructionDTO.InstructionText
            };

            // Create the new recipe instruction
            var createdInstruction = await recipeInstructionRepo.AddAsync(recipeInstruction);

            var createdInstructionDTO = new RecipeInstructionDTO
            {
                Id = createdInstruction.Id,
                RecipeId = createdInstruction.RecipeId,
                StepNumber = createdInstruction.StepNumber,
                InstructionText = createdInstruction.InstructionText,
                CreatedAt = createdInstruction.CreatedAt
            };

            return CreatedAtAction(nameof(Get), new { id = createdInstructionDTO.Id }, createdInstructionDTO);
        }

        /// <summary>
        /// Updates an existing recipe instruction with the specified values.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe instruction to update. Must match the ID in the provided <paramref
        /// name="recipeInstructionDTO"/>.</param>
        /// <param name="recipeInstructionDTO">The data transfer object containing the updated values for the recipe instruction. Cannot be null. The
        /// <c>Id</c> property must match the <paramref name="id"/> parameter.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the update operation. Returns <see
        /// cref="OkObjectResult"/> with the updated recipe instruction if successful; <see
        /// cref="BadRequestObjectResult"/> if the input is invalid; or <see cref="NotFoundObjectResult"/> if the recipe
        /// instruction does not exist.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] RecipeInstructionDTO recipeInstructionDTO)
        {
            // Return 400 Bad Request if the recipeInstructionDTO is null or the id does not match
            if (recipeInstructionDTO == null || id != recipeInstructionDTO.Id)
            {
                return BadRequest("Invalid recipe instruction data.");
            }

            // Check if the recipe instruction exists
            var existingInstruction = await recipeInstructionRepo.GetByIdAsync(id);
            if (existingInstruction == null)
            {
                return NotFound($"Recipe instruction with ID {id} not found.");
            }

            // Update the existing instruction with new values
            existingInstruction.RecipeId = recipeInstructionDTO.RecipeId;
            existingInstruction.StepNumber = recipeInstructionDTO.StepNumber;
            existingInstruction.InstructionText = recipeInstructionDTO.InstructionText;
            var updatedInstruction = await recipeInstructionRepo.UpdateAsync(id, existingInstruction);

            // Map the updated model back to DTO
            var updatedRecipeInstructionDTO = new RecipeInstructionDTO
            {
                Id = updatedInstruction.Id,
                RecipeId = updatedInstruction.RecipeId,
                StepNumber = updatedInstruction.StepNumber,
                InstructionText = updatedInstruction.InstructionText,
                CreatedAt = updatedInstruction.CreatedAt
            };

            return Ok(updatedRecipeInstructionDTO);
        }

        /// <summary>
        /// Deletes the recipe instruction with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe instruction to delete.</param>
        /// <returns>An <see cref="OkObjectResult"/> containing the deleted recipe instruction if the operation is successful;
        /// otherwise, a <see cref="NotFoundObjectResult"/> if the instruction does not exist.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            // Delete the recipe instruction with the specified ID
            var deletedInstruction = await recipeInstructionRepo.DeleteAsync(id);

            // Return 404 Not Found if the instruction does not exist
            if (deletedInstruction == null)
            {
                return NotFound($"Recipe instruction with ID {id} not found.");
            }

            return Ok(deletedInstruction);
        }
    }
}
