using IG.App.ViewModel;
using System.Diagnostics;
using Windows.ApplicationModel.Background;

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
        try
        {
            ViewModel.FilePath = result.FullPath;
        }
        catch (Exception)
        {
        }
    }


    /// <summary>Just to prevent compiiler warinings in async methods that do very little.</summary>
    /// <param name="callItself"></param>
    /// <returns></returns>
    private async Task<bool> DummyAsync(bool callItself = false)
    {
        if (callItself)
        {
            return await DummyAsync(false);
        }
        return true;
    }

    Brush _fileEntryInitialBackground = Color.FromRgb(255, 255, 255);

    string _storedFilePath = null;



    /// <summary>Indicates visually on the <see cref="FileEntry"/> control that something is dragged over,
    /// by changing background color.
    /// <remarks>This should be implemented via VievModel property binding in the future.</remarks></summary>
    void OnDragOverFileEntry(object sender, DragEventArgs eventArgs)
    {
        try
        {
            if (ViewModel.NumEntries == 0)
            {
                ++ViewModel.NumEntries;
                _storedFilePath = ViewModel.FilePath;
                ViewModel.FilePath = "Drag Enter.";
                // _fileEntryInitialBackground = this.FileEntry.Background;
                eventArgs.AcceptedOperation = DataPackageOperation.Copy;
                // this.FileEntry.Background = Color.Parse("Orange");
            }
        }
        catch { }
        // await DummyAsync();
    }

    void OnDragLeaveFileEntry(object sender, DragEventArgs eventArgs)
    {
        try
        {
            ViewModel.NumEntries = 0;
            if (ViewModel.NumEntries == 0)
            {
                ViewModel.FilePath = _storedFilePath;
                _storedFilePath = null;
                //this.FileEntry.Background = _fileEntryInitialBackground;
            }
        }
        catch { }
        // await DummyAsync();
    }

    async void OnDropFileEntry(object sender, DropEventArgs eventArgs)
    {
        this.FileEntry.Background = _fileEntryInitialBackground;
        this.FileEntry.Text = await eventArgs.Data.GetTextAsync();
        await DummyAsync();
    }


    //async void OnButtonClicked(object sender, EventArgs args)
    //{
    //    await DisplayAlert("Info", "New HashForm application, 2022", "OK");
    //}


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

