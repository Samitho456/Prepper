using Microsoft.AspNetCore.Mvc;
using Prepper;

namespace PrepperApi.Controllers
{
    [Route("api/[Controller]")]
    public class IngredientController : Controller
    {
        private IRepository<Ingredient> _ingredientsRepo;
        public IngredientController(IRepository<Ingredient> ingrediantRepo) 
        { 
            _ingredientsRepo = ingrediantRepo;
            _ingredientsRepo.Add(new Ingredient(0, "Ingredient 1", UnitEnum.Unit.Gram, new NutritionalProfile(50, 200, 0.2f, 0.05f, 10f, 2f, 1f, 0.1f)));
            _ingredientsRepo.Add(new Ingredient(0, "Ingredient 2", UnitEnum.Unit.Milliliter, new NutritionalProfile(30, 125, 0.1f, 0.02f, 5f, 1f, 0.5f, 0.05f)));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            return Ok(_ingredientsRepo.GetAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            try
            {
                var ingredient = _ingredientsRepo.GetById(id);
                return Ok(ingredient);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Create([FromBody] Ingredient ingredient)
        {
            var createdIngredient = _ingredientsRepo.Add(ingredient);
            return CreatedAtAction(nameof(Get), new { id = createdIngredient.Id }, createdIngredient);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            try
            {
                var deletedIngredient = _ingredientsRepo.Delete(id);
                return Ok(deletedIngredient);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(int id, [FromBody] Ingredient ingredient)
        {
            try
            {
                var updatedIngredient = _ingredientsRepo.Update(id, ingredient);
                return Ok(updatedIngredient);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
        }
    }
}
