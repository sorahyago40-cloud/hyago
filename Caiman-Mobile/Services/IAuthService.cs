namespace Caiman_Panel;

public interface IAuthService
{
	Task<AuthResult> LoginAsync(string username, string password);
	Task<AuthResult> RegisterAsync(string username, string password, string licenseKey);
	Task LogoutAsync();
	Task<bool> RefreshTokenAsync();

	bool IsAuthenticated { get; }
	string? GetCurrentUsername();
	string? GetCurrentToken();
	DateTime? GetTokenExpiration();
}

public class AuthResult
{
	public bool Success { get; set; }
	public string? Message { get; set; }
	public string? Token { get; set; }
	public int ExpiresIn { get; set; } // em segundos
}
