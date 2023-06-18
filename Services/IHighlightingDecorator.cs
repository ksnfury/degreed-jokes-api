using JokeApi.Models;

namespace JokeApi.Services
{
    public interface IHighlightingDecorator
    {
        string Decorate(string text, string searchTerm);
    }
}