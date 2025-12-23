using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Prepper.DTOs
{
    public class IngredientDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }
}
