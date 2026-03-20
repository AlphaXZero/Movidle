using Movidle.Models;
using Movidle.Services;
using Microsoft.AspNetCore.Components;


namespace Movidle.Components.Pages;

public partial class Home : ComponentBase
{

    [Inject] private MovieService MovieService { get; set; } = default!;
    [Inject] private UserService UserService { get; set; } = default!;
    [Inject] private AppState AppState { get; set; } = default!;

    private Movie rdm_movie = new();
    private List<Movie> movies = new();
    private string title = string.Empty;
    private Movie? movie;
    private string log = string.Empty;

    private string base_verif(string guess, string toFind)
    {
        if (movie != null && rdm_movie != null)
        {
            if (guess == toFind)
            {
                return "cell_verif_good";
            }
            else
            {
                return "cell_verif_bad";
            }
        }
        return string.Empty;
    }
    private string year_verif(string guess, string toFind)
    {
        if (movie != null && rdm_movie != null)
        {
            if (guess == toFind)
            {
                return "cell_verif_good";
            }
            else if (guess == "N/A" || toFind == "N/A")
            {
                return "cell_verif_bad";
            }
            else if (int.Parse(guess) > int.Parse(toFind))
            {
                return "up_year";
            }
            else if (int.Parse(guess) < int.Parse(toFind))
            {
                return "down_year";
            }
        }
        return string.Empty;
    }

    private string genre_verif(List<string> guess, List<string> toFind)
    {
        if (movie == null || rdm_movie == null) return string.Empty;

        if (guess.SequenceEqual(toFind))
            return "cell_verif_good";

        if (guess.Intersect(toFind).Any())
            return "cell_verif_partial";

        return "cell_verif_bad";
    }
    private async Task SearchMovie()
    {
        movie = await MovieService.SearchMovie(title);
        if (movie?.Title == null)
        {
            log = "Film non trouvé. Veuillez réessayer.";
            return;
        }
        log = String.Empty;
        movies.Add(movie);
        AppState.CurrentGuess.Add(movie);
    }

    private async Task AddToFavorites(Movie _movie)
    {
        if (_movie == null || AppState.UserId == 0)
        {
            log = "Please log in to add favorites.";
            return;
        }

        await UserService.AddFavoriteFilm(AppState.UserId, _movie.Title);
        log = $"Added '{_movie.Title}' to favorites!";
    }

    protected override async Task OnInitializedAsync()
    {
        movies = AppState.CurrentGuess.Count > 0 ? AppState.CurrentGuess : new List<Movie>();
        rdm_movie = await MovieService.GetRandomMovie();
        StateHasChanged();
    }
}