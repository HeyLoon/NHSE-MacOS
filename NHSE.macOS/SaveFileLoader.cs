using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using NHSE.Core;

namespace NHSE.macOS;

public static class SaveFileLoader
{
    public static bool TryGetSaveFile(string path, [NotNullWhen(true)] out HorizonSave? sav)
    {
        try
        {
            if (Directory.Exists(path))
                return OpenSaveFile(path, out sav);

            var ext = Path.GetExtension(path);
            if (ext.Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                var length = new FileInfo(path).Length;
                const int maxSize = 20 * 1024 * 1024;
                if (length < maxSize)
                {
                    sav = HorizonSave.FromZip(path);
                    return sav != null;
                }
            }
            else if (ext.Equals(".dat", StringComparison.OrdinalIgnoreCase))
            {
                var dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                    return OpenSaveFile(dir, out sav);
            }

            sav = null;
            return false;
        }
        catch
        {
            sav = null;
            return false;
        }
    }

    private static bool OpenSaveFile(string path, [NotNullWhen(true)] out HorizonSave? sav)
    {
        sav = HorizonSave.FromFolder(path);
        return sav != null && ValidateSaveFile(sav);
    }

    private static bool ValidateSaveFile(HorizonSave file)
    {
        // Skip validation warnings for now - just check if it loaded
        return true;
    }
}
