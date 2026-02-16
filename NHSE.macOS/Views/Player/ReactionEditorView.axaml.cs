using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NHSE.macOS.Views.Player;

public partial class ReactionEditorView : UserControl
{
    public ReactionEditorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
