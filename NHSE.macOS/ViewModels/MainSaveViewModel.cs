using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NHSE.Core;
using System;

namespace NHSE.macOS.ViewModels;

public partial class MainSaveViewModel : ViewModelBase
{
    private readonly MainSave _main;

    [ObservableProperty]
    private Hemisphere _hemisphere;

    [ObservableProperty]
    private Hemisphere _tourHemisphere;

    [ObservableProperty]
    private AirportColor _airportColor;

    [ObservableProperty]
    private uint _weatherSeed;

    [ObservableProperty]
    private uint _tourWeatherSeed;

    [ObservableProperty]
    private TurnipStonk _turnips;

    public MainSaveViewModel(MainSave mainSave)
    {
        _main = mainSave;
        LoadFromSave();
    }

    private void LoadFromSave()
    {
        Hemisphere = _main.Hemisphere;
        TourHemisphere = _main.TourHemisphere;
        AirportColor = _main.AirportThemeColor;
        WeatherSeed = _main.WeatherSeed;
        TourWeatherSeed = _main.TourWeatherSeed;
        Turnips = _main.Turnips;
    }

    public void Save()
    {
        _main.Hemisphere = Hemisphere;
        _main.TourHemisphere = TourHemisphere;
        _main.AirportThemeColor = AirportColor;
        _main.WeatherSeed = WeatherSeed;
        _main.TourWeatherSeed = TourWeatherSeed;
        _main.Turnips = Turnips;
    }

    [RelayCommand]
    private void EditFieldItems()
    {
        // Open field item editor
    }

    [RelayCommand]
    private void EditLandFlags()
    {
        // Open land flag editor
    }

    [RelayCommand]
    private void EditPatterns()
    {
        // Open pattern editor
    }

    [RelayCommand]
    private void EditProDesigns()
    {
        // Open PRO design editor
    }

    [RelayCommand]
    private void EditPatternFlag()
    {
        // Open pattern flag editor
    }

    [RelayCommand]
    private void EditDesignsTailor()
    {
        // Open tailor designs editor
    }

    [RelayCommand]
    private void EditPlayerHouses()
    {
        // Open player house editor
    }

    [RelayCommand]
    private void EditCampsite()
    {
        // Open campsite editor
    }

    [RelayCommand]
    private void EditRecycleBin()
    {
        // Open recycle bin editor
    }

    [RelayCommand]
    private void EditBulletin()
    {
        // Open bulletin editor
    }

    [RelayCommand]
    private void EditFieldGoods()
    {
        // Open field goods editor
    }

    [RelayCommand]
    private void EditMuseum()
    {
        // Open museum editor
    }

    [RelayCommand]
    private void EditVisitors()
    {
        // Open visitors editor
    }

    [RelayCommand]
    private void EditTurnipExchange()
    {
        // Open turnip exchange editor
    }

    [RelayCommand]
    private void EditFruitsFlowers()
    {
        // Open fruits and flowers editor
    }
}
