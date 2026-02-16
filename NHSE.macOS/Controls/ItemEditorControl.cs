using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using NHSE.Core;
using NHSE.macOS.ViewModels;
using NHSE.Sprites;
using System;
using System.IO;

namespace NHSE.macOS.Controls;

public class ItemEditorControl : UserControl
{
    private Image _itemImage;
    private ComboBox _itemComboBox;
    private NumericUpDown _countNumeric;
    private NumericUpDown _usesNumeric;
    private NumericUpDown _flag0Numeric;
    private NumericUpDown _flag1Numeric;
    private CheckBox _wrappedCheckBox;
    private CheckBox _extensionCheckBox;
    private StackPanel _extensionPanel;
    private StackPanel _flowerPanel;

    public static readonly StyledProperty<ItemEditorViewModel> ViewModelProperty =
        AvaloniaProperty.Register<ItemEditorControl, ItemEditorViewModel>(nameof(ViewModel));

    public ItemEditorViewModel ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public ItemEditorControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        var grid = new Grid
        {
            RowDefinitions = new RowDefinitions("Auto,Auto,Auto,Auto,Auto,*"),
            ColumnDefinitions = new ColumnDefinitions("*,Auto")
        };

        // Item Image
        _itemImage = new Image
        {
            Width = 64,
            Height = 64,
            Stretch = Avalonia.Media.Stretch.Uniform,
            Margin = new Thickness(10)
        };
        Grid.SetRow(_itemImage, 0);
        Grid.SetColumn(_itemImage, 1);
        Grid.SetRowSpan(_itemImage, 2);
        grid.Children.Add(_itemImage);

        // Item Selection
        var itemPanel = new StackPanel { Margin = new Thickness(5) };
        itemPanel.Children.Add(new TextBlock { Text = "Item:" });
        _itemComboBox = new ComboBox { ItemsSource = GameInfo.Strings.ItemDataSource };
        _itemComboBox.SelectionChanged += OnItemChanged;
        itemPanel.Children.Add(_itemComboBox);
        Grid.SetRow(itemPanel, 0);
        grid.Children.Add(itemPanel);

        // Extension Checkbox
        _extensionCheckBox = new CheckBox { Content = "Is Extension" };
        _extensionCheckBox.IsCheckedChanged += OnExtensionChanged;
        Grid.SetRow(_extensionCheckBox, 1);
        grid.Children.Add(_extensionCheckBox);

        // Extension Panel
        _extensionPanel = new StackPanel 
        { 
            IsVisible = false,
            Margin = new Thickness(5)
        };
        _extensionPanel.Children.Add(new TextBlock { Text = "Extension X:" });
        _extensionPanel.Children.Add(new NumericUpDown { Minimum = 0, Maximum = 255 });
        _extensionPanel.Children.Add(new TextBlock { Text = "Extension Y:" });
        _extensionPanel.Children.Add(new NumericUpDown { Minimum = 0, Maximum = 255 });
        Grid.SetRow(_extensionPanel, 2);
        grid.Children.Add(_extensionPanel);

        // Regular Item Panel
        var itemDetailsPanel = new StackPanel { Margin = new Thickness(5) };
        
        itemDetailsPanel.Children.Add(new TextBlock { Text = "Count:" });
        _countNumeric = new NumericUpDown { Minimum = 0, Maximum = 65535 };
        _countNumeric.ValueChanged += OnCountChanged;
        itemDetailsPanel.Children.Add(_countNumeric);

        itemDetailsPanel.Children.Add(new TextBlock { Text = "Use Count:" });
        _usesNumeric = new NumericUpDown { Minimum = 0, Maximum = 65535 };
        itemDetailsPanel.Children.Add(_usesNumeric);

        itemDetailsPanel.Children.Add(new TextBlock { Text = "System Param:" });
        _flag0Numeric = new NumericUpDown { Minimum = 0, Maximum = 255 };
        itemDetailsPanel.Children.Add(_flag0Numeric);

        itemDetailsPanel.Children.Add(new TextBlock { Text = "Additional Param:" });
        _flag1Numeric = new NumericUpDown { Minimum = 0, Maximum = 255 };
        _flag1Numeric.IsVisible = false;
        itemDetailsPanel.Children.Add(_flag1Numeric);

        // Wrapped Checkbox
        _wrappedCheckBox = new CheckBox { Content = "Wrapped" };
        _wrappedCheckBox.IsCheckedChanged += OnWrappedChanged;
        itemDetailsPanel.Children.Add(_wrappedCheckBox);

        Grid.SetRow(itemDetailsPanel, 3);
        grid.Children.Add(itemDetailsPanel);

        // Flower Panel
        _flowerPanel = new StackPanel 
        { 
            IsVisible = false,
            Margin = new Thickness(5)
        };
        _flowerPanel.Children.Add(new TextBlock { Text = "Flower Genes:" });
        var genesPanel = new UniformGrid { Columns = 4 };
        genesPanel.Children.Add(new CheckBox { Content = "R1" });
        genesPanel.Children.Add(new CheckBox { Content = "R2" });
        genesPanel.Children.Add(new CheckBox { Content = "Y1" });
        genesPanel.Children.Add(new CheckBox { Content = "Y2" });
        genesPanel.Children.Add(new CheckBox { Content = "W1" });
        genesPanel.Children.Add(new CheckBox { Content = "W2" });
        genesPanel.Children.Add(new CheckBox { Content = "S1" });
        genesPanel.Children.Add(new CheckBox { Content = "S2" });
        _flowerPanel.Children.Add(genesPanel);
        _flowerPanel.Children.Add(new CheckBox { Content = "Watered" });
        _flowerPanel.Children.Add(new CheckBox { Content = "Gold" });
        _flowerPanel.Children.Add(new TextBlock { Text = "Water Days:" });
        _flowerPanel.Children.Add(new NumericUpDown { Minimum = 0, Maximum = 31 });
        
        Grid.SetRow(_flowerPanel, 4);
        grid.Children.Add(_flowerPanel);

        Content = grid;
    }

    private void OnItemChanged(object? sender, SelectionChangedEventArgs e)
    {
        UpdateItemImage();
    }

    private void OnCountChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        UpdateItemImage();
    }

    private void OnExtensionChanged(object? sender, RoutedEventArgs e)
    {
        if (_extensionPanel != null)
        {
            _extensionPanel.IsVisible = _extensionCheckBox.IsChecked ?? false;
        }
    }

    private void OnWrappedChanged(object? sender, RoutedEventArgs e)
    {
        // Handle wrapped change
    }

    private void UpdateItemImage()
    {
        try
        {
            if (_itemComboBox.SelectedItem is ComboItem item && _countNumeric != null)
            {
                var sprite = ItemSprite.GetItemSprite((ushort)item.Value, (ushort)_countNumeric.Value);
                if (sprite != null)
                {
                    using var stream = new MemoryStream();
                    sprite.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Position = 0;
                    _itemImage.Source = new Bitmap(stream);
                }
            }
        }
        catch
        {
            // Ignore errors
        }
    }
}
