using Movidle.Models;
using Movidle.Services;
using Microsoft.AspNetCore.Components;

namespace Movidle.Components.Pages;

public partial class Login : ComponentBase
{
    [Inject] private UserService UserService { get; set; } = default!;
    [Inject] private AppState AppState { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    private string Username { get; set; } = string.Empty;
    private string Password { get; set; } = string.Empty;
    private string register_state = string.Empty;
    private string login_state = string.Empty;

    private async Task HandleRegister()
    {
        if (Username != string.Empty && Password != string.Empty)
        {
            var state = await UserService.Register(Username, Password);
            if (state)
            {
                register_state = "Registration successful! You can now log in.";
            }
            else
            {
                register_state = "This username is already taken.";
            }
        }
    }

    private async Task HandleLogin()
    {
        if (Username == string.Empty || Password == string.Empty)
        {
            login_state = "Please enter both username and password.";
            return;
        }
        else
        {
            var user_info = await UserService.Login(Username, Password);
            login_state = user_info.Username;
            AppState.UserId = user_info.Id;
            AppState.Username = user_info.Username;
            Navigation.NavigateTo($"/");
        }

    }
}