using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NHSE.macOS.Views.Player;

public partial class FlagEditorView : UserControl
{
    public FlagEditorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
