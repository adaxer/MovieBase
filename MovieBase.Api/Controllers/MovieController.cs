using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBase.Common;
using MovieBase.Data;

namespace MovieBase.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly ILogger<MovieController> _logger;
    private readonly MovieContext db;
    private readonly IMapper mapper;

    public MovieController(ILogger<MovieController> logger, MovieContext db, IMapper mapper)
    {
        _logger = logger;
        this.db = db;
        this.mapper = mapper;
    }

    [HttpGet("[Action]", Name = "GetAllMovies")]
    [Produces(MediaTypeNames.Application.Xml, MediaTypeNames.Application.Json)]
    [ResponseCache(Duration=10)]
    public async Task<IActionResult> List()
    {
        var result = (await db.Movies.ToListAsync()).Select(mapper.Map<Movie, MovieDTO>).ToList();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        return ((await db.Movies.FindAsync(id)) is Movie theOne)
            ? Ok(theOne)
            : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if ((await db.Movies.FindAsync(id)) is Movie theOne)
        {
            db.Movies.Remove(theOne);
            await db.SaveChangesAsync();
            return Ok();
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Post(Movie movie)
    {
        try
        {
            await db.Movies.AddAsync(movie);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Post failed");
            return BadRequest(ex);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Movie movie)
    {
        if (id != movie.Id)
        {
            return BadRequest("Die Movie-Id im Pfad stimmt nicht mit der im Body überein.");
        }
        if((await db.Movies.FindAsync(id)) is Movie existingMovie)
        {
            db.Entry(existingMovie).CurrentValues.SetValues(movie);
            await db.SaveChangesAsync();
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPatch]
    public IActionResult Patch([FromBody] JsonPatchDocument<Movie> jsonPatch)
    {
        return Ok();
    }

    [Authorize(Policy ="Admin")]
    [HttpGet("[action]")]
    public string AmILoggedIn()
    {
        var isAdmin = User.IsInRole("admin");
        return isAdmin
            ? "Yes"
            : "Maybe";
    }
}
