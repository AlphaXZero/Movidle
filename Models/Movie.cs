namespace Movidle.Models;

public class Movie
{
    public string ImdbID { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = new();
    public List<string> Countries { get; set; } = new();
    public string Metascore { get; set; } = string.Empty;

}