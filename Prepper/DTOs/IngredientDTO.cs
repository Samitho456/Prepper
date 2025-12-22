using System;
using System.Collections.Generic;
using System.Text;

namespace Prepper.DTOs
{
    public class IngredientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
