using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NHSE.Core;
using NHSE.Sprites;
using NHSE.macOS.Services;
using NHSE.macOS.ViewModels;
using NHSE.macOS.Views;
using System;
using System.IO;

namespace NHSE.macOS;

public partial class App : Application
{
    public static new App Current => (App)Application.Current!;
    
    public ApplicationSettings Settings { get; private set; } = new();
    public string SettingsPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NHSE", "settings.json");

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        LoadSettings();
        InitializeServices();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainViewModel = new MainWindowViewModel();
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            
            desktop.ShutdownRequested += OnShutdownRequested;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void InitializeServices()
    {
        // Initialize game strings based on settings
        var lang = Settings.Language;
        GameInfo.SetLanguage2Char(GameLanguage.GetLanguageIndex(lang));
        
        // Initialize sprites
        Task.Run(() =>
        {
            ItemSprite.Initialize(GameInfo.GetStrings("en").itemlist);
        });
    }

    private void LoadSettings()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                var json = File.ReadAllText(SettingsPath);
                Settings = System.Text.Json.JsonSerializer.Deserialize<ApplicationSettings>(json) ?? new ApplicationSettings();
            }
        }
        catch
        {
            Settings = new ApplicationSettings();
        }
    }

    public void SaveSettings()
    {
        try
        {
            var dir = Path.GetDirectoryName(SettingsPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
                
            var json = System.Text.Json.JsonSerializer.Serialize(Settings, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save settings: {ex.Message}");
        }
    }

    private void OnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        SaveSettings();
    }
}

public class ApplicationSettings
{
    public string Language { get; set; } = "en";
    public string LastFilePath { get; set; } = "";
    public SystemColorMode DarkMode { get; set; } = SystemColorMode.System;
}

public enum SystemColorMode
{
    System,
    Classic,
    Dark
}
