using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NHSE.macOS.Views.Player;

public partial class AchievementEditorView : UserControl
{
    public AchievementEditorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
