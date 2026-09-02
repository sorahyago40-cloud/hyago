using System.Security.Cryptography;
using System.Text;

namespace Caiman_Panel;

public class AuthService : IAuthService
{
	private readonly IStorageService _storageService;
	private readonly IApiService _apiService;
	private string? _currentToken;
	private string? _currentUsername;
	private DateTime? _tokenExpiration;

	public bool IsAuthenticated
	{
		get
		{
			if (string.IsNullOrEmpty(_currentToken))
				return false;

			if (_tokenExpiration.HasValue && DateTime.UtcNow >= _tokenExpiration)
				return false;

			return true;
		}
	}

	public AuthService(IStorageService storageService, IApiService apiService)
	{
		_storageService = storageService;
		_apiService = apiService;

		LoadStoredCredentials();
	}

	private void LoadStoredCredentials()
	{
		try
		{
			_currentToken = _storageService.Get("auth_token");
			_currentUsername = _storageService.Get("auth_username");

			var expirationStr = _storageService.Get("auth_expiration");
			if (!string.IsNullOrEmpty(expirationStr) && long.TryParse(expirationStr, out var ticks))
			{
				_tokenExpiration = new DateTime(ticks, DateTimeKind.Utc);
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Erro ao carregar credenciais armazenadas: {ex.Message}");
		}
	}

	public async Task<AuthResult> LoginAsync(string username, string password)
	{
		try
		{
			// Validar entrada
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
			{
				return new AuthResult
				{
					Success = false,
					Message = "Usuário e senha são obrigatórios"
				};
			}

			// Hash da senha (SHA256)
			var passwordHash = HashPassword(password);

			// Fazer login via API
			var result = await _apiService.LoginAsync(username, passwordHash);

			if (result.Success && !string.IsNullOrEmpty(result.Token))
			{
				// Armazenar credenciais
				_currentToken = result.Token;
				_currentUsername = username;
				_tokenExpiration = DateTime.UtcNow.AddSeconds(result.ExpiresIn);

				// Persistir
				_storageService.Set("auth_token", _currentToken);
				_storageService.Set("auth_username", _currentUsername);
				_storageService.Set("auth_expiration", _tokenExpiration!.Value.Ticks.ToString());

				return result;
			}

			return new AuthResult
			{
				Success = false,
				Message = result.Message ?? "Falha ao fazer login"
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

	public async Task<AuthResult> RegisterAsync(string username, string password, string licenseKey)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(licenseKey))
			{
				return new AuthResult
				{
					Success = false,
					Message = "Todos os campos são obrigatórios"
				};
			}

			var passwordHash = HashPassword(password);
			return await _apiService.RegisterAsync(username, passwordHash, licenseKey);
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Erro ao registrar: {ex.Message}");
			return new AuthResult
			{
				Success = false,
				Message = $"Erro: {ex.Message}"
			};
		}
	}

	public async Task LogoutAsync()
	{
		try
		{
			// Limpar token no servidor
			if (!string.IsNullOrEmpty(_currentToken))
			{
				await _apiService.LogoutAsync(_currentToken);
			}

			// Limpar memória
			_currentToken = null;
			_currentUsername = null;
			_tokenExpiration = null;

			// Limpar armazenamento
			_storageService.Remove("auth_token");
			_storageService.Remove("auth_username");
			_storageService.Remove("auth_expiration");
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Erro ao fazer logout: {ex.Message}");
		}
	}

	public async Task<bool> RefreshTokenAsync()
	{
		try
		{
			if (string.IsNullOrEmpty(_currentToken))
				return false;

			var result = await _apiService.RefreshTokenAsync(_currentToken);

			if (result.Success && !string.IsNullOrEmpty(result.Token))
			{
				_currentToken = result.Token;
				_tokenExpiration = DateTime.UtcNow.AddSeconds(result.ExpiresIn);

				_storageService.Set("auth_token", _currentToken);
				_storageService.Set("auth_expiration", _tokenExpiration.Value.Ticks.ToString());

				return true;
			}

			return false;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Erro ao renovar token: {ex.Message}");
			return false;
		}
	}

	public string? GetCurrentUsername() => _currentUsername;
	public string? GetCurrentToken() => _currentToken;
	public DateTime? GetTokenExpiration() => _tokenExpiration;

	private string HashPassword(string password)
	{
		using (var sha256 = SHA256.Create())
		{
			var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(hashedBytes);
		}
	}
}
