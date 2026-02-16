using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NHSE.Core;
using System;

namespace NHSE.macOS.ViewModels;

public partial class PlayerViewModel : ViewModelBase
{
    private readonly Player _player;
    private readonly MainSave _mainSave;
    private readonly int _playerIndex;

    [ObservableProperty]
    private string _playerName = "";

    [ObservableProperty]
    private string _townName = "";

    [ObservableProperty]
    private uint _bankBells;

    [ObservableProperty]
    private uint _nookMiles;

    [ObservableProperty]
    private uint _totalNookMiles;

    [ObservableProperty]
    private uint _wallet;

    [ObservableProperty]
    private uint _pocketCount1;

    [ObservableProperty]
    private uint _pocketCount2;

    [ObservableProperty]
    private uint _storageCount;

    [ObservableProperty]
    private uint _hotelTickets;

    [ObservableProperty]
    private uint _poki;

    [ObservableProperty]
    private bool _hasHotelTickets;

    [ObservableProperty]
    private bool _hasPoki;

    [ObservableProperty]
    private byte[]? _playerPhoto;

    public Player Player => _player;
    public int PlayerIndex => _playerIndex;

    public PlayerViewModel(Player player, MainSave mainSave, int playerIndex)
    {
        _player = player;
        _mainSave = mainSave;
        _playerIndex = playerIndex;
        LoadFromPlayer();
    }

    private void LoadFromPlayer()
    {
        var pers = _player.Personal;
        PlayerName = pers.PlayerName;
        TownName = pers.TownName;
        BankBells = pers.Bank.Value;
        NookMiles = pers.NookMiles.Value;
        TotalNookMiles = pers.TotalNookMiles.Value;
        Wallet = pers.Wallet.Value;
        
        // Swapped on purpose -- first count is the first two rows of items
        PocketCount1 = pers.PocketCount;
        PocketCount2 = pers.BagCount;
        StorageCount = pers.ItemChestCount;

        HasHotelTickets = pers.Data30 is { IsInitialized30: true };
        if (HasHotelTickets)
        {
            HotelTickets = pers.Data30?.HotelTickets.Value ?? 0;
        }

        HasPoki = _player.WhereAreN != null;
        if (HasPoki)
        {
            Poki = _player.WhereAreN?.Poki.Value ?? 0;
        }

        try
        {
            PlayerPhoto = pers.GetPhotoData();
        }
        catch
        {
            PlayerPhoto = null;
        }
    }

    public void Save()
    {
        var pers = _player.Personal;

        if (pers.PlayerName != PlayerName)
        {
            var orig = pers.GetPlayerIdentity().ToArray();
            pers.PlayerName = PlayerName;
            var updated = pers.GetPlayerIdentity();
            //_mainSave.ChangeIdentity(orig, updated);
        }

        if (pers.TownName != TownName)
        {
            var orig = pers.GetTownIdentity().ToArray();
            pers.TownName = TownName;
            var updated = pers.GetTownIdentity();
            //_mainSave.ChangeIdentity(orig, updated);
        }

        pers.Bank = pers.Bank with { Value = BankBells };
        pers.NookMiles = pers.NookMiles with { Value = NookMiles };
        pers.TotalNookMiles = pers.TotalNookMiles with { Value = TotalNookMiles };
        pers.Wallet = pers.Wallet with { Value = Wallet };

        pers.PocketCount = PocketCount1;
        pers.BagCount = PocketCount2;
        pers.ItemChestCount = StorageCount;

        if (HasHotelTickets && pers.Data30 is { IsInitialized30: true } addition)
        {
            addition.HotelTickets = addition.HotelTickets with { Value = HotelTickets };
        }

        if (HasPoki && _player.WhereAreN is { } x)
        {
            x.Poki = x.Poki with { Value = Poki };
        }
    }

    [RelayCommand]
    private void EditPlayerItems()
    {
        var pers = _player.Personal;
        var bag = pers.Bag;
        var pocket = pers.Pocket;
        // Open player item editor
    }

    [RelayCommand]
    private void EditStorage()
    {
        var pers = _player.Personal;
        var items = pers.ItemChest;
        // Open storage editor
    }

    [RelayCommand]
    private void EditRecipes()
    {
        // Open recipe editor
    }

    [RelayCommand]
    private void EditReceivedItems()
    {
        // Open received items editor
    }

    [RelayCommand]
    private void EditReactions()
    {
        // Open reactions editor
    }

    [RelayCommand]
    private void EditMisc()
    {
        // Open misc editor
    }

    [RelayCommand]
    private void EditPostBox()
    {
        // Open post box editor
    }

    [RelayCommand]
    private void EditAchievements()
    {
        // Open achievements editor
    }

    [RelayCommand]
    private void EditFlags()
    {
        // Open flags editor
    }

    [RelayCommand]
    private void SavePhoto()
    {
        // Save player photo
    }
}
