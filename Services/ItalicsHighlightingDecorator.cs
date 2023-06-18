using JokeApi.Models;

namespace JokeApi.Services
{
    public class ItalicsHighlightingDecorator : IHighlightingDecorator
    {
        public string Decorate(string jokeText, string searchTerm)
        {
            // Add italics decoration to the searched term in the joke text
            var decoratedText = jokeText.Replace(searchTerm, $"<i>{searchTerm}</i>");

            // Return the decorated text
            return decoratedText;
        }
    }
}