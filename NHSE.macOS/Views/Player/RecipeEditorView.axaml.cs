using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NHSE.macOS.Views.Player;

public partial class RecipeEditorView : UserControl
{
    public RecipeEditorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
