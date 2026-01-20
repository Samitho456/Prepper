using System;
using System.Collections.Generic;
using System.Text;

namespace Prepper.DTOs
{
    public class IngredientWithNutritionalProfilesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<NutritionalProfileDTO> NutritionalProfiles { get; set; }
    }
}
