using Microsoft.AspNetCore.Mvc;
using JokeApi.Services;
using JokeApi.Models;

[ApiController]
[Route("api/v1/[controller]")]
[ApiVersion("1.0")]
public class JokesController : ControllerBase
{
    private readonly IJokeService _jokeService;

    public JokesController(IJokeService jokeService)
    {
        _jokeService = jokeService;
    }

    [HttpGet("random")]
    public ActionResult<JokeDto> GetRandomJoke()
    {
        var joke = _jokeService.GetRandomJoke();
        if (joke == null)
            return NotFound();

        var jokeDto = MapToDto(joke);
        return jokeDto;
    }

    [HttpGet]
    public ActionResult<Dictionary<JokeLengthCategory, List<JokeDto>>> SearchJokes([FromQuery] string searchTerm)
    {

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("Search term cannot be empty");
        }

        var jokes = _jokeService.SearchJokes(searchTerm);

        if (jokes == null || jokes.Count == 0)
        {
            return NotFound();
        }

        var categorizedJokes = new Dictionary<JokeLengthCategory, List<JokeDto>>();
        foreach (var category in jokes.Keys)
        {
            var jokeDtos = jokes[category].Select(joke => MapToDto(joke)).ToList();
            categorizedJokes.Add(category, jokeDtos);
        }

        return Ok(categorizedJokes); ;
    }

    private JokeDto MapToDto(Joke joke)
    {
        return new JokeDto
        {
            Id = joke.Id,
            Text = joke.Text,
            Length = joke.Length
        };
    }
}