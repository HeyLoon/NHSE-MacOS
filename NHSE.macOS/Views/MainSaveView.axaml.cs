using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NHSE.macOS.Views;

public partial class MainSaveView : UserControl
{
    public MainSaveView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
