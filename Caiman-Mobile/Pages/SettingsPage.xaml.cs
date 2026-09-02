namespace Caiman_Panel;

public partial class SettingsPage : ContentPage
{
	private readonly SettingsViewModel _viewModel;

	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;

		SetPlatformInfo();
	}

	private void SetPlatformInfo()
	{
		var platform = DeviceInfo.Platform.ToString();
		var deviceName = DeviceInfo.Name;

		PlatformLabel.Text = platform;
		DeviceLabel.Text = deviceName;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.OnAppearing();
	}
}
