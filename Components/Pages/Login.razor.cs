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
    private string log = string.Empty;

    private string spinstate = "inactive";
    private bool IsValidInput()
    {
        if (Username == string.Empty || Password == string.Empty)
        {
            log = "Please enter both username and password.";
            spinstate = "inactive";
            return false;
        }
        return true;
    }
    private async Task HandleRegister()
    {
        spinstate = "spinner";
        if (!IsValidInput()) { return; }
        var state = await UserService.Register(Username, Password);
        if (state)
        {
            var user_info = await UserService.Login(Username, Password);
            if (user_info == null) { return; }
            AppState.UserId = user_info.Id;
            AppState.Username = user_info.Username;
            Navigation.NavigateTo($"/");
        }
        else
        {
            log = "This username is already taken.";
            spinstate = "inactive";
        }
    }
    private async Task HandleLogin()
    {
        spinstate = "spinner";
        if (!IsValidInput()) { return; }

        var user_info = await UserService.Login(Username, Password);
        if (user_info == null)
        {
            log = "Invalid username or password.";
            spinstate = "inactive";
            return;
        }
        AppState.UserId = user_info.Id;
        AppState.Username = user_info.Username;
        Navigation.NavigateTo($"/");
    }
}