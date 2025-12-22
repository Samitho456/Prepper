using System;
using System.Collections.Generic;
using System.Text;

namespace Prepper.DTOs
{
    public class NutritionalProfileDTO
    {
        public int Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public int IngredientId { get; set; }

        public int UnitAmount { get; set; }

        public string BaseUnit { get; set; }

        public float? Kcal { get; set; }

        public float? Kj { get; set; }

        public float? FatTotal { get; set; }

        public float? FatSaturated { get; set; }

        public float? CarbohydrateTotal { get; set; }

        public float? CarbohydrateSugars { get; set; }

        public float? Fiber { get; set; }

        public float? Protein { get; set; }

        public float? Salt { get; set; }
    }
}
