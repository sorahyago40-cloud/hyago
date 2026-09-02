using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace Caiman_Panel;

public partial class LoginViewModel : ObservableObject
{
	private readonly IAuthService _authService;
	private readonly IStorageService _storageService;

	[ObservableProperty]
	private string username = string.Empty;

	[ObservableProperty]
	private string password = string.Empty;

	[ObservableProperty]
	private bool rememberMe = false;

	[ObservableProperty]
	private bool isLoading = false;

	[ObservableProperty]
	private string errorMessage = string.Empty;

	[ObservableProperty]
	private bool showError = false;

	public LoginViewModel(IAuthService authService, IStorageService storageService)
	{
		_authService = authService;
		_storageService = storageService;
	}

	public void OnAppearing()
	{
		LoadSavedCredentials();
	}

	private void LoadSavedCredentials()
	{
		try
		{
			var savedUsername = _storageService.Get("username");
			var savedPassword = _storageService.Get("password");
			var savedRememberMe = _storageService.Get("rememberme") == "true";

			if (savedRememberMe && !string.IsNullOrEmpty(savedUsername))
			{
				Username = savedUsername;
				Password = savedPassword ?? string.Empty;
				RememberMe = true;
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Erro ao carregar credenciais salvas: {ex.Message}");
		}
	}

	[RelayCommand]
	private async Task Login()
	{
		if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
		{
			ShowErrorMessage("Preencha usuário e senha!");
			return;
		}

		try
		{
			IsLoading = true;
			ShowError = false;

			var result = await _authService.LoginAsync(Username, Password);

			if (result.Success)
			{
				if (RememberMe)
				{
					_storageService.Set("username", Username);
					_storageService.Set("password", Password);
					_storageService.Set("rememberme", "true");
				}

				await Shell.Current.GoToAsync("panel");
			}
			else
			{
				ShowErrorMessage(result.Message ?? "Falha ao fazer login");
			}
		}
		catch (Exception ex)
		{
			ShowErrorMessage($"Erro: {ex.Message}");
		}
		finally
		{
			IsLoading = false;
		}
	}

	[RelayCommand]
	private async Task Register()
	{
		// Implementar navegação para tela de registro se necessário
		await Shell.Current.DisplayAlert("Info", "Funcionalidade de registro em desenvolvimento", "OK");
	}

	private void ShowErrorMessage(string message)
	{
		ErrorMessage = message;
		ShowError = true;
	}
}
