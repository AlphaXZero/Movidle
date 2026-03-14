using Microsoft.EntityFrameworkCore;
using Movidle.Data;
using Movidle.Models;
using System.Security.Cryptography;
using System.Text;

namespace Movidle.Services;

public class UserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    private string HashPassword(string password) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));

    public async Task<User?> Login(string username, string password)
    {
        var hash = HashPassword(password);
        return await _db.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == hash);
    }

    public async Task<bool> Register(string username, string password)
    {
        if (await _db.Users.AnyAsync(u => u.Username == username))
            return false;

        _db.Users.Add(new User
        {
            Username = username,
            Password = HashPassword(password)
        });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task AddFavoriteFilm(int userId, string filmTitle)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return;

        if (!user.FavoriteFilms.Contains(filmTitle))
        {
            user.FavoriteFilms.Add(filmTitle);
            await _db.SaveChangesAsync();
        }
    }
}