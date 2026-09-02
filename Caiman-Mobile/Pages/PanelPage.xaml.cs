namespace Caiman_Panel;

public partial class PanelPage : ContentPage
{
	private readonly PanelViewModel _viewModel;

	public PanelPage(PanelViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.OnAppearing();
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		_viewModel.OnDisappearing();
	}
}
