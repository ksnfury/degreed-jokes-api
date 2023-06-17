using Microsoft.AspNetCore.Mvc;
using JokeApi.Services;
using JokeApi.Models;
using Microsoft.AspNetCore.Authorization;

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


    // Login action for user
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Check if the username and password are valid (dummy implementation)
        if (request.Username == "dummy" && request.Password == "password")
        {
            // Generate a JWT token (dummy implementation)
            var token = GenerateJwtToken(request.Username);

            // Return the JWT token as a response
            return Ok(new { token });
        }

        // Return an unauthorized status code if the login credentials are invalid
        return Unauthorized();
    }

    private string GenerateJwtToken(string username)
    {
        // Generate a JWT token using your preferred JWT library
        // Include the necessary claims, such as username and any required roles/permissions

        // Dummy implementation
        var token = "dummy.jwt.token";
        return token;
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
