namespace Caiman_Panel;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.ConfigureLogging()
			.RegisterServices()
			.RegisterPages()
			.RegisterViewModels();

		return builder.Build();
	}

	/// <summary>
	/// Configura logging para debug
	/// </summary>
	private static MauiAppBuilder ConfigureLogging(this MauiAppBuilder builder)
	{
#if DEBUG
		builder.Logging.AddDebug();
#endif
		return builder;
	}

	/// <summary>
	/// Registra serviços da aplicação
	/// </summary>
	private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<IAuthService, AuthService>();
		builder.Services.AddSingleton<IStorageService, StorageService>();
		builder.Services.AddSingleton<IApiService, ApiService>();
		builder.Services.AddSingleton<IPanelService, PanelService>();

		return builder;
	}

	/// <summary>
	/// Registra páginas XAML
	/// </summary>
	private static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<PanelPage>();
		builder.Services.AddSingleton<SettingsPage>();
		builder.Services.AddSingleton<AppShell>();

		return builder;
	}

	/// <summary>
	/// Registra ViewModels
	/// </summary>
	private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<LoginViewModel>();
		builder.Services.AddSingleton<PanelViewModel>();
		builder.Services.AddSingleton<SettingsViewModel>();

		return builder;
	}
}
