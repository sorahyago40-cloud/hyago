using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Caiman_Panel;

public partial class PanelViewModel : ObservableObject
{
	private readonly IPanelService _panelService;
	private readonly IAuthService _authService;
	private Timer? _statusUpdateTimer;

	[ObservableProperty]
	private string currentUser = "Usuário";

	[ObservableProperty]
	private string lastLoginTime = DateTime.Now.ToString("HH:mm");

	[ObservableProperty]
	private string sessionStatus = "🟢 Ativo";

	[ObservableProperty]
	private string connectionStatus = "🟢 Conectado";

	// Funcionalidades - Aimbot
	[ObservableProperty]
	private bool aimbotEnabled = false;

	[ObservableProperty]
	private double aimbotDelay = 100;

	// Funcionalidades - RapidFire
	[ObservableProperty]
	private bool rapidFireEnabled = false;

	// Funcionalidades - Wallhack
	[ObservableProperty]
	private bool wallhackEnabled = false;

	// Funcionalidades - ESP
	[ObservableProperty]
	private bool espEnabled = false;

	[ObservableProperty]
	private double espDistance = 200;

	public PanelViewModel(IPanelService panelService, IAuthService authService)
	{
		_panelService = panelService;
		_authService = authService;
	}

	public void OnAppearing()
	{
		LoadUserInfo();
		StartStatusUpdate();
	}

	public void OnDisappearing()
	{
		StopStatusUpdate();
	}

	private void LoadUserInfo()
	{
		try
		{
			CurrentUser = _authService.GetCurrentUsername() ?? "Usuário";
			LastLoginTime = DateTime.Now.ToString("HH:mm:ss");
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Erro ao carregar info do usuário: {ex.Message}");
		}
	}

	private void StartStatusUpdate()
	{
		_statusUpdateTimer = new Timer(_ =>
		{
			UpdateStatus();
		}, null, 0, 5000); // Atualizar a cada 5 segundos
	}

	private void StopStatusUpdate()
	{
		_statusUpdateTimer?.Dispose();
		_statusUpdateTimer = null;
	}

	private void UpdateStatus()
	{
		try
		{
			var isConnected = _panelService.IsConnected();
			SessionStatus = isConnected ? "🟢 Ativo" : "🔴 Inativo";
			ConnectionStatus = isConnected ? "🟢 Conectado" : "🟡 Desconectando";
		}
		catch
		{
			SessionStatus = "🔴 Erro";
			ConnectionStatus = "🔴 Desconectado";
		}
	}

	[RelayCommand]
	private async Task ApplySettings()
	{
		try
		{
			var settings = new PanelSettings
			{
				AimbotEnabled = AimbotEnabled,
				AimbotDelay = (int)AimbotDelay,
				RapidFireEnabled = RapidFireEnabled,
				WallhackEnabled = WallhackEnabled,
				ESPEnabled = ESPEnabled,
				ESPDistance = (int)ESPDistance
			};

			var result = await _panelService.ApplySettingsAsync(settings);

			if (result)
			{
				await Shell.Current.DisplayAlert("✅ Sucesso", "Configurações aplicadas com sucesso", "OK");
			}
			else
			{
				await Shell.Current.DisplayAlert("❌ Erro", "Falha ao aplicar configurações", "OK");
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("❌ Erro", $"Erro: {ex.Message}", "OK");
		}
	}

	[RelayCommand]
	private async Task Restart()
	{
		var result = await Shell.Current.DisplayAlert("Confirmar", "Deseja reiniciar o painel?", "Sim", "Não");
		if (result)
		{
			try
			{
				await _panelService.RestartAsync();
				await Shell.Current.DisplayAlert("✅ Sucesso", "Painel reiniciando...", "OK");
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("❌ Erro", $"Erro ao reiniciar: {ex.Message}", "OK");
			}
		}
	}

	[RelayCommand]
	private async Task Logout()
	{
		var result = await Shell.Current.DisplayAlert("Confirmar", "Desconectar e sair?", "Sim", "Não");
		if (result)
		{
			try
			{
				StopStatusUpdate();
				await _authService.LogoutAsync();
				await Shell.Current.GoToAsync("login");
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("❌ Erro", $"Erro ao sair: {ex.Message}", "OK");
			}
		}
	}
}
