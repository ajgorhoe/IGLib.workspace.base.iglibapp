using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace IG.App.ViewModel;

public partial class MainViewModel : 
    ObservableObject
    //,
    //INotifyPropertyChanged
{


    // Defined in ObservableObject:
    // public event PropertyChangedEventHandler PropertyChanged;
    // public void OnPropertyChanged(striing)


    // event PropertyChangedEventHandler PropertyChanged;

    public MainViewModel()
    {
        LaunchInfoDialogCommand = new Command(
            execute: () =>
            {
                // Do the stuff...
                // ToDo: implement command body!
                // 
                ((Command)LaunchInfoDialogCommand).ChangeCanExecute();
            },
            // ToDo: replace criterion below.
            canExecute: () => Number < 20000.0
        );

    }
    public ICommand LaunchInfoDialogCommand { get; private set; }

    private int Number { get; set; } = 1;


    private string _filePath;
    public string FilePath
    {
        get => _filePath;
        set
        {
            if (_filePath != value)
            {
                _filePath = value;

                OnPropertyChanged(nameof(FilePath));
                OnPropertyChanged(nameof(DirectoryPath));
            }
        }
    }

    public string DirectoryPath
    {
        get =>string.IsNullOrEmpty(FilePath) ? null: Path.GetDirectoryName(FilePath);
    }





    //double number = 1;

    //public double Number
    //{
    //    get
    //    {
    //        return number;
    //    }
    //    set
    //    {
    //        if (number != value)
    //        {
    //            number = value;
    //            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Number"));
    //        }
    //    }
    //}





    //IConnectivity connectivity;
    //public MainViewModel(IConnectivity connectivity)
    //{
    //    // Items = new ObservableCollection<string>();
    //    this.connectivity = connectivity;
    //}

    //[ObservableProperty]
    //ObservableCollection<string> items;

    //[ObservableProperty]
    //string text;

    //[RelayCommand]
    //async Task Add()
    //{
    //    if (string.IsNullOrWhiteSpace(Text))
    //        return;

    //    if(connectivity.NetworkAccess != NetworkAccess.Internet)
    //    {
    //        await Shell.Current.DisplayAlert("Uh Oh!", "No Internet", "OK");
    //        return;
    //    }

    //    Items.Add(Text);
    //    // add our item
    //    Text = string.Empty;
    //}

    //[RelayCommand]
    //void Delete(string s)
    //{
    //    if(Items.Contains(s))
    //    {
    //        Items.Remove(s);
    //    }
    //}

    //[RelayCommand]
    //async Task Tap(string s)
    //{
    //    await Shell.Current.GoToAsync($"{nameof(DetailPage)}?Text={s}");
    //}

}
