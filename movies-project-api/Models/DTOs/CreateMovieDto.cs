namespace YourNamespace.Models.DTOs
{
    public class CreateMovieDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
} 