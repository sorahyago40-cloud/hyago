using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Caiman_Panel;

public partial class SettingsViewModel : ObservableObject
{
	private readonly IStorageService _storageService;
	private readonly IAuthService _authService;
	private Timer? _performanceTimer;

	[ObservableProperty]
	private string username = string.Empty;

	[ObservableProperty]
	private string accountStatus = "Ativo";

	[ObservableProperty]
	private bool notificationsEnabled = true;

	[ObservableProperty]
	private bool autoLoginEnabled = false;

	[ObservableProperty]
	private double cpuUsage = 0.25; // 25%

	[ObservableProperty]
	private double memoryUsage = 0.40; // 40%

	[ObservableProperty]
	private string lastUpdateTime = "Nunca";

	public SettingsViewModel(IStorageService storageService, IAuthService authService)
	{
		_storageService = storageService;
		_authService = authService;
	}

	public void OnAppearing()
	{
		LoadSettings();
		StartPerformanceMonitoring();
	}

	private void LoadSettings()
	{
		try
		{
			Username = _authService.GetCurrentUsername() ?? "Usuário Desconhecido";
			AccountStatus = "✅ Ativo";

			NotificationsEnabled = _storageService.Get("notifications") != "false";
			AutoLoginEnabled = _storageService.Get("autologin") == "true";

			var lastUpdate = _storageService.Get("lastupdatecheck");
			if (!string.IsNullOrEmpty(lastUpdate))
			{
				LastUpdateTime = lastUpdate;
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Erro ao carregar configurações: {ex.Message}");
		}
	}

	private void StartPerformanceMonitoring()
	{
		_performanceTimer = new Timer(_ =>
		{
			UpdatePerformanceMetrics();
		}, null, 0, 2000); // Atualizar a cada 2 segundos
	}

	private void UpdatePerformanceMetrics()
	{
		try
		{
			// Simular valores de performance (em produção, isso viria do device)
			CPUUsage = Random.Shared.NextDouble() * 0.6 + 0.1; // 10-70%
			MemoryUsage = Random.Shared.NextDouble() * 0.5 + 0.2; // 20-70%
		}
		catch
		{
			// Ignorar erros de monitoramento
		}
	}

	[RelayCommand]
	private async Task CheckUpdates()
	{
		try
		{
			await Shell.Current.DisplayAlert("🔄 Verificando", "Procurando atualizações...", "OK");

			// Simular verificação
			await Task.Delay(2000);

			LastUpdateTime = DateTime.Now.ToString("HH:mm dd/MM");
			_storageService.Set("lastupdatecheck", LastUpdateTime);

			await Shell.Current.DisplayAlert("✅ Verificação", "Você está usando a versão mais recente", "OK");
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("❌ Erro", $"Erro ao verificar atualizações: {ex.Message}", "OK");
		}
	}

	[RelayCommand]
	private async Task ClearCache()
	{
		var result = await Shell.Current.DisplayAlert("Confirmar", "Limpar cache da aplicação?", "Sim", "Não");
		if (result)
		{
			try
			{
				// Limpar cache (implementar conforme necessário)
				await Shell.Current.DisplayAlert("✅ Sucesso", "Cache limpo com sucesso", "OK");
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert("❌ Erro", $"Erro ao limpar cache: {ex.Message}", "OK");
			}
		}
	}

	[RelayCommand]
	private async Task ViewLogs()
	{
		try
		{
			var logs = "Logs do Sistema:\n\n";
			logs += "[INFO] Aplicação iniciada\n";
			logs += "[INFO] Usuário autenticado\n";
			logs += "[DEBUG] Serviços carregados\n";
			logs += "[WARNING] Conexão lenta detectada\n";

			await Shell.Current.DisplayAlert("📋 Logs", logs, "Fechar");
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("❌ Erro", $"Erro ao exibir logs: {ex.Message}", "OK");
		}
	}

	[RelayCommand]
	private async Task Logout()
	{
		var result = await Shell.Current.DisplayAlert("Confirmar", "Desconectar?", "Sim", "Não");
		if (result)
		{
			try
			{
				_performanceTimer?.Dispose();
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
