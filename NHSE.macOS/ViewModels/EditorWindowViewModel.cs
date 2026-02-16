using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NHSE.Core;
using NHSE.Injection;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NHSE.macOS.ViewModels;

public partial class EditorWindowViewModel : ViewModelBase
{
    private readonly HorizonSave _sav;

    [ObservableProperty]
    private int _currentPlayerIndex = -1;

    [ObservableProperty]
    private ObservableCollection<string> _playerNames = new();

    [ObservableProperty]
    private MainSaveViewModel _mainSaveViewModel;

    [ObservableProperty]
    private PlayerViewModel? _currentPlayerViewModel;

    [ObservableProperty]
    private VillagerEditorViewModel _villagerViewModel;

    [ObservableProperty]
    private int _selectedTabIndex;

    [ObservableProperty]
    private ObservableCollection<string> _languages = new();

    [ObservableProperty]
    private int _selectedLanguageIndex;

    public HorizonSave SaveFile => _sav;

    public EditorWindowViewModel(HorizonSave saveFile)
    {
        _sav = saveFile;
        MainSaveViewModel = new MainSaveViewModel(saveFile.Main);
        VillagerViewModel = new VillagerEditorViewModel(saveFile.Main.GetVillagers(), saveFile.Players[0].Personal, saveFile);
        
        LoadPlayers();
        LoadLanguages();
    }

    private void LoadPlayers()
    {
        PlayerNames.Clear();
        foreach (var player in _sav.Players)
        {
            PlayerNames.Add(player.DirectoryName);
        }
        
        if (PlayerNames.Count > 0)
        {
            CurrentPlayerIndex = 0;
        }
    }

    private void LoadLanguages()
    {
        Languages.Clear();
        for (int i = 0; i < GameLanguage.LanguageCount; i++)
        {
            Languages.Add(GameLanguage.GetLanguageName(i));
        }
        
        var lang = App.Current.Settings.Language;
        var index = GameLanguage.GetLanguageIndex(lang);
        SelectedLanguageIndex = index;
    }

    partial void OnCurrentPlayerIndexChanged(int value)
    {
        if (value >= 0 && value < _sav.Players.Count)
        {
            SaveCurrentPlayer();
            CurrentPlayerViewModel = new PlayerViewModel(_sav.Players[value], _sav.Main, value);
        }
    }

    partial void OnSelectedLanguageIndexChanged(int value)
    {
        if ((uint)value >= GameLanguage.LanguageCount)
            return;
        
        var lang = GameInfo.SetLanguage2Char(value);
        App.Current.Settings.Language = lang;
        
        Task.Run(() =>
        {
            ItemSprite.Initialize(GameInfo.GetStrings("en").itemlist);
        });
    }

    [RelayCommand]
    private void SaveAll()
    {
        try
        {
            SaveCurrentPlayer();
            VillagerViewModel.Save();
            _sav.Main.SetVillagers(VillagerViewModel.Villagers);
            MainSaveViewModel.Save();
            
            _sav.Save((uint)DateTime.Now.Ticks);
            
            ShowSuccess("Save Data Export Success");
        }
        catch (Exception ex)
        {
            ShowError($"Save Data Export Fail: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task DumpDecryptedAsync()
    {
        var dialog = new Avalonia.Controls.SaveFileDialog
        {
            Title = "Select folder to dump decrypted files"
        };

        var window = Avalonia.Application.Current?.ApplicationLifetime 
            is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop 
            ? desktop.MainWindow : null;

        if (window != null)
        {
            var result = await dialog.ShowAsync(window);
            if (!string.IsNullOrEmpty(result))
            {
                var dir = Path.GetDirectoryName(result);
                if (!string.IsNullOrEmpty(dir))
                {
                    _sav.Dump(dir);
                    ShowSuccess("Decrypted files dumped successfully.");
                }
            }
        }
    }

    public async Task LoadDecryptedFromPathAsync(string main)
    {
        var dir = Path.GetDirectoryName(main);
        if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
        {
            ShowError("Import directory does not exist.");
            return;
        }

        try
        {
            _sav.Load(dir);
            ReloadAll();
            ShowSuccess("Decrypted save file loaded successfully.");
        }
        catch (Exception ex)
        {
            ShowError($"Failed to load decrypted save: {ex.Message}");
        }
    }

    [RelayCommand]
    private void VerifyHashes()
    {
        var result = _sav.GetInvalidHashes().ToArray();
        if (result.Length == 0)
        {
            ShowSuccess("Save data hashes are valid.");
            return;
        }

        var lines = string.Join(Environment.NewLine, result.Select(z => z.ToString()));
        // Copy to clipboard or show in dialog
    }

    [RelayCommand]
    private void OpenRamEditor()
    {
        // Open RAM editor window
    }

    [RelayCommand]
    private void OpenItemImages()
    {
        // Open item images window
    }

    [RelayCommand]
    private void ChangeTheme(string themeName)
    {
        if (Enum.TryParse<SystemColorMode>(themeName, out var theme))
        {
            App.Current.Settings.DarkMode = theme;
            App.Current.SaveSettings();
            // Request restart
        }
    }

    private void ReloadAll()
    {
        VillagerViewModel.Villagers = _sav.Main.GetVillagers();
        VillagerViewModel.Origin = _sav.Players[0].Personal;
        VillagerViewModel.Reload();
        LoadPlayers();
    }

    private void SaveCurrentPlayer()
    {
        CurrentPlayerViewModel?.Save();
    }

    private void ShowSuccess(string message)
    {
        // Show success notification
    }

    private void ShowError(string message)
    {
        SetError(message);
    }
}
