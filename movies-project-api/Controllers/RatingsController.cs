using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;
using YourNamespace.Models.DTOs;
using YourNamespace.Data;
using YourNamespace.Attributes;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly MovieDbContext _context;

        public RatingsController(MovieDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieRatingModel>>> GetRatings()
        {
            return await _context.MovieRatings.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieRatingModel>> GetRating(int id)
        {
            var rating = await _context.MovieRatings.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }
            return rating;
        }

        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<MovieRatingModel>>> GetMovieRatings(int movieId)
        {
            return await _context.MovieRatings
                .Where(r => r.MovieId == movieId)
                .ToListAsync();
        }

        [HttpPost]
        [ApiKey]
        public async Task<ActionResult<MovieRatingModel>> CreateRating(CreateRatingDto ratingDto)
        {
            var movie = await _context.Movies.FindAsync(ratingDto.MovieId);
            if (movie == null)
            {
                return NotFound("Movie not found");
            }

            if (ratingDto.Rating < 1 || ratingDto.Rating > 5)
            {
                return BadRequest("Rating must be between 1 and 5");
            }

            var rating = new MovieRatingModel
            {
                Rating = ratingDto.Rating,
                Review = ratingDto.Review,
                MovieId = ratingDto.MovieId,
                CreatedAt = DateTime.UtcNow
            };

            _context.MovieRatings.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRating), new { id = rating.Id }, rating);
        }

        [HttpPut("{id}")]
        [ApiKey]
        public async Task<IActionResult> UpdateRating(int id, CreateRatingDto ratingDto)
        {
            var rating = await _context.MovieRatings.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }

            if (ratingDto.Rating < 1 || ratingDto.Rating > 5)
            {
                return BadRequest("Rating must be between 1 and 5");
            }

            rating.Rating = ratingDto.Rating;
            rating.Review = ratingDto.Review;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ApiKey]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var rating = await _context.MovieRatings.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }

            _context.MovieRatings.Remove(rating);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
} 