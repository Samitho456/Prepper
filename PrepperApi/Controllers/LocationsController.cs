using Microsoft.AspNetCore.Mvc;
using Prepper;
using Prepper.DTOs;
using Prepper.Models;
using Sprache;

namespace PrepperApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController(IRepositoryDB<Location> repository) : Controller
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll(string sortBy = null, bool ascending = false)
        {
            try
            {
                var result = await repository.GetAllAsync(sortBy, ascending);

                var locationDtos = result.Select(location => new LocationDTO
                {
                    Id = location.Id,
                    Name = location.Name,
                    CreatedAt = location.CreatedAt ?? DateTimeOffset.MinValue
                    }).ToList();

                return Ok(locationDtos);
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

            var locationDto = new LocationDTO
            {
                Id = result.Id,
                Name = result.Name,
                CreatedAt = result.CreatedAt ?? DateTimeOffset.MinValue
            };

            return Ok(locationDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] LocationDTO location)
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

            var createdLocation = await repository.AddAsync(newLocation);

            return CreatedAtAction(nameof(Get), new { id = createdLocation.Id }, new LocationDTO
            {
                Id = createdLocation.Id,
                Name = createdLocation.Name,
                CreatedAt = createdLocation.CreatedAt ?? DateTimeOffset.MinValue
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] LocationDTO location)
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
                CreatedAt = result.CreatedAt ?? DateTimeOffset.MinValue
            });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await repository.DeleteAsync(id);
            if (result == null)
            {
                return NotFound($"Location with ID: {id} not found");
            }

            return Ok(new LocationDTO
            {
                Id = result.Id,
                Name = result.Name,
                CreatedAt = result.CreatedAt ?? DateTimeOffset.MinValue
            });
        }
    }
}
