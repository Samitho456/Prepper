using Microsoft.AspNetCore.Mvc;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;

namespace PrepperApi.Controllers
{
    public class LocationController(IRepositoryDB<Location> repository) : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(string sortBy = null, bool ascending = false)
        {
            try
            {
                var result = await repository.GetAllAsync(sortBy, ascending);
                return Ok(result);
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
                return NotFound($"Location with ID: {id} not found");
            }

            var locationDto = new Location
            {
                Id = result.Id,
                Name = result.Name,
                CreatedAt = result.CreatedAt
            };

            return Ok(locationDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] Location location)
        {
            if (location == null)
            {
                return BadRequest("Location cannot be null");
            }
            if (location.Name.Length < 3)
            {
                return BadRequest("Location name must be at least 3 characters long");
            }

            var newLocation = new Location
            {
                Name = location.Name
            };

            var createdLocation = await repository.AddAsync(location);

            var locationDto = new Location
            {
                Id = createdLocation.Id,
                Name = createdLocation.Name,
                CreatedAt = createdLocation.CreatedAt
            };

            return CreatedAtAction(nameof(Get), new { id = createdLocation.Id }, createdLocation);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] Location location)
        {
            if (location == null)
            {
                return BadRequest("Invalid location data");
            }
            if (location.Name.Length < 3)
            {
                return BadRequest("Location name must be at least 3 characters long");
            }

            var updatedLocation = new Location
            {
                Name = location.Name
            };

            var result = await repository.UpdateAsync(id, updatedLocation);
            if (result == null)
            {
                return NotFound($"Location with ID: {id} not found");
            }

            return Ok(new LocationDTO
            {
                Id = result.Id,
                Name = result.Name,
                CreatedAt = result.CreatedAt
            });
        }
    }
}
