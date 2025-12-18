namespace Prepper
{
    public class Ingredient
    {
        private string _name;
        private NutritionalProfile _nutritionalProfile;
        public int Id { get; set; }
        public string Name 
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be null or empty.");
                }
                if (value.Length < 2 || value.Length > 100)
                {
                    throw new ArgumentException("Name must be between 2 and 100 characters long.");
                }
                _name = value;
            }
        }
        public UnitEnum.Unit BaseUnit { get; set; }
        public NutritionalProfile NutritionalProfile 
        {
            get { return _nutritionalProfile; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("NutritionalProfile cannot be null.");
                }
                if (value.Kcal < 0 || value.Kj < 0 || value.FatTotal < 0 || value.FatSaturated < 0 ||
                    value.CarbohydrateTotal < 0 || value.CarbohydrateSugars < 0 || value.Protein < 0 || value.Salt < 0)
                {
                    throw new ArgumentException("Nutritional values cannot be negative.");
                }
                _nutritionalProfile = value;
            }
        }

        // Parameterized constructor
        public Ingredient(int id, string name, UnitEnum.Unit baseUnit, NutritionalProfile nutritionalProfile)
        {
            Id = id;
            Name = name;
            BaseUnit = baseUnit;
            NutritionalProfile = nutritionalProfile;
        }

        // Default constructor
        public Ingredient() { }

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, BaseUnit: {BaseUnit}. Nutritional Values: \n" +
                $"Kcal: {NutritionalProfile.Kcal}, Kj: {NutritionalProfile.Kj}, Fat: {NutritionalProfile.FatTotal} \n"
                + $"Saturated Fat: {NutritionalProfile.FatSaturated}, Carbohydrates: {NutritionalProfile.CarbohydrateTotal} \n" +
                $"Carbonhydrates Sugar: {NutritionalProfile.CarbohydrateSugars}, Protein: {NutritionalProfile.Protein}, Salt: {NutritionalProfile.Salt}";
        }
    }
}
