using Microsoft.VisualBasic;
using Movidle.Models;
using Movidle.Services;
using Microsoft.AspNetCore.Components;

namespace Movidle.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject] private MovieService MovieService { get; set; } = default!;

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

    }

    protected override async Task OnInitializedAsync()
    {
        rdm_movie = await MovieService.GetRandomMovie();
    }
}