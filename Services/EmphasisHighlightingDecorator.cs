using JokeApi.Models;
using Microsoft.AspNetCore.Html;

namespace JokeApi.Services
{
    public class EmphasisHighlightingDecorator : IHighlightingDecorator
    {
        public string Decorate(string jokeText, string searchTerm)
        {
            // Add emphasis decoration to the searched term in the joke text
            var decoratedText = jokeText.Replace(searchTerm, $"<em>{searchTerm}</em>");

            // Return the decorated text
            return decoratedText;
        }
    }
}
