using System.Text.Json.Serialization;

namespace YourNamespace.Models
{
    public class MovieRatingModel
    {
        public int Id { get; set; }
        public int Rating { get; set; }  // e.g., 1-5 stars
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key
        public int MovieId { get; set; }
        // Navigation property
        [JsonIgnore]
        public MovieModel Movie { get; set; } = null!;
    }
} 