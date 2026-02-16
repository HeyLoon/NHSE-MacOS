using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NHSE.macOS.Views;

public partial class PlayerView : UserControl
{
    public PlayerView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
