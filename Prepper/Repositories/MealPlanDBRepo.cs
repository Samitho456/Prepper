using Prepper.DTOs;
using Prepper.Models;
using System.Linq.Expressions;
using static Supabase.Postgrest.Constants;

namespace Prepper.Repositories
{
    public class MealPlanDBRepo : IRepositoryDB<MealPlan>
    {
        private readonly Supabase.Client _supabase;

        private static readonly Dictionary<string, Expression<Func<MealPlan, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "userid", mp => mp.UserId },
                { "recipeid", mp => mp.RecipeId },
                { "mealtype", mp => mp.MealType },
                { "date", mp => mp.Date },
                { "isconsumed", mp => mp.IsConsumed },
                { "createdat", mp => mp.CreatedAt }
            };

        /// <summary>
        /// Initializes a new instance of the MealPlanDBRepo class using the specified Supabase client.
        /// </summary>
        /// <param name="supabase">The Supabase client instance used to interact with the database. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="supabase"/> is null.</exception>
        public MealPlanDBRepo(Supabase.Client supabase)
        {
            _supabase = supabase ?? throw new ArgumentNullException(nameof(supabase));
        }

        /// <summary>
        /// Asynchronously adds a new meal plan to the database.
        /// </summary>
        /// <param name="item">The meal plan to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added meal plan, or null if the
        /// operation did not succeed.</returns>
        public async Task<MealPlan> AddAsync(MealPlan item)
        {
            var result = await _supabase
                .From<MealPlan>()
                .Insert(item);

            return result.Models.FirstOrDefault();
        }

        /// <summary>
        /// Deletes the meal plan with the specified identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the meal plan to delete. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the deleted meal plan if found and
        /// deleted; otherwise, <see langword="null"/> if no meal plan with the specified identifier exists.</returns>
        public async Task<MealPlan?> DeleteAsync(int id)
        {
            var result = await GetByIdAsync(id);

            if (result == null)
            {
                return null;
            }

            await _supabase
                .From<MealPlan>()
                .Where(mp => mp.Id == id)
                .Delete();

            return result;
        }

        /// <summary>
        /// Asynchronously retrieves all meal plans from the data source, optionally sorted by the specified column and
        /// direction.
        /// </summary>
        /// <param name="sortBy">The name of the column to sort the results by. Valid values are "userid", "recipeid", "mealtype", "date", "isconsumed", and
        /// "createdat". If null or empty, results are not sorted.</param>
        /// <param name="ascending">Specifies the sort direction. Set to <see langword="true"/> to sort in ascending order; otherwise, results
        /// are sorted in descending order.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
        /// cref="MealPlan"/> objects.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="sortBy"/> is not null or empty and does not match a valid column name.</exception>
        public async Task<IEnumerable<MealPlan>> GetAllAsync(string sortBy = null, bool ascending = false)
        {
            var query = _supabase.From<MealPlan>()
                .Select("*");

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (!sortColumns.TryGetValue(sortBy, out var sortColumn))
                {
                    throw new ArgumentException($"Invalid sortBy parameters: {sortBy}. " +
                        $"Possible parameters: userid, recipeid, mealtype, date, isconsumed, createdat");
                }

                var direction = ascending ? Ordering.Ascending : Ordering.Descending;

                query = query
                    .Select(mp => new object[] { mp.Id, mp.CreatedAt, mp.IsConsumed, mp.UserId, mp.RecipeId, mp.MealType, mp.Date })
                    .Order(sortColumn, direction);
            }

            var result = await query.Get();

            return result.Models;
        }

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


        private async Task<CompleteRecipeDTO> GetCompleteRecipe(int id)
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


        public async Task<IEnumerable<MealPlanWithRecipeDTO>> GetMealPlansForWeekWithRecipes(DateOnly weekStart)
        {
            DateOnly weekEnd = weekStart.AddDays(7);
            var result = await _supabase
                .From<MealPlan>()
                .Where(mp => mp.Date >= weekStart && mp.Date < weekEnd)
                .Order(mp => mp.Date, Ordering.Ascending)
                .Get();

            var mealPlansWithRecipes = result.Models.Select(mp => new MealPlanWithRecipeDTO
            {
                Id = mp.Id,
                CreatedAt = mp.CreatedAt,
                IsConsumed = mp.IsConsumed,
                UserId = mp.UserId,
                RecipeId = mp.RecipeId,
                MealType = mp.MealType,
                Date = mp.Date,
                Recipe = GetCompleteRecipe(mp.RecipeId).Result
            });
            return mealPlansWithRecipes;
        }


        /// <summary>
        /// Asynchronously retrieves a meal plan by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the meal plan to retrieve. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the meal plan with the specified
        /// identifier, or <see langword="null"/> if no matching meal plan is found.</returns>
        public async Task<MealPlan?> GetByIdAsync(int id)
        {
            var result = await _supabase
                .From<MealPlan>()
                .Where(mp => mp.Id == id)
                .Single();

            return result;
        }

        /// <summary>
        /// Updates an existing meal plan with the specified values asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the meal plan to update.</param>
        /// <param name="item">The meal plan object containing the updated values. All required fields must be set. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated meal plan if the update
        /// is successful; otherwise, <see langword="null"/> if no meal plan with the specified identifier exists.</returns>
        public async Task<MealPlan?> UpdateAsync(int id, MealPlan item)
        {
            var result = await _supabase
                .From<MealPlan>()
                .Where(mp => mp.Id == id)
                .Update(item);

            if (result == null || result.Models.Count == 0)
            {
                return null;
            }

            return result.Models.FirstOrDefault();
        }
    }
}
