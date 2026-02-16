using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NHSE.Core;
using System;

namespace NHSE.macOS.Views;

public partial class EditorWindow : Window
{
    private readonly SaveFile _saveFile;

    public EditorWindow(SaveFile saveFile)
    {
        _saveFile = saveFile;
        InitializeComponent();
        SetupUI();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void SetupUI()
    {
        Title = $"NHSE Editor - {_saveFile.SaveName}";
        Width = 1200;
        Height = 800;

        var grid = new Grid
        {
            RowDefinitions = new RowDefinitions("Auto,*")
        };

        // Status bar
        var statusBar = new TextBlock
        {
            Text = $"Loaded: {_saveFile.SaveName}",
            Margin = new Avalonia.Thickness(10)
        };
        Grid.SetRow(statusBar, 0);
        grid.Children.Add(statusBar);

        // Main editor content (placeholder for now)
        var tabControl = new TabControl();
        
        // Players tab
        var playersTab = new TabItem { Header = "Players" };
        playersTab.Content = new TextBlock 
        { 
            Text = "Player editor will be implemented here.",
            Margin = new Avalonia.Thickness(20)
        };
        tabControl.Items.Add(playersTab);

        // Villagers tab
        var villagersTab = new TabItem { Header = "Villagers" };
        villagersTab.Content = new TextBlock 
        { 
            Text = "Villager editor will be implemented here.",
            Margin = new Avalonia.Thickness(20)
        };
        tabControl.Items.Add(villagersTab);

        // Island tab
        var islandTab = new TabItem { Header = "Island" };
        islandTab.Content = new TextBlock 
        { 
            Text = "Island editor will be implemented here.",
            Margin = new Avalonia.Thickness(20)
        };
        tabControl.Items.Add(islandTab);

        // Items tab
        var itemsTab = new TabItem { Header = "Items" };
        itemsTab.Content = new TextBlock 
        { 
            Text = "Item editor will be implemented here.",
            Margin = new Avalonia.Thickness(20)
        };
        tabControl.Items.Add(itemsTab);

        Grid.SetRow(tabControl, 1);
        grid.Children.Add(tabControl);

        Content = grid;
    }
}
