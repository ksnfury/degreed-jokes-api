using JokeApi.Models;
using JokeApi.Data;

namespace JokeApi.Services
{
    public class JokeService : IJokeService
    {

        // this is for testing without any dbcontext
        private readonly List<Joke> _fallbackJokes;

        private readonly JokeDbContext _context;

        public JokeService(IServiceProvider serviceProvider)
        {

            try
            {
                _context = serviceProvider.GetRequiredService<JokeDbContext>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resolving JokeDbContext: {ex.Message}");
                _context = null;
            }
            
            // Initialize the fallback jokes if the database context is not available
            _fallbackJokes = new List<Joke>
            {
                new Joke { Id = 1, Text = "Why don't scientists trust atoms? Because they make up everything!", Length = 44 },
                new Joke { Id = 2, Text = "Did you hear about the mathematician who's afraid of negative numbers? He'll stop at nothing to avoid them!", Length = 78 },
                new Joke { Id = 3, Text = "Why don't skeletons fight each other? They don't have the guts!", Length = 55 },
            };
        }


        public Joke GetRandomJoke()
        {
            Joke joke;

            if (_context != null)
            {
                // Use the database context to fetch a random joke
                joke = _context.Jokes.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            }
            else
            {
                // Fallback to the fallback jokes list if the database context is not available
                var random = new Random();
                var randomIndex = random.Next(_fallbackJokes.Count);
                joke = _fallbackJokes[randomIndex];
            }
            return joke;
        }

        public Dictionary<JokeLengthCategory, List<Joke>> SearchJokes(string searchTerm, int limit)
        {

            var categorizedJokes = new Dictionary<JokeLengthCategory, List<Joke>>();

            var matchingJokes = _context != null
            ? _context.Jokes
                .Where(j => j.Text.Contains(searchTerm))
                .Take(limit)
                .ToList()
            : _fallbackJokes
                .Where(j => j.Text.Contains(searchTerm))
                .Take(limit)
                .ToList();

            foreach (var joke in matchingJokes)
            {
                var category = GetJokeLengthCategory(joke.Length);

                if (!categorizedJokes.ContainsKey(category))
                {
                    categorizedJokes[category] = new List<Joke>();
                }

                categorizedJokes[category].Add(joke);
            }

            return categorizedJokes;
        }

        private JokeLengthCategory GetJokeLengthCategory(int length)
        {
            if (length < 10)
            {
                return JokeLengthCategory.Short;
            }
            else if (length < 20)
            {
                return JokeLengthCategory.Medium;
            }
            else
            {
                return JokeLengthCategory.Long;
            }
        }

    }
}