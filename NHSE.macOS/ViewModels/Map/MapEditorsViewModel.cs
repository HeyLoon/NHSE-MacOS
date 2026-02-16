using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NHSE.Core;
using NHSE.Sprites;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace NHSE.macOS.ViewModels.Map;

public partial class FieldItemEditorViewModel : ViewModelBase
{
    private readonly MainSave _main;
    private Item[] _fieldItems;

    [ObservableProperty]
    private ObservableCollection<FieldItemViewModel> _items = new();

    [ObservableProperty]
    private int _selectedX;

    [ObservableProperty]
    private int _selectedY;

    [ObservableProperty]
    private ItemEditorViewModel _itemEditor = new();

    [ObservableProperty]
    private int _mapWidth;

    [ObservableProperty]
    private int _mapHeight;

    [ObservableProperty]
    private string _searchText = "";

    public FieldItemEditorViewModel(MainSave mainSave)
    {
        _main = mainSave;
        _fieldItems = mainSave.GetFieldItemLayer0();
        MapWidth = mainSave.FieldItemAcreWidth * 32;
        MapHeight = mainSave.FieldItemAcreHeight * 32;
        LoadItems();
    }

    private void LoadItems()
    {
        Items.Clear();
        int idx = 0;
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                var item = _fieldItems[idx++];
                Items.Add(new FieldItemViewModel(item, x, y));
            }
        }
    }

    public void Save()
    {
        _main.SetFieldItemLayer0(_fieldItems);
    }

    [RelayCommand]
    private void SetItem()
    {
        if (SelectedX >= 0 && SelectedX < MapWidth && SelectedY >= 0 && SelectedY < MapHeight)
        {
            var item = ItemEditor.SaveItem();
            int idx = SelectedY * MapWidth + SelectedX;
            _fieldItems[idx] = item;
            
            var existing = Items.FirstOrDefault(i => i.X == SelectedX && i.Y == SelectedY);
            if (existing != null)
            {
                existing.Item = item;
            }
        }
    }

    [RelayCommand]
    private void ClearItem()
    {
        if (SelectedX >= 0 && SelectedX < MapWidth && SelectedY >= 0 && SelectedY < MapHeight)
        {
            int idx = SelectedY * MapWidth + SelectedX;
            _fieldItems[idx] = new Item(Item.NONE);
            
            var existing = Items.FirstOrDefault(i => i.X == SelectedX && i.Y == SelectedY);
            if (existing != null)
            {
                existing.Item = new Item(Item.NONE);
            }
        }
    }

    [RelayCommand]
    private void BulkSpawn()
    {
        // Open bulk spawn dialog
    }

    [RelayCommand]
    private void SpawnCircle()
    {
        // Spawn items in circle pattern
    }

    [RelayCommand]
    private void SpawnRectangle()
    {
        // Spawn items in rectangle pattern
    }

    [RelayCommand]
    private void ImportLayer()
    {
        // Import field item layer
    }

    [RelayCommand]
    private void ExportLayer()
    {
        // Export field item layer
    }
}

public partial class FieldItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private Item _item;

    [ObservableProperty]
    private int _x;

    [ObservableProperty]
    private int _y;

    [ObservableProperty]
    private string _itemName = "";

    public FieldItemViewModel(Item item, int x, int y)
    {
        _item = item;
        _x = x;
        _y = y;
        UpdateItemName();
    }

    partial void OnItemChanged(Item value)
    {
        UpdateItemName();
    }

    private void UpdateItemName()
    {
        ItemName = GameInfo.Strings.GetItemName(Item.ItemId);
    }
}

public partial class PatternEditorViewModel : ViewModelBase
{
    private readonly DesignPattern[] _patterns;
    private readonly global::NHSE.Core.Player _player;

    [ObservableProperty]
    private ObservableCollection<DesignPatternViewModel> _designs = new();

    [ObservableProperty]
    private int _selectedPatternIndex;

    [ObservableProperty]
    private DesignPatternViewModel? _selectedPattern;

    [ObservableProperty]
    private byte[]? _patternImage;

    public PatternEditorViewModel(DesignPattern[] patterns, global::NHSE.Core.Player player)
    {
        _patterns = patterns;
        _player = player;
        LoadPatterns();
    }

    private void LoadPatterns()
    {
        Designs.Clear();
        for (int i = 0; i < _patterns.Length; i++)
        {
            Designs.Add(new DesignPatternViewModel(_patterns[i], i));
        }
        
        if (Designs.Count > 0)
        {
            SelectedPatternIndex = 0;
        }
    }

