using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Prepper.Models
{
    [Table("ingredients")]
    public class Ingredient : BaseModel
    {
        private string _name = string.Empty;
        // unique identifier for the ingredient
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        // timestamp of when the ingredient was created
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        // name of the ingredient
        [Column("name")]
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

        // Parameterized constructor
        public Ingredient(int id, string name, DateTimeOffset createdAt)
        {
            Id = id;
            Name = name;
            CreatedAt = createdAt;
        }

        // Default constructor
        public Ingredient() { }

        // Override ToString for better readability
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Created at {CreatedAt}";
        }
    }
}
