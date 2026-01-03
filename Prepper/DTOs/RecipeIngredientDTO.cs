using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prepper.DTOs
{
    public class RecipeIngredientDTO
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