    public void Save()
    {
        // Patterns are already modified in-place
    }

    partial void OnSelectedPatternIndexChanged(int value)
    {
        if (value >= 0 && value < Designs.Count)
        {
            SelectedPattern = Designs[value];
            LoadPatternImage();
        }
    }

    private void LoadPatternImage()
    {
        if (SelectedPattern == null) return;
        
        try
        {
            var bmp = SelectedPattern.Pattern.GetBitmap();
            using var stream = new MemoryStream();
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            PatternImage = stream.ToArray();
        }
        catch
        {
            PatternImage = null;
        }
    }

    [RelayCommand]
    private void ImportPattern()
    {
        // Import pattern from file
    }

    [RelayCommand]
    private void ExportPattern()
    {
        // Export pattern to file
    }

    [RelayCommand]
    private void LoadFromPlayer()
    {
        // Load pattern from player designs
    }
}

public partial class DesignPatternViewModel : ObservableObject
{
    [ObservableProperty]
    private DesignPattern _pattern;

    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private string _name = "";

    public DesignPatternViewModel(DesignPattern pattern, int index)
    {
        _pattern = pattern;
        _index = index;
        Name = pattern.DesignName;
    }
}

public partial class PlayerHouseEditorViewModel : ViewModelBase
{
    private readonly IPlayerHouse[] _houses;
    private readonly global::NHSE.Core.Player[] _players;
    private readonly MainSave _main;
    private readonly int _currentPlayer;

    [ObservableProperty]
    private ObservableCollection<PlayerHouseViewModel> _houseList = new();

    [ObservableProperty]
    private int _selectedHouseIndex;

    [ObservableProperty]
    private PlayerHouseViewModel? _selectedHouse;

    public PlayerHouseEditorViewModel(IPlayerHouse[] houses, global::NHSE.Core.Player[] players, MainSave main, int currentPlayer)
    {
        _houses = houses;
        _players = players;
        _main = main;
        _currentPlayer = currentPlayer;
        LoadHouses();
    }

    private void LoadHouses()
    {
        HouseList.Clear();
        for (int i = 0; i < _houses.Length; i++)
        {
            HouseList.Add(new PlayerHouseViewModel(_houses[i], i, _players[i].Personal.PlayerName));
        }
        
        if (HouseList.Count > 0)
        {
            SelectedHouseIndex = _currentPlayer;
        }
    }

    public void Save()
    {
        // Houses are modified in-place
    }

    partial void OnSelectedHouseIndexChanged(int value)
    {
        if (value >= 0 && value < HouseList.Count)
        {
            SelectedHouse = HouseList[value];
        }
    }

    [RelayCommand]
    private void EditHouseFlags()
    {
        // Open house flag editor
    }
}

public partial class PlayerHouseViewModel : ObservableObject
{
    [ObservableProperty]
    private IPlayerHouse _house;

    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private string _playerName = "";

    [ObservableProperty]
    private int _roomCount;

    public PlayerHouseViewModel(IPlayerHouse house, int index, string playerName)
    {
        _house = house;
        _index = index;
        _playerName = playerName;
        RoomCount = house.RoomCount;
    }
}

public partial class VillagerHouseEditorViewModel : ViewModelBase
{
    private readonly IVillagerHouse[] _houses;
    private readonly IVillager[] _villagers;

    [ObservableProperty]
    private ObservableCollection<VillagerHouseViewModel> _houseList = new();

    [ObservableProperty]
    private int _selectedHouseIndex;

    [ObservableProperty]
    private VillagerHouseViewModel? _selectedHouse;

    public VillagerHouseEditorViewModel(IVillagerHouse[] houses, IVillager[] villagers)
    {
        _houses = houses;
        _villagers = villagers;
        LoadHouses();
    }

    private void LoadHouses()
    {
        HouseList.Clear();
        for (int i = 0; i < _houses.Length; i++)
        {
            var villagerId = i < _villagers.Length ? _villagers[i].VillagerId : (ushort)0;
            HouseList.Add(new VillagerHouseViewModel(_houses[i], i, villagerId));
        }
        
        if (HouseList.Count > 0)
        {
            SelectedHouseIndex = 0;
        }
    }

    public void Save()
    {
        // Houses are modified in-place
    }

