using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NHSE.Core;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NHSE.macOS.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private HorizonSave? _saveFile;

    [ObservableProperty]
    private EditorWindowViewModel? _editorViewModel;

    [ObservableProperty]
    private string _windowTitle = "NHSE - New Horizons Save Editor";

    [ObservableProperty]
    private bool _hasSaveFile;

    [ObservableProperty]
    private ObservableCollection<string> _recentFiles = new();

    [ObservableProperty]
    private bool _isEditorVisible;

    [ObservableProperty]
    private bool _isLauncherVisible = true;

    public MainWindowViewModel()
    {
        LoadRecentFiles();
    }

    [RelayCommand]
    private async Task OpenSaveFileAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            // Open file dialog
            var dialog = new Avalonia.Controls.OpenFileDialog
            {
                Title = "Open main.dat...",
                AllowMultiple = false
            };
            dialog.Filters.Add(new Avalonia.Controls.FileDialogFilter 
            { 
                Name = "New Horizons Save File (main.dat)", 
                Extensions = { "dat" } 
            });

            var window = Avalonia.Application.Current?.ApplicationLifetime 
                is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop 
                ? desktop.MainWindow : null;

            if (window != null)
            {
                var result = await dialog.ShowAsync(window);
                if (result?.Length > 0 && !string.IsNullOrEmpty(result[0]))
                {
                    await LoadSaveFileAsync(result[0]);
                }
            }
        }
        catch (Exception ex)
        {
            SetError($"Failed to open save file: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task LoadSaveFileAsync(string path)
    {
        try
        {
            IsLoading = true;
            ClearError();

            if (!File.Exists(path))
            {
                SetError("File does not exist.");
                return;
            }

            var dir = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
            {
                SetError("Directory does not exist.");
                return;
            }

            var file = SaveFileLoader.TryGetSaveFile(path, out var sav);
            if (!file || sav == null)
            {
                SetError("Invalid save file.");
                return;
            }

            SaveFile = sav;
            HasSaveFile = true;
            WindowTitle = sav.GetSaveTitle("NHSE");
            
            // Create editor view model
            EditorViewModel = new EditorWindowViewModel(sav);
            
            // Switch to editor view
            IsLauncherVisible = false;
            IsEditorVisible = true;

            // Add to recent files
            AddRecentFile(path);
            
            // Save last file path
            App.Current.Settings.LastFilePath = path;
            App.Current.SaveSettings();
        }
        catch (Exception ex)
        {
            SetError($"Failed to load save file: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void OpenRecentFile(string path)
    {
        _ = LoadSaveFileAsync(path);
    }

    [RelayCommand]
    private void OpenSettings()
    {
        // Open settings dialog
    }

    [RelayCommand]
    private async Task OpenDecryptedSaveAsync()
    {
        var dialog = new Avalonia.Controls.OpenFileDialog
        {
            Title = "Open main.dat...",
            AllowMultiple = false
        };
        dialog.Filters.Add(new Avalonia.Controls.FileDialogFilter 
        { 
            Name = "New Horizons Save File (main.dat)", 
            Extensions = { "dat" } 
        });

        var window = Avalonia.Application.Current?.ApplicationLifetime 
            is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop 
            ? desktop.MainWindow : null;

        if (window != null)
        {
            var result = await dialog.ShowAsync(window);
            if (result?.Length > 0 && !string.IsNullOrEmpty(result[0]))
            {
                await LoadDecryptedSaveAsync(result[0]);
            }
        }
    }

    private async Task LoadDecryptedSaveAsync(string path)
    {
        if (EditorViewModel == null) return;
        await EditorViewModel.LoadDecryptedFromPathAsync(path);
    }

    private void LoadRecentFiles()
    {
        // Load from settings
        RecentFiles.Clear();
    }

    private void AddRecentFile(string path)
    {
        if (RecentFiles.Contains(path))
        {
            RecentFiles.Remove(path);
        }
        RecentFiles.Insert(0, path);
        if (RecentFiles.Count > 10)
        {
            RecentFiles.RemoveAt(RecentFiles.Count - 1);
        }
    }

    partial void OnSaveFileChanged(HorizonSave? value)
    {
        HasSaveFile = value != null;
    }
}
