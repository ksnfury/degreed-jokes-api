using JokeApi.Models;
using JokeApi.Data;
using JokeApi.Services.Cache;
using Microsoft.EntityFrameworkCore;

namespace JokeApi.Services
{
    public class JokeService : IJokeService
    {

        // this is for testing without any dbcontext
        private readonly List<Joke> _fallbackJokes;

        private readonly JokeDbContext _context;

        private readonly IHighlightingDecorator _highlightingDecorator;

        private readonly IJokeCache _jokeCache;

        private readonly ILogger _logger;


        public JokeService(IServiceProvider serviceProvider, IHighlightingDecorator highlightingDecorator, IJokeCache jokeCache, ILogger<JokeService> logger)
        {

            _logger = logger;

            try
            {
                _context = serviceProvider.GetRequiredService<JokeDbContext>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error resolving JokeDbContext: {ex.Message}");
                _context = null;

            }

            // Initialize the fallback jokes if the database context is not available
            if (_fallbackJokes == null)
            {
                // Fallback jokes to be loaded from the properties file
                _fallbackJokes = JokeHelper.RetrieveFallbackJokesFromPropertiesFile();
            }

            _highlightingDecorator = highlightingDecorator;

            _jokeCache = jokeCache;
        }


        private List<Joke> RetrieveFallbackJokesFromPropertiesFile()
        {

            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            var fallbackJokesFilePath = configuration["FallbackJokesFilePath"];
            return JokeHelper.RetrieveFallbackJokesFromPropertiesFile();
        }

        public Joke GetRandomJoke()
        {
            Joke joke;

            if (_context != null)
            {
                var randomJoke = _context.Jokes.FromSqlRaw("SELECT * FROM Jokes ORDER BY RAND() LIMIT 1").FirstOrDefault();

                return randomJoke;
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

            // first check cache if there is a hit 
            var cachedResults = _jokeCache.Get(searchTerm);

            if (cachedResults != null)
            {
                return cachedResults;
            }

            Dictionary<JokeLengthCategory, List<Joke>> categorizedJokes = GetJokesFromSource(searchTerm, limit);

            // Add the cache miss to the cache
            _jokeCache.Set(searchTerm, categorizedJokes);

            return categorizedJokes;
        }

        private Dictionary<JokeLengthCategory, List<Joke>> GetJokesFromSource(string searchTerm, int limit)
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

                // Apply highlighting to the joke text
                joke.Text = _highlightingDecorator.Decorate(joke.Text, searchTerm);

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