using Microsoft.AspNetCore.Mvc;
using JokeApi.Services;
using JokeApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

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
    [AllowAnonymous]
    public ActionResult<JokeDto> GetRandomJoke()
    {
        var joke = _jokeService.GetRandomJoke();
        if (joke == null)
            return NotFound();

        var jokeDto = MapToDto(joke);
        return jokeDto;
    }

    [HttpGet]
    [Authorize]
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
    [AllowAnonymous]
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
        // Set the secret key used for signing the token
        var secretKey = "wZGZG2D+gG+7X+Y+kuRKMfbXSNYMaq0ZAwX3cJvT02c=";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        // Create the signing credentials using the key and algorithm
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Create the claims for the token
        var claims = new[]
        {
        new Claim(ClaimTypes.Name, username)
    };

        // Create the JWT token
        var token = new JwtSecurityToken(
            issuer: "JokeAPI",
            audience: "JokeAPIUsers",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15), 
            signingCredentials: credentials);

        // Serialize the token to a string
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
