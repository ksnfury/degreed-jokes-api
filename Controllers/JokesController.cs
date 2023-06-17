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

        if (jokes.Count == 0)
            return NotFound();

        var jokeDtos = new List<JokeDto>();
        foreach (var joke in jokes)
        {
            var jokeDto = MapToDto(joke);
            jokeDtos.Add(jokeDto);
        }

        return jokeDtos;
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