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
    private Movie? lastGuessMovie;
    private List<Movie> movies = new();
    private Movie randomMovie = new();
    private string titleEntry = string.Empty;
    private string log = string.Empty;
    private bool isWon = false;
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
        lastGuessMovie = await MovieService.SearchMovie(titleEntry);
        if (lastGuessMovie?.Title == null)
        {
            log = "Movie not found. Please try again.";
            return;
        }
        if (movies.Any(m => m.Title == lastGuessMovie.Title))
        {
            log = "You have already guessed this movie. Try another one.";
            return;
        }
        log = String.Empty;
        AppState.GuessHistory.Add(lastGuessMovie);
        titleEntry = String.Empty;
        if (lastGuessMovie.Title == randomMovie.Title)
        {
            isWon = true;
        }
    }

    private void Restart()
    {
        AppState.GuessHistory.Clear();
        AppState.RandomMovie = new Movie();
        isWon = false;
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
        movies = AppState.GuessHistory;
        if (AppState.RandomMovie.Title == string.Empty)
        {
            randomMovie = await MovieService.GetRandomMovie();
            AppState.RandomMovie = randomMovie;
        }
        else
        {
            randomMovie = AppState.RandomMovie;
        }
    }
}