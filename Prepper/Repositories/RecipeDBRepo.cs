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


        // fix nutritional profile per serving if has null values
        private async Task<NutritionalProfileDTO> GetNutritionalProfilePerServingAsync(int recipeId)
        {
            // Retrieve all ingredients for the specified recipe
            var recipeIngredients = await _supabase
                .From<RecipeIngredient>()
                .Where(ri => ri.RecipeId == recipeId)
                .Get();

            // If no ingredients found, return null
            if (!recipeIngredients.Models.Any())
            {
                return null;
            }

            // Initialize aggregated nutritional values
            float? totalKcal = 0;
            float? totalKj = 0;
            float? totalFatTotal = 0;
            float? totalFatSaturated = 0;
            float? totalCarbohydrateTotal = 0;
            float? totalCarbohydrateSugars = 0;
            float? totalFiber = 0;
            float? totalProtein = 0;
            float? totalSalt = 0;

            // Track which fields have missing data
            bool hasIncompleteKcal = false;
            bool hasIncompleteKj = false;
            bool hasIncompleteFatTotal = false;
            bool hasIncompleteFatSaturated = false;
            bool hasIncompleteCarbohydrateTotal = false;
            bool hasIncompleteCarbohydrateSugars = false;
            bool hasIncompleteFiber = false;
            bool hasIncompleteProtein = false;
            bool hasIncompleteSalt = false;

            foreach (var recipeIngredient in recipeIngredients.Models)
            {
                var nutritionalProfiles = await _supabase
                    .From<NutritionalProfile>()
                    .Where(np => np.IngredientId == recipeIngredient.IngredientId && np.BaseUnit == recipeIngredient.Unit)
                    .Get();

                if (nutritionalProfiles.Models == null || !nutritionalProfiles.Models.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: No nutritional profile found for ingredient {recipeIngredient.IngredientId} with unit {recipeIngredient.Unit}");
                    // Mark all fields as incomplete when no profile is found
                    hasIncompleteKcal = true;
                    hasIncompleteKj = true;
                    hasIncompleteFatTotal = true;
                    hasIncompleteFatSaturated = true;
                    hasIncompleteCarbohydrateTotal = true;
                    hasIncompleteCarbohydrateSugars = true;
                    hasIncompleteFiber = true;
                    hasIncompleteProtein = true;
                    hasIncompleteSalt = true;
                    continue;
                }

                var nutritionalProfile = nutritionalProfiles.Models.FirstOrDefault();
                
                if (nutritionalProfile == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: Nutritional profile is null for ingredient {recipeIngredient.IngredientId}");
                    // Mark all fields as incomplete when profile is null
                    hasIncompleteKcal = true;
                    hasIncompleteKj = true;
                    hasIncompleteFatTotal = true;
                    hasIncompleteFatSaturated = true;
                    hasIncompleteCarbohydrateTotal = true;
                    hasIncompleteCarbohydrateSugars = true;
                    hasIncompleteFiber = true;
                    hasIncompleteProtein = true;
                    hasIncompleteSalt = true;
                    continue;
                }

                // Calculate nutritional values based on the quantity used
                var scaleFactor = recipeIngredient.Quantity / nutritionalProfile.UnitAmount;

                // Aggregate nutritional values and track missing data
                if (nutritionalProfile.Kcal.HasValue)
                {
                    totalKcal += (float?)(nutritionalProfile.Kcal * scaleFactor);
                }
                else
                {
                    hasIncompleteKcal = true;
                }

                if (nutritionalProfile.Kj.HasValue)
                {
                    totalKj += (float?)(nutritionalProfile.Kj * scaleFactor);
                }
                else
                {
                    hasIncompleteKj = true;
                }

                if (nutritionalProfile.FatTotal.HasValue)
                {
                    totalFatTotal += (float?)(nutritionalProfile.FatTotal * scaleFactor);
                }
                else
                {
                    hasIncompleteFatTotal = true;
                }

                if (nutritionalProfile.FatSaturated.HasValue)
                {
                    totalFatSaturated += (float?)(nutritionalProfile.FatSaturated * scaleFactor);
                }
                else
                {
                    hasIncompleteFatSaturated = true;
                }

                if (nutritionalProfile.CarbohydrateTotal.HasValue)
                {
                    totalCarbohydrateTotal += (float?)(nutritionalProfile.CarbohydrateTotal * scaleFactor);
                }
                else
                {
                    hasIncompleteCarbohydrateTotal = true;
                }

                if (nutritionalProfile.CarbohydrateSugars.HasValue)
                {
                    totalCarbohydrateSugars += (float?)(nutritionalProfile.CarbohydrateSugars * scaleFactor);
                }
                else
                {
                    hasIncompleteCarbohydrateSugars = true;
                }

                if (nutritionalProfile.Fiber.HasValue)
                {
                    totalFiber += (float?)(nutritionalProfile.Fiber * scaleFactor);
                }
                else
                {
                    hasIncompleteFiber = true;
                }

                if (nutritionalProfile.Protein.HasValue)
                {
                    totalProtein += (float?)(nutritionalProfile.Protein * scaleFactor);
                }
                else
                {
                    hasIncompleteProtein = true;
                }

                if (nutritionalProfile.Salt.HasValue)
                {
                    totalSalt += (float?)(nutritionalProfile.Salt * scaleFactor);
                }
                else
                {
                    hasIncompleteSalt = true;
                }
            }

            // Build list of inaccurate fields
            var inaccurateFields = new List<string>();
            if (hasIncompleteKcal) inaccurateFields.Add("Kcal");
            if (hasIncompleteKj) inaccurateFields.Add("Kj");
            if (hasIncompleteFatTotal) inaccurateFields.Add("FatTotal");
            if (hasIncompleteFatSaturated) inaccurateFields.Add("FatSaturated");
            if (hasIncompleteCarbohydrateTotal) inaccurateFields.Add("CarbohydrateTotal");
            if (hasIncompleteCarbohydrateSugars) inaccurateFields.Add("CarbohydrateSugars");
            if (hasIncompleteFiber) inaccurateFields.Add("Fiber");
            if (hasIncompleteProtein) inaccurateFields.Add("Protein");
            if (hasIncompleteSalt) inaccurateFields.Add("Salt");

            // Return aggregated nutritional profile for the recipe
            return new NutritionalProfileDTO
            {
                Kcal = totalKcal == 0 ? null : totalKcal,
                Kj = totalKj == 0 ? null : totalKj,
                FatTotal = totalFatTotal == 0 ? null : totalFatTotal,
                FatSaturated = totalFatSaturated == 0 ? null : totalFatSaturated,
                CarbohydrateTotal = totalCarbohydrateTotal == 0 ? null : totalCarbohydrateTotal,
                CarbohydrateSugars = totalCarbohydrateSugars == 0 ? null : totalCarbohydrateSugars,
                Fiber = totalFiber == 0 ? null : totalFiber,
                Protein = totalProtein == 0 ? null : totalProtein,
                Salt = totalSalt == 0 ? null : totalSalt,
                HasIncompleteData = inaccurateFields.Count > 0,
                InaccurateFields = inaccurateFields
            };
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
                }).ToList(),
                NutritionalProfilePerServing = await GetNutritionalProfilePerServingAsync(recipe.Id)
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
                }).ToList(),
                NutritionalProfilePerServing = await GetNutritionalProfilePerServingAsync(addedRecipe.Id)
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