using Microsoft.EntityFrameworkCore;
using JokeApi.Models;
namespace JokeApi.Data
{
    public class JokeDbContext : DbContext
    {
        public DbSet<Joke> Jokes { get; set; }

        public JokeDbContext(DbContextOptions<JokeDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Joke>(entity =>
            {
                entity.ToTable("Jokes");
                entity.Property(e => e.Text).IsRequired();
            });
        }
    }
}