using JokeApi.Models;

namespace JokeApi.Services
{
    public class JokeService : IJokeService
    {
        private readonly List<Joke> _jokes;

        public JokeService()
        {
            // Initialize the list of jokes
            _jokes = new List<Joke>
            {
                new Joke { Id = 1, Text = "Why don't scientists trust atoms? Because they make up everything!", Length = 48 },
                new Joke { Id = 2, Text = "How do you organize a space party? You planet!", Length = 40 },
                new Joke { Id = 3, Text = "Why don't skeletons fight each other? They don't have the guts!", Length = 46 },
                // Add more jokes as needed
            };
        }

        public Joke GetRandomJoke()
        {
            Random random = new Random();
            int randomIndex = random.Next(_jokes.Count);
            return _jokes[randomIndex];
        }

        public IEnumerable<Joke> GetJokes()
        {
            return _jokes;
        }

        public IEnumerable<Joke> SearchJokes(string searchTerm)
        {
            return _jokes.Where(joke => joke.Text.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }
}