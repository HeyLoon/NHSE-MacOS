using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NHSE.Core;
using System;
using System.IO;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using System.Diagnostics.CodeAnalysis;

namespace NHSE.macOS.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        SetupUI();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SetupUI()
    {
        Title = "NHSE - Animal Crossing: New Horizons Save Editor";
        Width = 800;
        Height = 600;

        var grid = new Grid
        {
            RowDefinitions = new RowDefinitions("Auto,*")
        };

        // Menu
        var menu = new Menu();
        var fileMenu = new MenuItem { Header = "File" };
        var openItem = new MenuItem { Header = "Open..." };
        openItem.Click += async (s, e) => await OpenSaveFileAsync();
        fileMenu.Items.Add(openItem);
        
        fileMenu.Items.Add(new Separator());
        
        var exitItem = new MenuItem { Header = "Exit" };
        exitItem.Click += (s, e) => Close();
        fileMenu.Items.Add(exitItem);
        
        menu.Items.Add(fileMenu);
        
        Grid.SetRow(menu, 0);
        grid.Children.Add(menu);

        // Main content
        var welcomePanel = new StackPanel 
        { 
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
        };
        
        var titleBlock = new TextBlock
        {
            Text = "NHSE - Animal Crossing: New Horizons Save Editor",
            FontSize = 24,
            Margin = new Avalonia.Thickness(0, 0, 0, 20)
        };
        welcomePanel.Children.Add(titleBlock);
        
        var subtitleBlock = new TextBlock
        {
            Text = "Click File > Open to load a save file",
            FontSize = 14,
            Opacity = 0.7
        };
        welcomePanel.Children.Add(subtitleBlock);

        Grid.SetRow(welcomePanel, 1);
        grid.Children.Add(welcomePanel);

        Content = grid;

        // Handle drag and drop
        AddHandler(DragDrop.DropEvent, OnDrop);
        AddHandler(DragDrop.DragOverEvent, OnDragOver);
    }

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.Files))
        {
            e.DragEffects = DragDropEffects.Copy;
        }
    }

    private async void OnDrop(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.Files))
        {
            var files = e.Data.GetFiles();
            if (files != null)
            {
                foreach (var file in files)
                {
                    await TryOpenSaveFileAsync(file.Path.LocalPath);
                }
            }
        }
    }

    private async Task OpenSaveFileAsync()
    {
        var options = new FilePickerOpenOptions
        {
            Title = "Open Save File",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("New Horizons Save")
                {
                    Patterns = new[] { "main.dat", "*.dat" }
                }
            }
        };

        var result = await StorageProvider.OpenFilePickerAsync(options);
        if (result.Count > 0)
        {
            await TryOpenSaveFileAsync(result[0].Path.LocalPath);
        }
    }

    private async Task TryOpenSaveFileAsync(string path)
    {
        try
        {
            if (!SaveFileLoader.TryGetSaveFile(path, out var sav))
            {
                await ShowErrorAsync("Failed to load save file.");
                return;
            }

            var editorWindow = new EditorWindow(sav!);
            await editorWindow.ShowDialog(this);
        }
        catch (Exception ex)
        {
            await ShowErrorAsync($"Error loading save file: {ex.Message}");
        }
    }

    private async Task ShowErrorAsync(string message)
    {
        var dialog = new Window
        {
            Title = "Error",
            Width = 400,
            Height = 200,
            Content = new StackPanel
            {
                Margin = new Avalonia.Thickness(20),
                Children =
                {
                    new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap },
                    new Button 
                    { 
                        Content = "OK", 
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        Margin = new Avalonia.Thickness(0, 20, 0, 0)
                    }
                }
            }
        };

        if (dialog.Content is StackPanel panel && panel.Children[1] is Button btn)
        {
            btn.Click += (s, e) => dialog.Close();
        }

        await dialog.ShowDialog(this);
    }
}
