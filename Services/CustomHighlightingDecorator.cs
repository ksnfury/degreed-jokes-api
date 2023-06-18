using JokeApi.Models;

namespace JokeApi.Services
{
    public class CustomHighlightingDecorator : IHighlightingDecorator
    {
        private readonly string _startTag;
        private readonly string _endTag;

        public CustomHighlightingDecorator(string startTag, string endTag)
        {
            _startTag = startTag;
            _endTag = endTag;
        }

        public string Decorate(string jokeText, string searchTerm)
        {
            // Add custom decoration to the searched term in the joke text
            var decoratedText = jokeText.Replace(searchTerm, $"{_startTag}{searchTerm}{_endTag}");

            // Return the decorated text
            return decoratedText;
        }
    }
}