    partial void OnSelectedHouseIndexChanged(int value)
    {
        if (value >= 0 && value < HouseList.Count)
        {
            SelectedHouse = HouseList[value];
        }
    }
}

public partial class VillagerHouseViewModel : ObservableObject
{
    [ObservableProperty]
    private IVillagerHouse _house;

    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private ushort _villagerId;

    [ObservableProperty]
    private string _villagerName = "";

    public VillagerHouseViewModel(IVillagerHouse house, int index, ushort villagerId)
    {
        _house = house;
        _index = index;
        _villagerId = villagerId;
        VillagerName = VillagerResources.GetVillagerName(villagerId, GameInfo.Strings);
    }
}

public partial class MuseumEditorViewModel : ViewModelBase
{
    private readonly Museum _museum;

    [ObservableProperty]
    private bool[] _donatedFish = [];

    [ObservableProperty]
    private bool[] _donatedInsects = [];

    [ObservableProperty]
    private bool[] _donatedSeaCreatures = [];

    [ObservableProperty]
    private bool[] _donatedFossils = [];

    [ObservableProperty]
    private bool[] _donatedArt = [];

    public MuseumEditorViewModel(Museum museum)
    {
        _museum = museum;
        LoadData();
    }

    private void LoadData()
    {
        DonatedFish = _museum.DonatedFish.ToArray();
        DonatedInsects = _museum.DonatedInsects.ToArray();
        DonatedSeaCreatures = _museum.DonatedSeaCreatures.ToArray();
        DonatedFossils = _museum.DonatedFossils.ToArray();
        DonatedArt = _museum.DonatedArt.ToArray();
    }

    public void Save()
    {
        _museum.DonatedFish = DonatedFish;
        _museum.DonatedInsects = DonatedInsects;
        _museum.DonatedSeaCreatures = DonatedSeaCreatures;
        _museum.DonatedFossils = DonatedFossils;
        _museum.DonatedArt = DonatedArt;
    }

    [RelayCommand]
    private void SetAllDonated()
    {
        for (int i = 0; i < DonatedFish.Length; i++) DonatedFish[i] = true;
        for (int i = 0; i < DonatedInsects.Length; i++) DonatedInsects[i] = true;
        for (int i = 0; i < DonatedSeaCreatures.Length; i++) DonatedSeaCreatures[i] = true;
        for (int i = 0; i < DonatedFossils.Length; i++) DonatedFossils[i] = true;
        for (int i = 0; i < DonatedArt.Length; i++) DonatedArt[i] = true;
    }

    [RelayCommand]
    private void ClearAllDonated()
    {
        for (int i = 0; i < DonatedFish.Length; i++) DonatedFish[i] = false;
        for (int i = 0; i < DonatedInsects.Length; i++) DonatedInsects[i] = false;
        for (int i = 0; i < DonatedSeaCreatures.Length; i++) DonatedSeaCreatures[i] = false;
        for (int i = 0; i < DonatedFossils.Length; i++) DonatedFossils[i] = false;
        for (int i = 0; i < DonatedArt.Length; i++) DonatedArt[i] = false;
    }
}

public partial class LandFlagEditorViewModel : ViewModelBase
{
    private readonly bool[] _flags;

    [ObservableProperty]
    private ObservableCollection<LandFlagViewModel> _flagsList = new();

    [ObservableProperty]
    private string _searchText = "";

    public LandFlagEditorViewModel(bool[] flags)
    {
        _flags = flags;
        LoadFlags();
    }

    private void LoadFlags()
    {
        FlagsList.Clear();
        for (int i = 0; i < _flags.Length; i++)
        {
            FlagsList.Add(new LandFlagViewModel
            {
                Index = i,
                Name = GameInfo.Strings.GetEventFlagLandName(i),
                IsSet = _flags[i]
            });
        }
    }

    public bool[] Save()
    {
        for (int i = 0; i < FlagsList.Count; i++)
        {
            _flags[i] = FlagsList[i].IsSet;
        }
        return _flags;
    }

    partial void OnSearchTextChanged(string value)
    {
        // Filter flags
    }

    [RelayCommand]
    private void SetAll()
    {
        foreach (var flag in FlagsList)
        {
            flag.IsSet = true;
        }
    }

    [RelayCommand]
    private void ClearAll()
    {
        foreach (var flag in FlagsList)
        {
            flag.IsSet = false;
        }
    }
}

public partial class LandFlagViewModel : ObservableObject
{
    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private bool _isSet;
}
