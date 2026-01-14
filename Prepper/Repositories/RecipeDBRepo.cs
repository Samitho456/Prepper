using Prepper.DTOs;
using Prepper.Models;
using System.Linq.Expressions;
using static Supabase.Postgrest.Constants;

namespace Prepper.Repositories
{
    public class RecipeDBRepo : IRepositoryDB<Recipe>
    {
        // Supabase client instance for database operations
        private readonly Supabase.Client _supabase;

        private static readonly Dictionary<string, Expression<Func<Recipe, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "servings", r => r.Servings  },
                { "meal_type", r => r.MealType },
                { "title", r => r.Title },
                { "createdat", r => r.CreatedAt }
            };

        /// <summary>
        /// Initializes a new instance of the RecipeDBRepo class using the specified Supabase client.
        /// </summary>
        /// <param name="supabase">The Supabase client instance used to interact with the database. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="supabase"/> is null.</exception>
        public RecipeDBRepo(Supabase.Client supabase)
        {
            _supabase = supabase ?? throw new ArgumentNullException(nameof(supabase));
        }

        /// <summary>
        /// Asynchronously adds a new recipe to the database.
        /// </summary>
        /// <param name="item">The recipe to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added recipe, or null if the
        /// operation did not succeed.</returns>
        public async Task<Recipe> AddAsync(Recipe item)
        {
            var result = await _supabase
                .From<Recipe>()
                .Insert(item);

            return result.Models.FirstOrDefault();
        }

        /// <summary>
        /// Deletes the recipe with the specified identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe to delete. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deleted recipe if found and
        /// deleted; otherwise, <see langword="null"/> if no recipe with the specified identifier exists.</returns>
        public async Task<Recipe?> DeleteAsync(int id)
        {
            // Get the original object
            var result = await GetByIdAsync(id);

            // Returns null if no object is found
            if (result == null)
            {
                return null;
            }

            // Delete the row in Supabase
            await _supabase
                .From<Recipe>()
                .Where(r => r.Id == id)
                .Delete();

            return result;
        }

        /// <summary>
        /// Asynchronously retrieves all recipes from the data source, optionally sorted by the specified column and
        /// direction.
        /// </summary>
        /// <param name="sortBy">The name of the column to sort the results by. Valid values are "servings", "meal_type", "title", and
        /// "createdat". If null or empty, results are not sorted.</param>
        /// <param name="ascending">Specifies the sort direction. Set to <see langword="true"/> to sort in ascending order; otherwise, results
        /// are sorted in descending order.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
        /// cref="Recipe"/> objects.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="sortBy"/> is not null or empty and does not match a valid column name.</exception>
        public async Task<IEnumerable<Recipe>> GetAllAsync(string sortBy = null, bool ascending = false)
        {
            var query = _supabase.From<Recipe>()
                .Select("*");

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (!sortColumns.TryGetValue(sortBy, out var sortColumn))
                {
                    throw new ArgumentException($"Invalid sortBy parameters: {sortBy}. " +
                        $"Possible parameters: servings, meal_type, title, createdat");
                }

                var direction = ascending ? Ordering.Ascending : Ordering.Descending;

                query = query
                    .Select(r => new object[] { r.Id, r.Title, r.Description, r.Servings, r.MealType, r.PreparationTimeMinutes, r.CreatedAt })
                    .Order(sortColumn, direction);
            }

            var result = await query
                .Get();

            return result
                .Models;
        }

        /// <summary>
        /// Asynchronously retrieves a recipe by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe to retrieve. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the recipe with the specified
        /// identifier, or <see langword="null"/> if no matching recipe is found.</returns>
        public async Task<Recipe?> GetByIdAsync(int id)
        {
            var result = await _supabase
                .From<Recipe>()
                .Where(r => r.Id == id)
                .Single();

            return result;
        }


        /// <summary>
        /// Retrieves a complete recipe, including its ingredients and instructions, for the specified recipe
        /// identifier.
        /// </summary>
        /// <remarks>The returned recipe includes all associated ingredients and instructions, ordered by
        /// step number. If no recipe exists with the specified identifier, the method returns <see
        /// langword="null"/>.</remarks>
        /// <param name="id">The unique identifier of the recipe to retrieve.</param>
        /// <returns>A <see cref="CompleteRecipeDTO"/> containing the recipe details, ingredients, and instructions if found;
        /// otherwise, <see langword="null"/>.</returns>
        public async Task<CompleteRecipeDTO> GetCompleteRecipe(int id)
        {
            var recipe = await _supabase
                .From<Recipe>()
                .Where(r => r.Id == id)
                .Single();
            
            if (recipe == null)
            {
                return null;
            }

            var recipeIngredients = await _supabase
                .From<RecipeIngredient>()
                .Where(i => i.RecipeId == id)
                .Get();

            var ingredientIds = recipeIngredients.Models
                .Select(ri => ri.IngredientId)
                .ToList();

            var allIngredients = await _supabase
                .From<Ingredient>()
                .Get();

            var ingredients = allIngredients.Models
                .Where(i => ingredientIds.Contains(i.Id))
                .ToList();

            var instructions = await _supabase
                .From<RecipeInstruction>()
                .Where(ri => ri.RecipeId == id)
                .Order(ri => ri.StepNumber, Ordering.Ascending)
                .Get();

            return new CompleteRecipeDTO
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                Servings = recipe.Servings,
                PreparationTimeMinutes = recipe.PreparationTimeMinutes,
                MealType = recipe.MealType,
                CreatedAt = recipe.CreatedAt,
                Ingredients = recipeIngredients.Models.Select(ri =>
                {
                    var ingredient = ingredients.FirstOrDefault(i => i.Id == ri.IngredientId);
                    return new CompleteRecipeIngredientDTO
                    {
                        Id = ingredient.Id,
                        Name = ingredient.Name,
                        RecipeId = ri.RecipeId,
                        IngredientId = ri.IngredientId,
                        Quantity = ri.Quantity,
                        Unit = ri.Unit,
                        CreatedAt = ri.CreatedAt
                    };
                }).ToList(),
                Instructions = instructions.Models.Select(ri => new RecipeInstructionDTO
                {
                    StepNumber = ri.StepNumber,
                    InstructionText = ri.InstructionText,
                    CreatedAt = ri.CreatedAt
                }).ToList()
            };
        }

        public async Task<CompleteRecipeDTO?> AddCompleteRecipe(CompleteRecipeDTO completeRecipeDTO)
        {
            if (completeRecipeDTO == null)
            {
                return null;
            }
            // Add the Recipe
            var recipe = new Recipe
            {
                Title = completeRecipeDTO.Title,
                Description = completeRecipeDTO.Description,
                Servings = completeRecipeDTO.Servings,
                PreparationTimeMinutes = completeRecipeDTO.PreparationTimeMinutes,
                MealType = completeRecipeDTO.MealType
            };
            var addedRecipe = await AddAsync(recipe);
            var ingredientList = new List<RecipeIngredient>();


            // Add the Ingredients
            foreach (var ingredientDTO in completeRecipeDTO.Ingredients)
            {
                if (ingredientDTO.IngredientId == 0)
                {
                    // Ingredient does not exist, create it
                    var newIngredient = new Ingredient
                    {
                        Name = ingredientDTO.Name
                    };
                    var addedIngredient = await _supabase.From<Ingredient>().Insert(newIngredient);
                    ingredientDTO.IngredientId = addedIngredient.Models.First().Id;
                }

                var recipeIngredient = new RecipeIngredient
                {
                    RecipeId = addedRecipe.Id,
                    IngredientId = ingredientDTO.IngredientId,
                    Quantity = ingredientDTO.Quantity,
                    Unit = ingredientDTO.Unit
                };
                var ingredient = await _supabase.From<RecipeIngredient>().Insert(recipeIngredient);
                ingredientList.Add(ingredient.Models.First());
            }

            // Add the Instructions
            var instructionList = new List<RecipeInstruction>();

            foreach (var instructionDTO in completeRecipeDTO.Instructions)
            {
                var recipeInstruction = new RecipeInstruction
                {
                    RecipeId = addedRecipe.Id,
                    StepNumber = instructionDTO.StepNumber,
                    InstructionText = instructionDTO.InstructionText
                };
                var instruction = await _supabase.From<RecipeInstruction>().Insert(recipeInstruction);
                instructionList.Add(instruction.Models.First());
            }

            // Return the complete recipe
            return new CompleteRecipeDTO
            {
                Id = addedRecipe.Id,
                Title = addedRecipe.Title,
                Description = addedRecipe.Description,
                Servings = addedRecipe.Servings,
                PreparationTimeMinutes = addedRecipe.PreparationTimeMinutes,
                MealType = addedRecipe.MealType,
                CreatedAt = addedRecipe.CreatedAt,
                Ingredients = ingredientList.Select(ri => new CompleteRecipeIngredientDTO
                {
                    Id = ri.Id,
                    RecipeId = ri.RecipeId,
                    IngredientId = ri.IngredientId,
                    Quantity = ri.Quantity,
                    Unit = ri.Unit,
                    CreatedAt = ri.CreatedAt
                }).ToList(),
                Instructions = instructionList.Select(ri => new RecipeInstructionDTO
                {
                    StepNumber = ri.StepNumber,
                    InstructionText = ri.InstructionText,
                    CreatedAt = ri.CreatedAt
                }).ToList()
            };
        }

        /// <summary>
        /// Updates an existing recipe with the specified values asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe to update.</param>
        /// <param name="item">The recipe object containing the updated values. All required fields must be set. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated recipe if the update
        /// is successful; otherwise, <see langword="null"/> if no recipe with the specified identifier exists.</returns>
        public async Task<Recipe?> UpdateAsync(int id, Recipe item)
        {
            // Updates the row in Supabase
            var result = await _supabase
                .From<Recipe>()
                .Where(r => r.Id == id)
                .Update(item);

            // Returns null if the item does not exist
            if (result == null || result.Models.Count == 0)
            {
                return null;
            }

            return result.Models.FirstOrDefault();
        }
    }
}