using JokeApi.Models;

namespace JokeApi.Services
{
    public interface IJokeService
    {
        Joke GetRandomJoke();
        IEnumerable<Joke> GetJokes();
        IEnumerable<Joke> SearchJokes(string searchTerm);
    }
}