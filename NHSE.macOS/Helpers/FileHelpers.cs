using NHSE.Core;
using System;
using System.IO;

namespace NHSE.macOS.Helpers;

public static class SaveFileHelper
{
    public static bool TryGetSaveFile(string path, out HorizonSave? saveFile)
    {
        saveFile = null;
        
        try
        {
            var dir = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
                return false;

            saveFile = SaveFileLoader.LoadSaveFile(dir);
            return saveFile != null;
        }
        catch
        {
            return false;
        }
    }
}

public static class ItemHelper
{
    public static string GetItemDisplayName(Item item)
    {
        if (item.ItemId == Item.NONE)
            return "(Empty)";
        
        return GameInfo.Strings.GetItemName(item.ItemId);
    }

    public static string GetItemHexString(Item item)
    {
        var data = item.ToBytesClass();
        var u64 = System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(data);
        return $"{u64:X16}";
    }

    public static bool TryParseItemHex(string hexString, out Item item)
    {
        item = new Item(Item.NONE);
        
        try
        {
            if (ulong.TryParse(hexString, System.Globalization.NumberStyles.HexNumber, null, out var val))
            {
                item = new Item(val);
                return true;
            }
        }
        catch
        {
            // Ignore parse errors
        }
        
        return false;
    }
}

public static class FileDialogHelper
{
    public static async Task<string?> ShowOpenFileDialogAsync(string title, string[] filters)
    {
        var dialog = new Avalonia.Controls.OpenFileDialog
        {
            Title = title,
            AllowMultiple = false
        };

        foreach (var filter in filters)
        {
            var parts = filter.Split('|');
            if (parts.Length == 2)
            {
                dialog.Filters.Add(new Avalonia.Controls.FileDialogFilter
                {
                    Name = parts[0],
                    Extensions = new System.Collections.Generic.List<string> { parts[1].TrimStart('*', '.') }
                });
            }
        }

        var window = Avalonia.Application.Current?.ApplicationLifetime
            is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow : null;

        if (window != null)
        {
            var result = await dialog.ShowAsync(window);
            return result?.FirstOrDefault();
        }

        return null;
    }

    public static async Task<string?> ShowSaveFileDialogAsync(string title, string[] filters, string defaultFileName)
    {
        var dialog = new Avalonia.Controls.SaveFileDialog
        {
            Title = title,
            DefaultExtension = Path.GetExtension(defaultFileName),
            InitialFileName = defaultFileName
        };

        foreach (var filter in filters)
        {
            var parts = filter.Split('|');
            if (parts.Length == 2)
            {
                dialog.Filters.Add(new Avalonia.Controls.FileDialogFilter
                {
                    Name = parts[0],
                    Extensions = new System.Collections.Generic.List<string> { parts[1].TrimStart('*', '.') }
                });
            }
        }

        var window = Avalonia.Application.Current?.ApplicationLifetime
            is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow : null;

        if (window != null)
        {
            return await dialog.ShowAsync(window);
        }

        return null;
    }

    public static async Task<string?> ShowFolderBrowserDialogAsync(string title)
    {
        var dialog = new Avalonia.Controls.OpenFolderDialog
        {
            Title = title
        };

        var window = Avalonia.Application.Current?.ApplicationLifetime
            is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow : null;

        if (window != null)
        {
            return await dialog.ShowAsync(window);
        }

        return null;
    }
}
