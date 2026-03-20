using Movidle.Models;
using Movidle.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;


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
    private bool won = false;
    private List<string> favoriteFilms = new();
    private string isFavorite(Movie _movie)
    {
        if (AppState.UserId == 0) return string.Empty;
        if (favoriteFilms.Contains(_movie.Title))
        {
            return "favorite";
        }
        return "unfavorite";
    }
    private string base_verif(string guess, string toFind)
    {
        if (guess != null && toFind != null)
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
        if (guess != null && toFind != null)
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
        if (guess == null || toFind == null) return string.Empty;

        if (guess.SequenceEqual(toFind))
            return "cell_verif_good";

        if (guess.Intersect(toFind).Any())
            return "cell_verif_partial";

        return "cell_verif_bad";
    }
    private async Task MakeAGuess()
    {
        movie = await MovieService.SearchMovie(title);
        if (movie?.Title == null)
        {
            log = "Film non trouvé. Veuillez réessayer.";
            return;
        }
        log = String.Empty;

        AppState.CurrentGuess.Add(movie);
        title = String.Empty;
        if (movie.Title == rdm_movie.Title)
        {
            won = true;
        }
    }

    private void Restart()
    {
        AppState.CurrentGuess.Clear();
        AppState.rdm_movie = new Movie();
        won = false;
        // Force reinitialization of the component to reset the game state
        _ = OnInitializedAsync();
    }

    private async Task AddToFavorites(Movie _movie)
    {
        if (_movie == null || AppState.UserId == 0)
        {
            log = "Please log in to add favorites.";
            return;
        }

        await UserService.AddFavoriteFilm(AppState.UserId, _movie.Title);
    }

    private async Task RemoveFromFavorites(Movie _movie)
    {
        if (_movie == null || AppState.UserId == 0)
        {
            log = "Please log in to remove favorites.";
            return;
        }

        await UserService.RemoveFavoriteFilm(AppState.UserId, _movie.Title);
    }
    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await MakeAGuess();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        favoriteFilms = await UserService.GetFavoriteFilms(AppState.UserId);
        movies = AppState.CurrentGuess;
        if (AppState.rdm_movie.Title == string.Empty)
        {
            rdm_movie = await MovieService.GetRandomMovie();
            AppState.rdm_movie = rdm_movie;
        }
        else
        {
            rdm_movie = AppState.rdm_movie;
        }
    }
}