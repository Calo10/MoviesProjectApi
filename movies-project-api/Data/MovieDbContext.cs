using Microsoft.EntityFrameworkCore;
using YourNamespace.Models;

namespace YourNamespace.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options)
            : base(options)
        {
        }

        public DbSet<MovieModel> Movies { get; set; }
        public DbSet<ActorModel> Actors { get; set; }
        public DbSet<MovieRatingModel> MovieRatings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many relationship between Movies and Actors
            modelBuilder.Entity<MovieModel>()
                .HasMany(m => m.Actors)
                .WithMany(a => a.Movies);

            // Configure one-to-many relationship between Movie and Ratings
            modelBuilder.Entity<MovieRatingModel>()
                .HasOne(r => r.Movie)
                .WithMany(m => m.Ratings)
                .HasForeignKey(r => r.MovieId);
        }
    }
} 