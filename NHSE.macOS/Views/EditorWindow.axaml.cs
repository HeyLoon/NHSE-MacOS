using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NHSE.Core;
using NHSE.macOS.ViewModels;

namespace NHSE.macOS.Views;

public partial class EditorWindow : Window
{
    public EditorWindow(HorizonSave saveFile)
    {
        InitializeComponent();
        DataContext = new EditorWindowViewModel(saveFile);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
