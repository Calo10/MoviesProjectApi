namespace YourNamespace.Models.DTOs
{
    public class CreateRatingDto
    {
        public int Rating { get; set; }
        public string? Review { get; set; }
        public int MovieId { get; set; }
    }
} 