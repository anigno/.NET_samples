using log4net;

namespace TestMaui;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		BindingContext=new MainPageViewModel();
	}

	private void AddClicked(object sender, EventArgs e)
	{
		count++;
	}
	private void UpdateClicked(object sender, EventArgs e)
	{
		count++;
	}
	private void DeleteClicked(object sender, EventArgs e)
	{
		count++;
	}
	private void PasswordSelected(object sender, EventArgs e)
	{
		count++;
	}
}

