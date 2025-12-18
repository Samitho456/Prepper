namespace Prepper
{
    public class NutritionalProfile
    {
        public float Kcal { get; set; }
        public float Kj { get; set; }
        public float FatTotal { get; set; }
        public float FatSaturated { get; set; }
        public float CarbohydrateTotal { get; set; }
        public float CarbohydrateSugars { get; set; }
        public float Protein { get; set; }
        public float Salt { get; set; }

        public NutritionalProfile(float energyKcal, float energyKj, float fatTotal, float fatSaturated, float carbohydrateTotal, float carbohydrateSugars, float protein, float salt)
        {
            Kcal = energyKcal;
            Kj = energyKj;
            FatTotal = fatTotal;
            FatSaturated = fatSaturated;
            CarbohydrateTotal = carbohydrateTotal;
            CarbohydrateSugars = carbohydrateSugars;
            Protein = protein;
            Salt = salt;
        }

        public NutritionalProfile() { }

        public override string ToString()
        {
            return $"Kcal: {Kcal}, Kj: {Kj}, Fat: {FatTotal} \n" +
                $"Saturated Fat: {FatSaturated}, Carbohydrates: {CarbohydrateTotal} \n" +
                $"Carbonhydrates Sugar: {CarbohydrateSugars}, Protein: {Protein}, Salt: {Salt}";
        }
    }
}
