using Microsoft.EntityFrameworkCore;
using Movidle.Data;
using Movidle.Models;
using Movidle.Services;
using Xunit;

namespace Movidle.Tests;

// ─── Helpers ────────────────────────────────────────────────────────────────

file static class Factory
{
    public static AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    public static Movie MakeMovie(string title, string director = "Director",
        string year = "2000", string metascore = "80",
        List<string>? genres = null, List<string>? countries = null) => new()
        {
            Title = title,
            Director = director,
            Year = year,
            Metascore = metascore,
            Genres = genres ?? new() { "Action" },
            Countries = countries ?? new() { "USA" }
        };
}

// ─── DB / UserService ────────────────────────────────────────────────────────

public class UserServiceTests
{
    [Fact(DisplayName = "Register → creates the user in DB")]
    public async Task Register_CreatesUser()
    {
        var service = new UserService(Factory.CreateDb());
        Assert.True(await service.Register("alice", "pass"));
    }

    [Fact(DisplayName = "Register → rejects duplicate username")]
    public async Task Register_RejectsDuplicate()
    {
        var service = new UserService(Factory.CreateDb());
        await service.Register("alice", "pass");
        Assert.False(await service.Register("alice", "other"));
    }

    [Fact(DisplayName = "Login → returns user on correct credentials")]
    public async Task Login_CorrectCredentials_ReturnsUser()
    {
        var service = new UserService(Factory.CreateDb());
        await service.Register("alice", "pass");
        var user = await service.Login("alice", "pass");
        Assert.NotNull(user);
        Assert.Equal("alice", user.Username);
    }

    [Fact(DisplayName = "Login → returns null on wrong password")]
    public async Task Login_WrongPassword_ReturnsNull()
    {
        var service = new UserService(Factory.CreateDb());
        await service.Register("alice", "pass");
        Assert.Null(await service.Login("alice", "wrong"));
    }

    [Fact(DisplayName = "AddFavorite → film appears in favorites")]
    public async Task AddFavorite_AppearsInList()
    {
        var service = new UserService(Factory.CreateDb());
        await service.Register("alice", "pass");
        var user = await service.Login("alice", "pass");
        await service.AddFavoriteFilm(user!.Id, "Inception");
        Assert.Contains("Inception", await service.GetFavoriteFilms(user.Id));
    }

    [Fact(DisplayName = "RemoveFavorite → film disappears from favorites")]
    public async Task RemoveFavorite_DisappearsFromList()
    {
        var service = new UserService(Factory.CreateDb());
        await service.Register("alice", "pass");
        var user = await service.Login("alice", "pass");
        await service.AddFavoriteFilm(user!.Id, "Inception");
        await service.RemoveFavoriteFilm(user.Id, "Inception");
        Assert.DoesNotContain("Inception", await service.GetFavoriteFilms(user.Id));
    }
}

// ─── AppState ────────────────────────────────────────────────────────────────

public class AppStateTests
{
    [Fact(DisplayName = "AppState → starts with empty username and no guesses")]
    public void AppState_DefaultValues_AreEmpty()
    {
        var state = new AppState();
        Assert.Equal(string.Empty, state.Username);
        Assert.Equal(0, state.UserId);
        Assert.Empty(state.GuessHistory);
        Assert.Equal(string.Empty, state.RandomMovie.Title);
    }

    [Fact(DisplayName = "AppState → GuessHistory stores added movies")]
    public void AppState_GuessHistory_StoresMovies()
    {
        var state = new AppState();
        state.GuessHistory.Add(Factory.MakeMovie("Inception"));
        Assert.Single(state.GuessHistory);
        Assert.Equal("Inception", state.GuessHistory[0].Title);
    }

    [Fact(DisplayName = "AppState → GuessHistory clears correctly")]
    public void AppState_GuessHistory_ClearsCorrectly()
    {
        var state = new AppState();
        state.GuessHistory.Add(Factory.MakeMovie("Inception"));
        state.GuessHistory.Clear();
        Assert.Empty(state.GuessHistory);
    }
}

// ─── Verification logic ──────────────────────────────────────────────────────

public class VerificationTests
{
    // Simule les méthodes de Home.razor.cs extraites pour être testables
    private static string BaseVerif(string guess, string toFind)
    {
        if (guess == toFind) return "cell_verif_good";
        return "cell_verif_bad";
    }

    private static string YearVerif(string guess, string toFind)
    {
        if (guess == toFind) return "cell_verif_good";
        if (guess == "N/A" || toFind == "N/A") return "cell_verif_bad";
        if (int.Parse(guess) > int.Parse(toFind)) return "up_year";
        if (int.Parse(guess) < int.Parse(toFind)) return "down_year";
        return string.Empty;
    }

    private static string GenreVerif(List<string> guess, List<string> toFind)
    {
        if (guess.SequenceEqual(toFind)) return "cell_verif_good";
        if (guess.Intersect(toFind).Any()) return "cell_verif_partial";
        return "cell_verif_bad";
    }

    // BaseVerif
    [Fact(DisplayName = "BaseVerif → exact match returns good")]
    public void BaseVerif_ExactMatch_ReturnsGood() =>
        Assert.Equal("cell_verif_good", BaseVerif("Nolan", "Nolan"));

    [Fact(DisplayName = "BaseVerif → different values returns bad")]
    public void BaseVerif_Different_ReturnsBad() =>
        Assert.Equal("cell_verif_bad", BaseVerif("Nolan", "Spielberg"));

    // YearVerif
    [Fact(DisplayName = "YearVerif → exact year returns good")]
    public void YearVerif_ExactYear_ReturnsGood() =>
        Assert.Equal("cell_verif_good", YearVerif("2000", "2000"));

    [Fact(DisplayName = "YearVerif → guess too high returns up_year (go lower)")]
    public void YearVerif_GuessTooHigh_ReturnsUpYear() =>
        Assert.Equal("up_year", YearVerif("2010", "2000"));

    [Fact(DisplayName = "YearVerif → guess too low returns down_year (go higher)")]
    public void YearVerif_GuessTooLow_ReturnsDownYear() =>
        Assert.Equal("down_year", YearVerif("1990", "2000"));

    [Fact(DisplayName = "YearVerif → N/A value returns bad")]
    public void YearVerif_NAValue_ReturnsBad() =>
        Assert.Equal("cell_verif_bad", YearVerif("N/A", "2000"));

    // GenreVerif
    [Fact(DisplayName = "GenreVerif → exact genre list returns good")]
    public void GenreVerif_ExactMatch_ReturnsGood() =>
        Assert.Equal("cell_verif_good", GenreVerif(
            new() { "Action", "Drama" },
            new() { "Action", "Drama" }));

    [Fact(DisplayName = "GenreVerif → partial genre overlap returns partial")]
    public void GenreVerif_PartialMatch_ReturnsPartial() =>
        Assert.Equal("cell_verif_partial", GenreVerif(
            new() { "Action", "Comedy" },
            new() { "Action", "Drama" }));

    [Fact(DisplayName = "GenreVerif → no genre overlap returns bad")]
    public void GenreVerif_NoMatch_ReturnsBad() =>
        Assert.Equal("cell_verif_bad", GenreVerif(
            new() { "Comedy" },
            new() { "Horror" }));
}