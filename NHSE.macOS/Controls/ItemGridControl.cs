using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using NHSE.Core;
using NHSE.macOS.ViewModels;
using NHSE.Sprites;
using System;
using System.IO;

namespace NHSE.macOS.Controls;

public class ItemGridControl : Control
{
    public static readonly StyledProperty<ItemViewModel?>[,]> ItemsProperty =
        AvaloniaProperty.Register<ItemGridControl, ItemViewModel?[,]>(nameof(Items));

    public static readonly StyledProperty<int> ColumnsProperty =
        AvaloniaProperty.Register<ItemGridControl, int>(nameof(Columns), 10);

    public static readonly StyledProperty<int> RowsProperty =
        AvaloniaProperty.Register<ItemGridControl, int>(nameof(Rows), 4);

    public static readonly StyledProperty<int> CellSizeProperty =
        AvaloniaProperty.Register<ItemGridControl, int>(nameof(CellSize), 64);

    public static readonly StyledProperty<ItemViewModel?> SelectedItemProperty =
        AvaloniaProperty.Register<ItemGridControl, ItemViewModel?>(nameof(SelectedItem));

    public ItemViewModel?[,] Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public int Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public int Rows
    {
        get => GetValue(RowsProperty);
        set => SetValue(RowsProperty, value);
    }

    public int CellSize
    {
        get => GetValue(CellSizeProperty);
        set => SetValue(CellSizeProperty, value);
    }

    public ItemViewModel? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public ItemGridControl()
    {
        Items = new ItemViewModel?[Columns, Rows];
        ClipToBounds = true;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        var position = e.GetPosition(this);
        var col = (int)(position.X / CellSize);
        var row = (int)(position.Y / CellSize);

        if (col >= 0 && col < Columns && row >= 0 && row < Rows)
        {
            SelectedItem = Items[col, row];
            e.Handled = true;
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                var x = col * CellSize;
                var y = row * CellSize;
                var rect = new Rect(x, y, CellSize - 1, CellSize - 1);

                // Draw cell background
                var brush = SelectedItem == Items[col, row] ? Brushes.LightBlue : Brushes.White;
                context.FillRectangle(brush, rect);
                context.DrawRectangle(new Pen(Brushes.Gray, 1), rect);

                // Draw item
                var item = Items[col, row];
                if (item != null && item.Item.ItemId != Item.NONE)
                {
                    DrawItem(context, item.Item, x, y);
                }
            }
        }
    }

    private void DrawItem(DrawingContext context, Item item, double x, double y)
    {
        try
        {
            var sprite = ItemSprite.GetItemSprite(item.ItemId, item.Count);
            if (sprite != null)
            {
                using var stream = new MemoryStream();
                sprite.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;
                var bitmap = new Bitmap(stream);
                context.DrawImage(bitmap, new Rect(x, y, CellSize - 2, CellSize - 2));
            }
        }
        catch
        {
            // Ignore drawing errors
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        return new Size(Columns * CellSize, Rows * CellSize);
    }
}
