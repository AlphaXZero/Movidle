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

        var movie = await _http.GetFromJsonAsync<Movie>(url);

        return movie;
    }
}