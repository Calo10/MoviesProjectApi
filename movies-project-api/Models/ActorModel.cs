using System.Text.Json.Serialization;

namespace YourNamespace.Models
{
    public class ActorModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Biography { get; set; }

        // Navigation property
        [JsonIgnore]
        public List<MovieModel> Movies { get; set; } = new();
    }
} 