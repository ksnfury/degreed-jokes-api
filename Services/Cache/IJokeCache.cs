using JokeApi.Models;

namespace JokeApi.Services.Cache
{
    public interface IJokeCache
    {
        Dictionary<JokeLengthCategory, List<Joke>> Get(string key);
        void Set(string key, Dictionary<JokeLengthCategory, List<Joke>> value);
    }
}