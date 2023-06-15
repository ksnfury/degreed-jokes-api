using Microsoft.AspNetCore.Mvc;
using JokeApi.Services;

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
        public IActionResult GetRandomJoke()
        {
            var joke = _jokeService.GetRandomJoke();
            return Ok(joke);
        }

        [HttpGet]
        public IActionResult SearchJokes(string searchTerm)
        {
            var jokes = _jokeService.SearchJokes(searchTerm);
            return Ok(jokes);
        }
    }