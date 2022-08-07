using IG.App.ViewModel;
using System.Diagnostics;

namespace IG.App;

public partial class MainPage : ContentPage
{

    public MainPage(MainViewModel vm) : base()
    {
        InitializeComponent();
        BindingContext = vm;
        ViewModel = vm;
    }


    /// <summary>Referencr to the corresponding ViewModel.</summary>
    MainViewModel ViewModel { get; init; }


    async void OnButtonBrowseClicked(object sender, EventArgs args)
    {
        PickOptions options = new()
        {
            PickerTitle = "Please select the file to be hashed",
            
        };
        var result = await FilePicker.Default.PickAsync(options);
        ViewModel.FilePath = result.FullPath;
        
    }


    async void OnButtonClicked(object sender, EventArgs args)
    {
        await DisplayAlert("Info", "New HashForm application, 2022", "OK");
    }


    async void OnButtonHelpClicked(object sender, EventArgs args)
    {
        await DisplayAlert("Help", "Help is not implemented yet", "OK");
    }

    async void OnButtonAboutClicked(object sender, EventArgs args)
    {
        await DisplayAlert("Info", "New HashForm application, 2022", "OK");
    }

    async void OnButtonTestClicked(object sender, EventArgs args)
    {
        await DisplayAlert("Info", "New HashForm application, 2022", "OK");
    }


}

