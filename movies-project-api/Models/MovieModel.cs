using System.Text.Json.Serialization;

namespace YourNamespace.Models
{
    public class MovieModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        
        // Navigation properties
        public List<ActorModel> Actors { get; set; } = new();
        public List<MovieRatingModel> Ratings { get; set; } = new();
    }
} 