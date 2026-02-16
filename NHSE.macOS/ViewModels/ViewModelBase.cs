using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace NHSE.macOS.ViewModels;

public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _hasError;

    protected void SetError(string? message)
    {
        ErrorMessage = message;
        HasError = !string.IsNullOrEmpty(message);
    }

    protected void ClearError()
    {
        ErrorMessage = null;
        HasError = false;
    }
}

public abstract partial class ViewModelBase<TModel> : ViewModelBase where TModel : class
{
    [ObservableProperty]
    private TModel? _model;

    protected virtual void OnModelChanged(TModel? value)
    {
    }

    partial void OnModelChanged(TModel? value)
    {
        OnModelChanged(value);
    }
}
