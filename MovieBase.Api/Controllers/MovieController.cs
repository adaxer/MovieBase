using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MovieBase.Common;
using MovieBase.Data;

namespace MovieBase.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly ILogger<MovieController> _logger;
    private readonly MovieContext db;

    public MovieController(ILogger<MovieController> logger, MovieContext db)
    {
        _logger = logger;
        this.db = db;
    }

    [HttpGet("[Action]", Name = "GetAllMovies")]
    public IEnumerable<Movie> List()
    {
        return db.Movies.ToList();
    }

    [HttpGet("{id}")]
    public Movie? Get(int id)
    {
        return (db.Movies.Find(id) is Movie theOne)
            ? theOne
            : null;
    }

    [HttpDelete("{id}")]
    public bool Delete(int id)
    {
        if (db.Movies.Find(id) is Movie theOne)
        {
            db.Movies.Remove(theOne);
            db.SaveChanges();
            return true;
        }
        return false;
    }

    [HttpPost]
    public bool Post(Movie movie)
    {
        db.Movies.Add(movie);
        db.SaveChanges();
        return true;
    }

    [HttpPut("{id}")]
    public bool Put(int id, Dictionary<string, string> values)
    {
        return true;
    }

    [HttpPatch]
    public IActionResult Patch([FromBody] JsonPatchDocument<Movie> jsonPatch)
    {
        return Ok();
    }

    [Authorize]
    [HttpGet("[action]")]
    public string AmILoggedIn()
    {
        return "Yes";
    }
}
