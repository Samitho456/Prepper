using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Prepper.Models
{
    public class NutritionalProfile : BaseModel
    {
        // Id of the nutritional profile
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        // Timestamp of when the ingredient was created
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        // Foreign key to ingrdient
        [Column("ingredientid")]
        public int IngredientId { get; set; }

        // The amount of the unit the nutritional profile refers to (e. g., 100, 34)
        [Column("unit_amount")]
        public int UnitAmount { get; set; }

        // Unit of the ingredient's base measurement (e.g., grams, milliliters)
        [Column("base_unit")]
        public string BaseUnit { get; set; }

        // kcal in nutritional profile
        [Column("kcal")]
        public float? Kcal { get; set; }

        // kj in nutritional profile
        [Column("kj")]
        public float? Kj { get; set; }

        // total fat in nutritional profile
        [Column("fat_total")]
        public float? FatTotal { get; set; }

        // saturated fat in nutritional profile
        [Column("fat_saturated")]
        public float? FatSaturated { get; set; }

        // total carbohydrates in nutritional profile
        [Column("carb_total")]
        public float? CarbohydrateTotal { get; set; }

        // sugars in nutritional profile
        [Column("carb_sugar")]
        public float? CarbohydrateSugars { get; set; }

        // protein in nutritional profile
        [Column("protein")]
        public float? Protein { get; set; }

        // salt in nutritional profile
        [Column("salt")]
        public float? Salt { get; set; }


        // Parameterized constructor
        public NutritionalProfile(int id, DateTimeOffset createdAt, int unit_amount, string base_unit, float energyKcal, float energyKj, float fatTotal, float fatSaturated, float carbohydrateTotal, float carbohydrateSugars, float protein, float salt)
        {
            Id = id;
            CreatedAt = createdAt;
            UnitAmount = unit_amount;
            BaseUnit = base_unit;
            Kcal = energyKcal;
            Kj = energyKj;
            FatTotal = fatTotal;
            FatSaturated = fatSaturated;
            CarbohydrateTotal = carbohydrateTotal;
            CarbohydrateSugars = carbohydrateSugars;
            Protein = protein;
            Salt = salt;
        }

        // Default constructor
        public NutritionalProfile() { }

        // Override ToString for better readability
        public override string ToString()
        {
            return $"Kcal: {Kcal}, Kj: {Kj}, Fat: {FatTotal} \n" +
                $"Saturated Fat: {FatSaturated}, Carbohydrates: {CarbohydrateTotal} \n" +
                $"Carbonhydrates Sugar: {CarbohydrateSugars}, Protein: {Protein}, Salt: {Salt}";
        }
    }
}
