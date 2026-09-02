using System.Text;
using System.Text.Json;

namespace Caiman_Panel;

public interface IApiService
{
	Task<AuthResult> LoginAsync(string username, string passwordHash);
	Task<AuthResult> RegisterAsync(string username, string passwordHash, string licenseKey);
	Task<bool> LogoutAsync(string token);
	Task<AuthResult> RefreshTokenAsync(string token);
	Task<T?> GetAsync<T>(string endpoint, string? token = null);
	Task<TResult?> PostAsync<TData, TResult>(string endpoint, TData data, string? token = null);
}

public class ApiService : IApiService
{
	private readonly HttpClient _httpClient;
	private const string BaseUrl = "https://api.caiman.panel"; // Configurar com seu servidor

	public ApiService()
	{
		var handler = new HttpClientHandler();
#if DEBUG
		handler.ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true;
#endif
		_httpClient = new HttpClient(handler)
		{
			Timeout = TimeSpan.FromSeconds(30),
			BaseAddress = new Uri(BaseUrl)
		};

		_httpClient.DefaultRequestHeaders.Add("User-Agent", "Caiman-Mobile/1.0");
	}

	public async Task<AuthResult> LoginAsync(string username, string passwordHash)
	{
		try
		{
			var payload = new { username, password = passwordHash };
			var content = new StringContent(
				JsonSerializer.Serialize(payload),
				Encoding.UTF8,
				"application/json"
			);

			var response = await _httpClient.PostAsync("/auth/login", content);
			var json = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				using var doc = JsonDocument.Parse(json);
				var root = doc.RootElement;

				return new AuthResult
				{
					Success = true,
					Token = root.GetProperty("token").GetString(),
					ExpiresIn = root.TryGetProperty("expiresIn", out var exp) ? exp.GetInt32() : 3600,
					Message = "Login bem-sucedido"
				};
			}

			return new AuthResult
			{
				Success = false,
				Message = "Credenciais inválidas"
			};
		}
		catch (HttpRequestException ex)
		{
			Debug.WriteLine($"Erro de conexão: {ex.Message}");
			return new AuthResult
			{
				Success = false,
				Message = "Erro de conexão com servidor"
			};
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Erro ao fazer login: {ex.Message}");
			return new AuthResult
			{
				Success = false,
				Message = $"Erro: {ex.Message}"
			};
		}
	}

	public async Task<AuthResult> RegisterAsync(string username, string passwordHash, string licenseKey)
	{
		try
		{
			var payload = new { username, password = passwordHash, licenseKey };
			var content = new StringContent(
				JsonSerializer.Serialize(payload),
				Encoding.UTF8,
				"application/json"
			);

			var response = await _httpClient.PostAsync("/auth/register", content);
			var json = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				return new AuthResult { Success = true, Message = "Registro bem-sucedido" };
			}

			return new AuthResult
			{
				Success = false,
				Message = "Falha ao registrar"
			};
		}
		catch (Exception ex)
		{
			return new AuthResult { Success = false, Message = ex.Message };
		}
	}

	public async Task<bool> LogoutAsync(string token)
	{
		try
		{
			AddAuthHeader(token);
			var response = await _httpClient.PostAsync("/auth/logout", null);
			return response.IsSuccessStatusCode;
		}
		catch
		{
			return false;
		}
	}

	public async Task<AuthResult> RefreshTokenAsync(string token)
	{
		try
		{
			AddAuthHeader(token);
			var response = await _httpClient.PostAsync("/auth/refresh", null);
			var json = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				using var doc = JsonDocument.Parse(json);
				return new AuthResult
				{
					Success = true,
					Token = doc.RootElement.GetProperty("token").GetString(),
					ExpiresIn = 3600
				};
			}

			return new AuthResult { Success = false };
		}
		catch
		{
			return new AuthResult { Success = false };
		}
	}

	public async Task<T?> GetAsync<T>(string endpoint, string? token = null)
	{
		try
		{
			if (!string.IsNullOrEmpty(token))
				AddAuthHeader(token);

			var response = await _httpClient.GetAsync(endpoint);
			var json = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				return JsonSerializer.Deserialize<T>(json);
			}

			return default;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Erro ao fazer GET: {ex.Message}");
			return default;
		}
	}

	public async Task<TResult?> PostAsync<TData, TResult>(string endpoint, TData data, string? token = null)
	{
		try
		{
			if (!string.IsNullOrEmpty(token))
				AddAuthHeader(token);

			var content = new StringContent(
				JsonSerializer.Serialize(data),
				Encoding.UTF8,
				"application/json"
			);

			var response = await _httpClient.PostAsync(endpoint, content);
			var json = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				return JsonSerializer.Deserialize<TResult>(json);
			}

			return default;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Erro ao fazer POST: {ex.Message}");
			return default;
		}
	}

	private void AddAuthHeader(string token)
	{
		_httpClient.DefaultRequestHeaders.Authorization =
			new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
	}
}
