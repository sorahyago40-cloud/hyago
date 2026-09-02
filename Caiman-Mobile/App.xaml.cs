namespace Caiman_Panel;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

	protected override void OnStart()
	{
		base.OnStart();
		// Executar ao iniciar app
	}

	protected override void OnResume()
	{
		base.OnResume();
		// Executar ao resumir app
	}

	protected override void OnSleep()
	{
		base.OnSleep();
		// Executar ao suspender app
	}
}
