using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NHSE.Core;
using NHSE.Injection;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NHSE.macOS.ViewModels.SysBot;

public partial class SysBotViewModel : ViewModelBase
{
    private readonly InjectionType _injectionType;
    private SysBot? _sysBot;
    private USBBot? _usbBot;

    [ObservableProperty]
    private string _ipAddress = "";

    [ObservableProperty]
    private int _port = 6000;

    [ObservableProperty]
    private bool _isConnected;

    [ObservableProperty]
    private string _connectionStatus = "Not Connected";

    [ObservableProperty]
    private ObservableCollection<string> _logMessages = new();

    [ObservableProperty]
    private bool _useUSB;

    [ObservableProperty]
    private byte[] _readBuffer = [];

    [ObservableProperty]
    private byte[] _writeBuffer = [];

    [ObservableProperty]
    private string _readAddress = "";

    [ObservableProperty]
    private string _writeAddress = "";

    [ObservableProperty]
    private int _readLength = 32;

    public SysBotViewModel(InjectionType injectionType)
    {
        _injectionType = injectionType;
    }

    [RelayCommand]
    private async Task ConnectAsync()
    {
        try
        {
            IsLoading = true;
            ClearError();

            if (UseUSB)
            {
                _usbBot = new USBBot();
                // USB connection logic
            }
            else
            {
                _sysBot = new SysBot();
                _sysBot.Connect(IPAddress, Port);
            }

            IsConnected = true;
            ConnectionStatus = "Connected";
            AddLog("Connected successfully");
        }
        catch (Exception ex)
        {
            SetError($"Connection failed: {ex.Message}");
            AddLog($"Connection failed: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void Disconnect()
    {
        IsConnected = false;
        ConnectionStatus = "Not Connected";
        _sysBot?.Disconnect();
        _usbBot?.Disconnect();
        _sysBot = null;
        _usbBot = null;
        AddLog("Disconnected");
    }

    [RelayCommand]
    private async Task ReadMemoryAsync()
    {
        if (!IsConnected) return;

        try
        {
            IsLoading = true;
            
            // Parse address
            if (ulong.TryParse(ReadAddress, System.Globalization.NumberStyles.HexNumber, null, out var address))
            {
                // Read memory logic
                AddLog($"Read {ReadLength} bytes from 0x{address:X}");
            }
        }
        catch (Exception ex)
        {
            SetError($"Read failed: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task WriteMemoryAsync()
    {
        if (!IsConnected) return;

        try
        {
            IsLoading = true;
            
            // Parse address
            if (ulong.TryParse(WriteAddress, System.Globalization.NumberStyles.HexNumber, null, out var address))
            {
                // Write memory logic
                AddLog($"Wrote {WriteBuffer.Length} bytes to 0x{address:X}");
            }
        }
        catch (Exception ex)
        {
            SetError($"Write failed: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void InjectItems(Item[] items)
    {
        if (!IsConnected || _sysBot == null) return;

        try
        {
            var injector = new PocketInjector(items, _sysBot);
            AddLog($"Injected {items.Length} items");
        }
        catch (Exception ex)
        {
            SetError($"Injection failed: {ex.Message}");
        }
    }

    private void AddLog(string message)
    {
        LogMessages.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
    }
}

public partial class BatchEditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _searchPattern = "";

    [ObservableProperty]
    private string _replacePattern = "";

    [ObservableProperty]
    private string _targetType = "";

    [ObservableProperty]
    private int _batchSize = 100;

    [ObservableProperty]
    private ObservableCollection<BatchEditResult> _results = new();

    [ObservableProperty]
    private bool _isRunning;

    [ObservableProperty]
    private double _progress;

    [RelayCommand]
    private async Task StartBatchEditAsync()
    {
        if (IsRunning) return;

        try
        {
            IsRunning = true;
            Results.Clear();
            Progress = 0;

            // Batch edit logic
            await Task.Delay(100); // Placeholder

            Results.Add(new BatchEditResult { Success = true, Message = "Batch edit completed" });
        }
        catch (Exception ex)
        {
            Results.Add(new BatchEditResult { Success = false, Message = ex.Message });
        }
        finally
        {
            IsRunning = false;
            Progress = 100;
        }
    }

    [RelayCommand]
    private void CancelBatchEdit()
    {
        // Cancel logic
    }
}

public partial class BatchEditResult : ObservableObject
{
    [ObservableProperty]
    private bool _success;

    [ObservableProperty]
    private string _message = "";

    [ObservableProperty]
    private int _affectedCount;
}

public partial class AutoInjectorViewModel : ViewModelBase
{
    private readonly AutoInjector? _autoInjector;

    [ObservableProperty]
    private bool _isInjecting;

    [ObservableProperty]
    private int _readInterval = 1000;

    [ObservableProperty]
    private int _writeDelay = 100;

    [ObservableProperty]
    private string _status = "Idle";

    [ObservableProperty]
    private int _successCount;

    [ObservableProperty]
    private int _failCount;

    public AutoInjectorViewModel(PocketInjector injector, SysBotViewModel sysBotViewModel)
    {
        // Create auto injector
        _autoInjector = null; // TODO: Fix AutoInjector constructor signature
        /*
        _autoInjector = new AutoInjector(
            injector,
            result =>
            {
                if (result == InjectionResult.Success)
                    SuccessCount++;
                else
                    FailCount++;
            },
            result =>
            {
                // Write callback
            }
        );
        */
    }

    [RelayCommand]
    private void StartAutoInject()
    {
        IsInjecting = true;
        Status = "Injecting...";
    }

    [RelayCommand]
    private void StopAutoInject()
    {
        IsInjecting = false;
        Status = "Stopped";
    }

    [RelayCommand]
    private void ResetCounters()
    {
        SuccessCount = 0;
        FailCount = 0;
    }
}

public partial class HexEditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private byte[] _data = [];

    [ObservableProperty]
    private string _hexString = "";

    [ObservableProperty]
    private int _address = 0;

    [ObservableProperty]
    private int _selectionStart;

    [ObservableProperty]
    private int _selectionLength;

    partial void OnDataChanged(byte[] value)
    {
        HexString = BitConverter.ToString(value).Replace("-", " ");
    }

    partial void OnHexStringChanged(string value)
    {
        try
        {
            var hex = value.Replace(" ", "").Replace("-", "");
            if (hex.Length % 2 == 0)
            {
                var bytes = new byte[hex.Length / 2];
                for (int i = 0; i < hex.Length; i += 2)
                {
                    bytes[i / 2] = byte.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
                }
                Data = bytes;
            }
        }
        catch
        {
            // Ignore invalid hex
        }
    }

    [RelayCommand]
    private void JumpToAddress()
    {
        // Jump to address logic
    }

    [RelayCommand]
    private void SaveChanges()
    {
        // Save changes logic
    }

    [RelayCommand]
    private void ExportToFile()
    {
        // Export logic
    }

    [RelayCommand]
    private void ImportFromFile()
    {
        // Import logic
    }
}
