

using JokeApi.Models;

namespace JokeApi.Data
{
    public class DbInitializer
    {

        private readonly JokeDbContext _context;

        public DbInitializer(JokeDbContext context)
        {
            _context = context;
        }
        public static void SeedData(JokeDbContext context)
        {
            if (!context.Jokes.Any())
            {
                var jokes = new List<Joke>
            {
                new Joke { Text = "Why don't scientists trust atoms? Because they make up everything!", Length = 42 },
                new Joke { Text = "Did you hear about the mathematician who's afraid of negative numbers? He will stop at nothing to avoid them.", Length = 76 },
                // Add more initial jokes here...
            };

                context.Jokes.AddRange(jokes);
                context.SaveChanges();
            }
        }

        public static void Initialize(JokeDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Jokes.Any())
            {
                // Seed the database with initial data
                var fallbackJokes = JokeHelper.RetrieveFallbackJokesFromPropertiesFile();
                context.Jokes.AddRange(fallbackJokes);
                context.SaveChanges();
            }
        }
    }
}