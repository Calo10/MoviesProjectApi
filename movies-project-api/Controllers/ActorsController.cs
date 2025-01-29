using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;
using YourNamespace.Data;
using YourNamespace.Models.DTOs;
using YourNamespace.Attributes;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly MovieDbContext _context;

        public ActorsController(MovieDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorModel>>> GetActors()
        {
            return await _context.Actors.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActorModel>> GetActor(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return actor;
        }

        [HttpPost]
        [ApiKey]
        public async Task<ActionResult<ActorModel>> CreateActor(CreateActorDto actorDto)
        {
            var actor = new ActorModel
            {
                Name = actorDto.Name,
                DateOfBirth = actorDto.DateOfBirth,
                Biography = actorDto.Biography
            };

            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actor);
        }

        [HttpPut("{id}")]
        [ApiKey]
        public async Task<IActionResult> UpdateActor(int id, ActorModel actor)
        {
            if (id != actor.Id)
            {
                return BadRequest();
            }

            _context.Entry(actor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ApiKey]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{actorId}/movies/{movieId}")]
        [ApiKey]
        public async Task<IActionResult> AssignActorToMovie(int actorId, int movieId)
        {
            var actor = await _context.Actors.FindAsync(actorId);
            var movie = await _context.Movies
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (actor == null || movie == null)
            {
                return NotFound();
            }

            if (movie.Actors.Any(a => a.Id == actorId))
            {
                return BadRequest("Actor is already assigned to this movie");
            }

            movie.Actors.Add(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{actorId}/movies/{movieId}")]
        [ApiKey]
        public async Task<IActionResult> RemoveActorFromMovie(int actorId, int movieId)
        {
            var actor = await _context.Actors.FindAsync(actorId);
            var movie = await _context.Movies.Include(m => m.Actors).FirstOrDefaultAsync(m => m.Id == movieId);

            if (actor == null || movie == null)
            {
                return NotFound();
            }

            if (!movie.Actors.Any(a => a.Id == actorId))
            {
                return BadRequest("Actor is not assigned to this movie");
            }

            movie.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ActorModel>>> SearchActors([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Search term cannot be empty");

            return await _context.Actors
                .Where(a => a.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        [HttpGet("{id}/movies")]
        public async Task<ActionResult<IEnumerable<MovieModel>>> GetActorMovies(int id)
        {
            var actor = await _context.Actors
                .Include(a => a.Movies)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (actor == null)
            {
                return NotFound("Actor not found");
            }

            // Return only the movie details without nested relationships
            return actor.Movies.Select(m => new MovieModel
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                ReleaseDate = m.ReleaseDate
            }).ToList();
        }
    }
} 