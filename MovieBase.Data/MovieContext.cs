using Microsoft.EntityFrameworkCore;
using MovieBase.Common;

namespace MovieBase.Data;

public class MovieContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }

    public MovieContext(DbContextOptions<MovieContext> options) : base(options)
    {
        Seed();
    }

    void Seed()
    {
        if (!Movies.Any()) // Überprüft, ob bereits Filme vorhanden sind
        {
            var movies = new List<Movie>
            {
                new Movie { Title = "Blade Runner", Director = "Ridley Scott", Released = DateOnly.Parse("1982-06-25") },
                new Movie { Title = "Star Wars: A New Hope", Director = "George Lucas", Released = DateOnly.Parse("1977-05-25") },
                new Movie { Title = "The Matrix", Director = "Lana Wachowski, Lilly Wachowski", Released = DateOnly.Parse("1999-03-31") },
                new Movie { Title = "Inception", Director = "Christopher Nolan", Released = DateOnly.Parse("2010-07-16") },
                new Movie { Title = "Interstellar", Director = "Christopher Nolan", Released = DateOnly.Parse("2014-11-07") },
                new Movie { Title = "2001: A Space Odyssey", Director = "Stanley Kubrick", Released = DateOnly.Parse("1968-04-02") },
                new Movie { Title = "The Fifth Element", Director = "Luc Besson", Released = DateOnly.Parse("1997-05-09") },
                new Movie { Title = "Avatar", Director = "James Cameron", Released = DateOnly.Parse("2009-12-18") },
                new Movie { Title = "The Martian", Director = "Ridley Scott", Released = DateOnly.Parse("2015-09-30") },
                new Movie { Title = "Arrival", Director = "Denis Villeneuve", Released = DateOnly.Parse("2016-11-11") }
            };

            Movies.AddRange(movies);
            SaveChanges();
        }
    }
}
