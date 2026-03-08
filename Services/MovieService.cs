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

        var json = await _http.GetFromJsonAsync<JsonObject>(url);
        if (json == null) return null;

        var movie = new Movie
        {
            ImdbID = json["imdbID"]?.ToString(),
            Title = json["Title"]?.ToString(),
            Director = json["Director"]?.ToString(),
            Year = json["Year"]?.ToString(),
            Countries = json["Country"]?.ToString()?.Split(", ").ToList() ?? new(),
            Metascore = json["Metascore"]?.ToString(),
            Genres = json["Genre"]?.ToString()
                            ?.Split(", ")
                            .ToList() ?? new()
        };
        return movie;
    }
}