using IG.App.ViewModel;

namespace IG.App;

public partial class MainPage : ContentPage
{

	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    async void OnButtonClicked(object sender, EventArgs args)
    {
        await DisplayAlert("Info", "New HashForm application, 2022", "OK");
    }



}

