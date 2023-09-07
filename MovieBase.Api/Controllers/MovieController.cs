using Microsoft.AspNetCore.Mvc;
using MovieBase.Common;

namespace MovieBase.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly ILogger<MovieController> _logger;

    public MovieController(ILogger<MovieController> logger)
    {
        _logger = logger;
    }

    [HttpGet("[Action]", Name = "GetAllMovies")]
    public IEnumerable<Movie> List()
    {
        return new List<Movie> { new Movie { Id = 1, Title = "Indiana Jones" }, new Movie { Id = 2, Title = "Starwars 27" } };
    }

    [HttpGet("{id}")]
    public Movie Get(int id)
    {
        return new Movie { Id = id, Title = $"Movie {id}" };
    }

    [HttpDelete("{id}")]
    public bool Delete(int id)
    {
        return true;
    }

    [HttpPost]
    public bool Post([FromBody] Movie movie)
    {
        return true;
    }
}
