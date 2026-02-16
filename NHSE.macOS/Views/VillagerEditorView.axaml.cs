using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NHSE.macOS.Views;

public partial class VillagerEditorView : UserControl
{
    public VillagerEditorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
