using JokeApi.Models;

namespace JokeApi.Services
{
    public interface IJokeService
    {
        Joke GetRandomJoke();
        Dictionary<JokeLengthCategory, List<Joke>> SearchJokes(string searchTerm, int limit = 30);
    }
}