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

    private string spinstate = "inactive";
    private async Task HandleRegister()
    {
        if (Username != string.Empty && Password != string.Empty)
        {
            spinstate = "spinner";
            var state = await UserService.Register(Username, Password);
            if (state)
            {
                var user_info = await UserService.Login(Username, Password);
                AppState.UserId = user_info.Id;
                AppState.Username = user_info.Username;
                Navigation.NavigateTo($"/");
            }
            else
            {
                register_state = "This username is already taken.";
                spinstate = "inactive";
            }
        }
    }

    private async Task HandleLogin()
    {
        spinstate = "spinner";
        if (Username == string.Empty || Password == string.Empty)
        {
            login_state = "Please enter both username and password.";
            spinstate = "inactive";
            return;
        }
        else
        {
            var user_info = await UserService.Login(Username, Password);
            if (user_info == null)
            {
                login_state = "Invalid username or password.";
                spinstate = "inactive";
                return;
            }
            AppState.UserId = user_info.Id;
            AppState.Username = user_info.Username;
            Navigation.NavigateTo($"/");

        }

    }
}