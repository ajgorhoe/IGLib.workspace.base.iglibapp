using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IG.Crypto;
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

    public MainViewModel(HashCalculatorBase hashCalculator = null)
    {
        HashCalculator = hashCalculator;

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
            canExecute: () => true
        );

    }


    private HashCalculatorBase _hashCalculator = null;


    // ToDo: replace type with interface!
    public HashCalculatorBase HashCalculator
    {
        get
        {
            if (_hashCalculator == null)
                _hashCalculator = new HashCalculator();
            return _hashCalculator;
        }
        set
        {
            if (value != _hashCalculator)
                _hashCalculator = value;
        }
    }


    /// <summary>Calculate hash function of specific type on the current input from this class.</summary>
    /// <param name="hashType">Typ of the hash function applied.</param>
    /// <returns></returns>
    protected async Task<string> CalculateHashAsync(string hashType)
    {
        if (IsTextHashing)
        {
            if (string.IsNullOrEmpty(this.TextToHash))
                throw new InvalidOperationException("Hash computation: Text to be hashed is empty.");
            var hashText = async () =>
            {
                await Task.FromResult(true); // dummy await; ToDo: replace with async method
                return HashCalculator.CalculateTextHashString(hashType, TextToHash);
            };
            return await hashText();
        } else if (IsFileHashing)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                throw new ArgumentException("Hash computation: Path to file to be hashed is not specified.");
            }
            if (!File.Exists(FilePath))
            {
                throw new InvalidOperationException("Hash computation: File to be hashed does not exist.");
            }
            var hashFile = async () => {
                await Task.FromResult(true); // dummy await; ToDo: replace with async method
                return HashCalculator.CalculateFileHashString(hashType, FilePath);
            };
            return await hashFile();
        } else
        {
            throw new InvalidOperationException("Input for calculation of hash function is not specified.");
        }
    }

    public async void CalculateMissingHashesAsync()
    {
        // ToDo: fix IsHashesOutdated, then remove "|| true"" 
        if (IsHashesOutdated)
        {
            if (IsInputDataSufficient)
            {
                try
                {
                    IsCalculating = true;
                    if (CalculateMD5 && string.IsNullOrEmpty(HashValueMD5))
                    {
                        string hashValue = await CalculateHashAsync(HashConst.MD5Hash);
                        HashValueMD5 = hashValue;
                    }
                    if (CalculateSHA1 && string.IsNullOrEmpty(HashValueSHA1))
                    {
                        string hashValue = await CalculateHashAsync(HashConst.SHA1Hash);
                        HashValueSHA1 = hashValue;
                    }
                    if (CalculateSHA256 && string.IsNullOrEmpty(HashValueSHA256))
                    {
                        string hashValue = await CalculateHashAsync(HashConst.SHA256Hash);
                        HashValueSHA256 = hashValue;
                    }
                    if (CalculateSHA512 && string.IsNullOrEmpty(HashValueSHA512))
                    {
                        string hashValue = await CalculateHashAsync(HashConst.SHA512Hash);
                        HashValueSHA512 = hashValue;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    IsCalculating = false;
                    RefreshHashesOutdatedProperty(false);
                }
            }
            else
            {
                throw new InvalidOperationException("Input data for calculation of hashes is not ready.");
            }
        }
    }



    private string _droppedTextValue = null;

    public string DroppedTextValue
    {
        get => _droppedTextValue;
        set
        {
            if (value != _droppedTextValue)
            {
                _droppedTextValue = value;
                OnPropertyChanged(nameof(DroppedTextValue));
            }
        }
    }

    private int _numEtries = 0;

    /// <summary>Counts total number of DragEnter / DragLeave events, for debug purposes.</summary>
    public int NumEntries
    {
        get => _numEtries;
        set
        {
            if (value != _numEtries)
            {
                _numEtries = value;
                OnPropertyChanged(nameof(NumEntries));
            }
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


    private string _filePath;
    public string FilePath
    {
        get => _filePath;
        set
        {
            if (_filePath != value)
            {
                _filePath = value;

                //if (!string.IsNullOrEmpty(value))
                //{
                //    if (IsFileHashing && File.Exists(value))
                //    {
                //        IsInputDataSufficient = true;
                //    }
                //}
                if (IsFileHashing)
                {
                    InvalidateHashValues();
                    TextToHash = GetFilePreview(_filePath);
                    FileInfo fi = new FileInfo(_filePath);
                    {
                        IsInputDataSufficient = !string.IsNullOrEmpty(FilePath) 
                            && File.Exists(FilePath) && fi.Length > 0;
                    }
                    OnPropertyChanged(nameof(FilePath));
                    OnPropertyChanged(nameof(DirectoryPath));
                    RefreshHashesOutdatedProperty();
                }
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
                RefreshHashesOutdatedProperty();
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
            while ((line = reader.ReadLine()) != null && numLinesRead < _maxLines)
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

    private bool _calculateHashesAutomatically = false;

    public bool CalculateHashesAutomatically
    {
        get => _calculateHashesAutomatically;
        set
        {
            if (value != _calculateHashesAutomatically)
            {
                _calculateHashesAutomatically = value;
                OnPropertyChanged(nameof(CalculateHashesAutomatically));
                RefreshHashesOutdatedProperty();  // to eventually trigger automatic calculation of hashes
            }
        }
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
                RefreshHashesOutdatedProperty();
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
                RefreshHashesOutdatedProperty();
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
                RefreshHashesOutdatedProperty();
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
                RefreshHashesOutdatedProperty();
            }
        }
    }





    /// <summary>Recalculates the <see cref="IsHashesOutdated"/> property, and performs any automatic 
    /// tasks dependent on this property.</summary>
    /// <param name="performAutomaticCalculations">If false then automatic calculations ae not performed.
    /// Default is true.</param>
    public bool RefreshHashesOutdatedProperty(bool performAutomaticCalculations =  true)
    {
        bool isOutdated = GetHashesOutdated();  // this will also call OnPropertyChanged(nameof(IsHashesOutdated));
        if (isOutdated)
        {
            if (CalculateHashesAutomatically && performAutomaticCalculations && !IsCalculating)
            {
                try
                {
                    CalculateMissingHashesAsync();
                }
                catch
                { }
                finally
                {
                    isOutdated = GetHashesOutdated();
                }
            }
        }
        return isOutdated;
    }


    /// <summary>Computes the true value for <see cref="IsHashesOutdated"/>.</summary>
    /// <returns></returns>
    protected bool GetHashesOutdated()
    {
        bool isOutdated = false;
        if (IsInputDataSufficient)
        {
            if (CalculateMD5 && string.IsNullOrEmpty(HashValueMD5))
            {
                isOutdated = true;
            }
            else if (CalculateSHA1 && string.IsNullOrEmpty(HashValueSHA1))
            {
                isOutdated = true;
            }
            else if (CalculateSHA256 && string.IsNullOrEmpty(HashValueSHA256))
            {
                isOutdated = true;
            }
            else if (CalculateSHA512 && string.IsNullOrEmpty(HashValueSHA512))
            {
                isOutdated = true;
            }
        }
        if (isOutdated != _isHashesOutdated)
        {
            _isHashesOutdated = isOutdated;
            OnPropertyChanged(nameof(IsHashesOutdated));
        }
        return isOutdated;
    }

    private bool _isHashesOutdated = false;

    /// <summary>Whether the currently kept hash values are outdated and need recalculation.</summary>
    public bool IsHashesOutdated
    {
        get
        {
            return GetHashesOutdated();
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

    /// <summary>This method was added in order to alleviate the issue with labels in UI bot refreshing on 
    /// updating values of <see cref="HashValueMD5"/>, <see cref="HashValueSHA1"/>, etc. Calling this method
    /// did not help (a possible bug?), but it helped when labels were changed to read-only text entries.</summary>
    public void RefreshHashvaluesInUi()
    {
        OnPropertyChanged(nameof(HashValueMD5));
        OnPropertyChanged(nameof(HashValueSHA1));
        OnPropertyChanged(nameof(HashValueSHA256));
        OnPropertyChanged(nameof(HashValueSHA512));
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
            }
            OnPropertyChanged(nameof(HashValueMD5));
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
            }
            OnPropertyChanged(nameof(HashValueSHA1));
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
            }
            OnPropertyChanged(nameof(HashValueSHA256));
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
            }
            OnPropertyChanged(nameof(HashValueSHA512));
        }
    }

    public virtual void InvalidateHashValues()
    {
        HashValueMD5 = null;
        HashValueSHA1 = null;
        HashValueSHA256 = null;
        HashValueSHA512 = null;
        RefreshHashesOutdatedProperty();
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
                RefreshHashesOutdatedProperty();
            }

        }
    }

    
    public bool IsCalculationButtonsEnabled
    {
        get => IsInputDataSufficient && IsHashesOutdated && !IsCalculating;
    }


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
                RefreshHashesOutdatedProperty();
            }
        }
    }

    protected string LastTextToHashWhenFileHashing { get; set; } = null;

    protected string LastTextToHashWhenTextHashing { get; set; } = null;

    //protected void OnFilePropertiesChanged()
    //{
    //    if (IsFileHashing)
    //    {
    //        // ToDo: put file preview into TextToHash!
    //    } else
    //    {

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
