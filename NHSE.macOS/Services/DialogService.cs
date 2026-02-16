using NHSE.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHSE.macOS.Services;

public interface IDialogService
{
    Task ShowInformationAsync(string title, string message);
    Task ShowWarningAsync(string title, string message);
    Task ShowErrorAsync(string title, string message);
    Task<bool> ShowConfirmationAsync(string title, string message);
    Task<string?> ShowOpenFileDialogAsync(string title, string filter, string defaultFileName);
    Task<string?> ShowSaveFileDialogAsync(string title, string filter, string defaultFileName);
    Task<string?> ShowFolderBrowserDialogAsync(string title);
}

public interface IClipboardService
{
    Task SetTextAsync(string text);
    Task<string?> GetTextAsync();
}

public interface INotificationService
{
    void ShowNotification(string title, string message);
}

public class DialogService : IDialogService
{
    public Task ShowInformationAsync(string title, string message)
    {
        return ShowDialogAsync(title, message, DialogType.Information);
    }

    public Task ShowWarningAsync(string title, string message)
    {
        return ShowDialogAsync(title, message, DialogType.Warning);
    }

    public Task ShowErrorAsync(string title, string message)
    {
        return ShowDialogAsync(title, message, DialogType.Error);
    }

    public Task<bool> ShowConfirmationAsync(string title, string message)
    {
        return ShowConfirmationDialogAsync(title, message);
    }

    public Task<string?> ShowOpenFileDialogAsync(string title, string filter, string defaultFileName)
    {
        // Implementation would use Avalonia's file dialogs
        return Task.FromResult<string?>(null);
    }

    public Task<string?> ShowSaveFileDialogAsync(string title, string filter, string defaultFileName)
    {
        return Task.FromResult<string?>(null);
    }

    public Task<string?> ShowFolderBrowserDialogAsync(string title)
    {
        return Task.FromResult<string?>(null);
    }

    private Task ShowDialogAsync(string title, string message, DialogType type)
    {
        // Implementation will be done when windows are available
        return Task.CompletedTask;
    }

    private Task<bool> ShowConfirmationDialogAsync(string title, string message)
    {
        return Task.FromResult(false);
    }
}

public enum DialogType
{
    Information,
    Warning,
    Error
}

public class ClipboardService : IClipboardService
{
    public Task SetTextAsync(string text)
    {
        // Will be implemented with Avalonia's clipboard API
        return Task.CompletedTask;
    }

    public Task<string?> GetTextAsync()
    {
        return Task.FromResult<string?>(null);
    }
}

public class NotificationService : INotificationService
{
    public void ShowNotification(string title, string message)
    {
        // Implementation for macOS notifications
    }
}
