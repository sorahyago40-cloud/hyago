namespace Caiman_Panel;

public class PanelSettings
{
	public bool AimbotEnabled { get; set; }
	public int AimbotDelay { get; set; }
	public bool RapidFireEnabled { get; set; }
	public bool WallhackEnabled { get; set; }
	public bool ESPEnabled { get; set; }
	public int ESPDistance { get; set; }
}

public interface IPanelService
{
	Task<bool> ApplySettingsAsync(PanelSettings settings);
	Task<bool> RestartAsync();
	bool IsConnected();
	Task<string> GetStatusAsync();
	Task<Dictionary<string, object>> GetStatsAsync();
}

public class PanelService : IPanelService
{
	private readonly IApiService _apiService;
	private readonly IAuthService _authService;
	private bool _isConnected = true;
	private DateTime _lastConnectionCheck = DateTime.Now;

	public PanelService(IApiService apiService, IAuthService authService)
	{
		_apiService = apiService;
		_authService = authService;
	}

	public async Task<bool> ApplySettingsAsync(PanelSettings settings)
	{
		try
		{
			var token = _authService.GetCurrentToken();
			if (string.IsNullOrEmpty(token))
				return false;

			var result = await _apiService.PostAsync<PanelSettings, dynamic>(
				"/panel/settings/apply",
				settings,
				token
			);

			return result != null;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Erro ao aplicar configurações: {ex.Message}");
			return false;
		}
	}

	public async Task<bool> RestartAsync()
	{
		try
		{
			var token = _authService.GetCurrentToken();
			if (string.IsNullOrEmpty(token))
				return false;

			var result = await _apiService.PostAsync<object, dynamic>(
				"/panel/restart",
				new { },
				token
			);

			return result != null;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Erro ao reiniciar: {ex.Message}");
			return false;
		}
	}

	public bool IsConnected()
	{
		// Verificar conexão a cada 30 segundos
		if ((DateTime.Now - _lastConnectionCheck).TotalSeconds > 30)
		{
			CheckConnectionAsync().GetAwaiter().GetResult();
			_lastConnectionCheck = DateTime.Now;
		}

		return _isConnected;
	}

	public async Task<string> GetStatusAsync()
	{
		try
		{
			var token = _authService.GetCurrentToken();
			if (string.IsNullOrEmpty(token))
				return "Desconectado";

			var result = await _apiService.GetAsync<dynamic>("/panel/status", token);
			return result?.status?.ToString() ?? "Desconhecido";
		}
		catch
		{
			return "Erro";
		}
	}

	public async Task<Dictionary<string, object>> GetStatsAsync()
	{
		try
		{
			var token = _authService.GetCurrentToken();
			if (string.IsNullOrEmpty(token))
				return new Dictionary<string, object>();

			var result = await _apiService.GetAsync<Dictionary<string, object>>("/panel/stats", token);
			return result ?? new Dictionary<string, object>();
		}
		catch
		{
			return new Dictionary<string, object>();
		}
	}

	private async Task CheckConnectionAsync()
	{
		try
		{
			var token = _authService.GetCurrentToken();
			if (string.IsNullOrEmpty(token))
			{
				_isConnected = false;
				return;
			}

			var result = await _apiService.GetAsync<dynamic>("/health", token);
			_isConnected = result != null;
		}
		catch
		{
			_isConnected = false;
		}
	}
}
