namespace Movidle.Models;

public class Movie
{
    public string? ImdbID { get; set; }
    public string? Title { get; set; }
    public string? Director { get; set; }
    public string? Year { get; set; }
    public List<string>? Genres { get; set; }
    public List<string>? Countries { get; set; }
    public string? Metascore { get; set; }

}