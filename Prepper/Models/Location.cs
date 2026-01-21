using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Prepper.Models
{
    [Table("locations")]
    public class Location : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        public Location(int id, string name, DateTimeOffset createdAt)
        {
            Id = id;
            Name = name;
            CreatedAt = createdAt;
        }

        public Location() { }

        public override string ToString()
        {
            return $"Location(Id: {Id}, Name: {Name}, CreatedAt: {CreatedAt})";
        }
    }
}
