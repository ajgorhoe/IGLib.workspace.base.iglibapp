using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
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
#if DEBUG
        IsDebugMode = true;
#else
        IsDebugMode = false;
#endif
        IsDebugInfoVisible = IsDebugMode ? true : false;

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


    private int _numEtries = 0;

    /// <summary>Counts total number of DragEnter / DragLeave events, for debug purposes.</summary>
    public int NumEntries
    {
        get => _numEtries;
        set 
        {
            _numEtries = value;
            OnPropertyChanged(nameof(NumEntries));
        }
    }

    public bool IsDebugMode { get; init; }

    private bool _isDebugInfoVisible = false;

    /// <summary>If true then debug information will be visible on the view.
    /// This is prevented when not in Debug mode.</summary>
    public bool IsDebugInfoVisible
    {
        get => _isDebugInfoVisible;
        set {
            if (value != _isDebugInfoVisible)
            {
                if (IsDebugMode)
                {
                    _isDebugInfoVisible = value;
                    OnPropertyChanged(nameof(IsDebugInfoVisible));
                }
                else
                {
                    IsDebugInfoVisible = false;
                }
            }
        }
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

                if (!string.IsNullOrEmpty(value))
                {
                    if (IsFileHashing && File.Exists(value))
                    {
                        IsInputDataSufficient = true;
                    }
                }
                if (IsFileHashing)
                {
                    TextToHash = GetFilePreview(FilePath);
                }
                OnPropertyChanged(nameof(FilePath));
                OnPropertyChanged(nameof(DirectoryPath));
            }
        }
    }

    public string DirectoryPath
    {
        get => string.IsNullOrEmpty(FilePath) ? null : Path.GetDirectoryName(FilePath);
    }

    private bool _isFileHashing = true;

    public bool IsFileHashing
    {
        get => _isFileHashing;
        set
        {
            if (value != _isFileHashing)
            {
                _isFileHashing = value;
                OnPropertyChanged(nameof(IsFileHashing));
                OnPropertyChanged(nameof(IsTextHashing));
                OnPropertyChanged(nameof(TextEntryLabelText));
                if (_isFileHashing)
                {
                    TextToHash = GetFilePreview(FilePath);
                } else
                {
                    TextToHash = null; // LastTextToHashWhenTextHashing;
                }
            }
        }
    }


    private int _maxLines = 40;
    private int _maxCharacters = 2000;
    protected string GetFilePreview(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }
        if (!File.Exists(filePath))
        {
            return null;
        }
        bool isProbablyTextFile = false;
        // Read part of the file and sew whether the file could be a text file;
        // This is difficult to establish out in general, but we will base our estimation on the number of newlines:
        int numChecked = _maxCharacters;

        using (FileStream fileStream = File.OpenRead(filePath))
        {
            var rawData = new byte[numChecked];
            var rawLength = fileStream.Read(rawData, 0, rawData.Length);
            // Count newlines: 
            int numNewLines = 0;
            for (int i = 0; i < rawLength; ++i)
            {
                if (rawData[i] == '\n')
                {
                    ++numNewLines;
                }
            }
            if (numNewLines > rawLength / 300)
            {
                isProbablyTextFile |= true;
            }
            if (!isProbablyTextFile)
            {
                string str = Encoding.Default.GetString(rawData, 0, rawLength);
                if (rawLength < _maxLines)
                {
                    return str;
                }
                else
                {
                    return str + Environment.NewLine + "...";
                }

            }
        }
        // Probably a string file, try to read a certain number of lines: 
        StringBuilder sb = new StringBuilder();
        int numLinesRead = 0;
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null && numLinesRead <_maxLines)
            {
                ++numLinesRead;
                sb.AppendLine(line);
            }
        }
        if (numLinesRead >= _maxLines)
        {
            sb.AppendLine(Environment.NewLine + "...");
        }
        return sb.ToString();
    }

    public bool IsTextHashing
    {
        get => !IsFileHashing;
        set { IsFileHashing = !value; }
    }

    private bool _calculateMD5 = true;

    public bool CalculateMD5
    {
        get => _calculateMD5;
        set
        {
            if (value != _calculateMD5)
            {
                _calculateMD5 = value;
                OnPropertyChanged(nameof(CalculateMD5));
            }
        }
    }



    private bool _calculateSHA1 = true;

    public bool CalculateSHA1
    {
        get => _calculateSHA1;
        set
        {
            if (value != _calculateSHA1)
            {
                _calculateSHA1 = value;
                OnPropertyChanged(nameof(CalculateSHA1));
            }
        }
    }

    private bool _calculateSHA256 = true;

    public bool CalculateSHA256
    {
        get => _calculateSHA256;
        set
        {
            if (value != _calculateSHA256)
            {
                _calculateSHA256 = value;
                OnPropertyChanged(nameof(CalculateSHA256));
                if (value == true)
                    IsHashesOutdated = true;
            }
        }
    }

    private bool _calculateSHA512 = false;

    public bool CalculateSHA512
    {
        get => _calculateSHA512;
        set
        {
            if (value != _calculateSHA512)
            {
                _calculateSHA512 = value;
                OnPropertyChanged(nameof(CalculateSHA512));
            }
        }
    }


    private bool _isHashesOutdated = false;

    public bool IsHashesOutdated
    {
        get => _isHashesOutdated;
        set
        {
            if (value != _isHashesOutdated)
            {
                _isHashesOutdated = value;
                OnPropertyChanged(nameof(IsHashesOutdated));
            }
        }
    }

    private bool _isCalculating = false;

    /// <summary>Wheen changed to true, this also triggers actual calculation of hash values!</summary>
    public bool IsCalculating
    {
        get => _isCalculating;
        set
        {
            if (value != _isCalculating)
            {
                if (value == true && !IsHashesOutdated)
                {
                    // Hashes are not outdated, no need to do calculation!
                    return;
                }
                _isCalculating = value;
                OnPropertyChanged(nameof(IsCalculating));
                // ToDo: trigger calculation!
            }
        }
    }


    private string _hashValueMD5 = null;

    public string HashValueMD5
    {
        get => _hashValueMD5;
        set
        {
            if (value != _hashValueMD5)
            {
                _hashValueMD5 = value;
                OnPropertyChanged(nameof(HashValueMD5));
            }
        }
    }

    private string _hashValueSHA1 = null;

    public string HashValueSHA1
    {
        get => _hashValueSHA1;
        set
        {
            if (value != _hashValueSHA1)
            {
                _hashValueSHA1 = value;
                OnPropertyChanged(nameof(HashValueSHA1));
            }
        }
    }


    private string _hashValueSHA256 = null;

    public string HashValueSHA256
    {
        get => _hashValueSHA256;
        set
        {
            if (value != _hashValueSHA256)
            {
                _hashValueSHA256 = value;
                OnPropertyChanged(nameof(HashValueSHA256));
            }
        }
    }

    private string _hashValueSHA512 = null;

    public string HashValueSHA512
    {
        get => _hashValueSHA512;
        set
        {
            if (value != _hashValueSHA512)
            {
                _hashValueSHA512 = value;
                OnPropertyChanged(nameof(HashValueSHA512));
            }
        }
    }

    protected virtual void InvalidateHashValues()
    {
        HashValueMD5 = null;
        HashValueSHA1 = null;
        HashValueSHA256 = null;
        HashValueSHA512 = null;
        IsHashesOutdated = true;
    }

    private bool _isInputDataSufficient = false;

    public bool IsInputDataSufficient
    {
        get => _isInputDataSufficient;
        set
        {
            if (value != _isInputDataSufficient)
            {
                if (value == true)
                {
                    // Double check that assignment of true is correct:
                    if (IsFileHashing)
                    {
                        if (string.IsNullOrEmpty(FilePath))
                            return;
                        if (!File.Exists(FilePath))
                            return;
                    } else if (IsTextHashing)
                    {
                        if (string.IsNullOrEmpty(TextToHash))
                            return;
                    }
                }
                _isInputDataSufficient = value;
                OnPropertyChanged(nameof(IsInputDataSufficient));
                if (value)
                {
                    IsHashesOutdated = true;
                }
            }

        }
    }

    // private bool _isCalculationButtonsEnabled = false;

    public bool IsCalculationButtonsEnabled
    {
        get => IsInputDataSufficient && IsHashesOutdated && !IsCalculating;
    }

    private string _textEntryLabelText = null;

    public string TextEntryLabelText
    {
        get => IsFileHashing ? "File Content Preview:" : "Enter Hashed Text Below:";
    }

    private string _textToHash = null;


    public string TextToHash
    {
        get => _textToHash;
        set
        {
            if (value != _textToHash)
            {
                _textToHash = value;
                OnPropertyChanged(nameof(TextToHash));
                if (IsTextHashing)
                {
                    InvalidateHashValues();
                    IsInputDataSufficient = !string.IsNullOrEmpty(_textToHash);
                    LastTextToHashWhenTextHashing = _textToHash;
                }

            }
        }
    }

    protected string LastTextToHashWhenFileHashing { get; set; } = null;

    protected string LastTextToHashWhenTextHashing { get; set; } = null;

    protected void OnFilePropertiesChanged()
    {
        if (IsFileHashing)
        {
            // ToDo: put file preview into TextToHash!
        } else
        {

        }
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
