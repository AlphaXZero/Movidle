using Movidle.Models;

public class AppState
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public List<Movie> GuessHistory { get; set; } = new();
    public Movie rdm_movie { get; set; } = new Movie();
}