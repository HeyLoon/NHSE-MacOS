using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using NHSE.Core;
using NHSE.Sprites;
using System;
using System.Globalization;
using System.IO;

namespace NHSE.macOS.Converters;

public class ByteArrayToImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is byte[] byteArray && byteArray.Length > 0)
        {
            try
            {
                using var stream = new MemoryStream(byteArray);
                return new Bitmap(stream);
            }
            catch
            {
                return null;
            }
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ItemToImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Item item)
        {
            try
            {
                var sprite = ItemSprite.GetItemSprite(item.ItemId, item.Count);
                if (sprite != null)
                {
                    using var stream = new MemoryStream();
                    sprite.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Position = 0;
                    return new Bitmap(stream);
                }
            }
            catch
            {
                return null;
            }
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ItemIdToImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        ushort itemId = 0;
        ushort count = 1;
        
        if (value is ushort id)
        {
            itemId = id;
        }
        else if (value is int intId)
        {
            itemId = (ushort)intId;
        }
        else if (value is Item item)
        {
            itemId = item.ItemId;
            count = item.Count;
        }
        
        if (itemId != 0)
        {
            try
            {
                var sprite = ItemSprite.GetItemSprite(itemId, count);
                if (sprite != null)
                {
                    using var stream = new MemoryStream();
                    sprite.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Position = 0;
                    return new Bitmap(stream);
                }
            }
            catch
            {
                return null;
            }
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
