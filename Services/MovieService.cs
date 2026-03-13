using System.Text.Json.Nodes;
using Movidle.Models;
namespace Movidle.Services;

public class MovieService
{
    private readonly HttpClient _http;

    public MovieService(HttpClient http)
    {
        _http = http;
    }

    public async Task<Movie?> SearchMovie(string title)
    {

        var url = $"https://www.omdbapi.com/?t={Uri.EscapeDataString(title)}&apikey=394f0c4e";

        var json = await _http.GetFromJsonAsync<JsonObject>(url) ?? new JsonObject();
        if (json["Response"]?.ToString() == "False") return null;

        var movie = new Movie
        {
            ImdbID = json["imdbID"]?.ToString() ?? string.Empty,
            Title = json["Title"]?.ToString() ?? string.Empty,
            Director = json["Director"]?.ToString() ?? string.Empty,
            Year = json["Year"]?.ToString() ?? string.Empty,
            Countries = json["Country"]?.ToString()?.Split(", ").ToList() ?? new(),
            Metascore = json["Metascore"]?.ToString() ?? string.Empty,
            Genres = json["Genre"]?.ToString()?.Split(", ").ToList() ?? new()
        };
        return movie;
    }

    public async Task<Movie> GetRandomMovie()
    {
        Random random = new();
        List<string> movies_pool = new() { "Fight Club", "Pulp Fiction", "Inception", "Inglourious Basterds", "Avatar", "Forrest Gump",
        "Django Unchained", "Shutter Island", "The Lord of the Rings: The Fellowship of the Ring", "The Matrix", "Interstellar",
"The Dark Knight", "Kill Bill: Vol. 1", "Titanic", "Se7en", "The Shining", "The Lord of the Rings: The Return of the King",
        "Intouchables", "Back to the Future", "Spirited Away", "The Truman Show", "Drive", "Astérix & Obélix: Mission Cléopâtre",
         "The Lord of the Rings: The Two Towers", "The Wolf of Wall Street", "Amélie", "The Green Mile", "A Clockwork Orange",
         "Harry Potter and the Sorcerer's Stone", "Jurassic Park", "The Dark Knight Rises", "Gladiator", "Star Wars", "The Empire Strikes Back",
         "Return of the Jedi", "The Godfather", "The Godfather Part II", "Goodfellas", "Taxi Driver", "Casino", "Scarface", "Apocalypse Now",
         "Full Metal Jacket", "2001: A Space Odyssey", "Blade Runner", "Train to Busan", "The Host", "Decision to Leave",
         "The Northman", "Everything Everywhere All at Once", "Dune", "Dune: Part Two", "Arrival", "Ex Machina", "Her", "Gravity", "Children of Men",
          "Edge of Tomorrow", "District 9", "Looper", "Snowpiercer", "The Martian", "Ready Player One", "The Hunger Games",
           "The Maze Runner", "The Fault in Our Stars", "The Notebook", "Eternal Sunshine of the Spotless Mind", "Lost in Translation",
            "Before Sunrise", "Before Sunset", "Before Midnight", "Call Me by Your Name", "Brokeback Mountain", "Donnie Darko",
             "Fight Club", "American Beauty", "The Sixth Sense", "Unbreakable", "Split", "Glass", "Get Out", "Us", "Nope",
              "The Conjuring", "The Exorcist", "Halloween", "The Texas Chain Saw Massacre", "Scream", "Saw", "Final Destination",
               "The Blair Witch Project", "It", "It Chapter Two", "World War Z", "28 Days Later", "28 Weeks Later",
                "The Walking Dead", "Zombieland", "Train to Busan", "King Kong", "Godzilla", "Pacific Rim", "Jurassic World" };
        return await SearchMovie(movies_pool[random.Next(movies_pool.Count)]) ?? new Movie();
    }
}