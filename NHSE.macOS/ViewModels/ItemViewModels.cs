using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NHSE.Core;
using NHSE.Sprites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace NHSE.macOS.ViewModels;

public partial class ItemEditorViewModel : ViewModelBase
{
    [ObservableProperty]
    private Item _currentItem = new(Item.NONE);

    [ObservableProperty]
    private ObservableCollection<ComboItem> _allItems = new();

    [ObservableProperty]
    private ObservableCollection<ComboItem> _recipes = new();

    [ObservableProperty]
    private ObservableCollection<ComboItem> _fossils = new();

    [ObservableProperty]
    private ushort _selectedItemId;

    [ObservableProperty]
    private ushort _itemCount;

    [ObservableProperty]
    private ushort _useCount;

    [ObservableProperty]
    private byte _systemParam;

    [ObservableProperty]
    private byte _additionalParam;

    [ObservableProperty]
    private bool _isFlower;

    [ObservableProperty]
    private bool _isWrapped;

    [ObservableProperty]
    private int _wrappingType;

    [ObservableProperty]
    private int _wrappingPaper;

    [ObservableProperty]
    private bool _wrappingShowItem;

    [ObservableProperty]
    private bool _wrapping80;

    [ObservableProperty]
    private bool _isExtension;

    [ObservableProperty]
    private byte _extensionX;

    [ObservableProperty]
    private byte _extensionY;

    [ObservableProperty]
    private ushort _extensionItemId;

    [ObservableProperty]
    private bool _isRecipe;

    [ObservableProperty]
    private bool _isFossil;

    [ObservableProperty]
    private bool _isMessageBottle;

    [ObservableProperty]
    private bool _showFlowerGenes;

    [ObservableProperty]
    private bool _isWatered;

    [ObservableProperty]
    private bool _isWateredGold;

    [ObservableProperty]
    private int _waterDays;

    [ObservableProperty]
    private FlowerGene _flowerGenes;

    public ItemEditorViewModel()
    {
        InitializeItemLists();
    }

    private void InitializeItemLists()
    {
        var strings = GameInfo.Strings;
        foreach (var item in strings.ItemDataSource)
        {
            AllItems.Add(item);
        }
        
        foreach (var recipe in strings.CreateItemDataSource(RecipeList.Recipes, false))
        {
            Recipes.Add(recipe);
        }
        
        foreach (var fossil in strings.CreateItemDataSource(GameLists.Fossils, false))
        {
            Fossils.Add(fossil);
        }
    }

    public void LoadItem(Item item)
    {
        CurrentItem = item;
        SelectedItemId = item.ItemId;
        var kind = ItemInfo.GetItemKind(item.ItemId);

        IsFlower = kind.IsFlowerGene(item.ItemId);
        IsRecipe = kind == ItemKind.Kind_DIYRecipe || kind == ItemKind.Kind_MessageBottle;
        IsFossil = kind == ItemKind.Kind_FossilUnknown;
        IsMessageBottle = kind == ItemKind.Kind_MessageBottle;
        ShowFlowerGenes = IsFlower;

        if (IsFlower)
        {
            FlowerGenes = item.Genes;
            IsWateredGold = item.IsWateredGold;
            IsWatered = item.IsWatered;
            WaterDays = item.DaysWatered;
        }
        else
        {
            ItemCount = item.Count;
            UseCount = item.UseCount;
            SystemParam = item.SystemParam;
        }

        if (kind == ItemKind.Kind_MessageBottle || item.ItemId >= 60_000)
        {
            AdditionalParam = item.AdditionalParam;
        }
        else
        {
            IsWrapped = item.WrappingType != 0;
            WrappingType = (int)item.WrappingType;
            WrappingPaper = (int)item.WrappingPaper;
            WrappingShowItem = item.WrappingShowItem;
            Wrapping80 = item.Wrapping80;
        }

        if (item.ItemId == Item.EXTENSION)
        {
            IsExtension = true;
            ExtensionItemId = item.ExtensionItemId;
            ExtensionX = item.ExtensionX;
            ExtensionY = item.ExtensionY;
        }
    }

    public Item SaveItem()
    {
        var item = new Item();
        
        if (IsExtension)
        {
            item.ItemId = Item.EXTENSION;
            item.ExtensionItemId = ExtensionItemId;
            item.ExtensionX = ExtensionX;
            item.ExtensionY = ExtensionY;
        }
        else
        {
            item.ItemId = SelectedItemId;
            var kind = ItemInfo.GetItemKind(SelectedItemId);

            if (kind.IsFlowerGene(SelectedItemId))
            {
                item.Genes = FlowerGenes;
                item.DaysWatered = WaterDays;
                item.IsWateredGold = IsWateredGold;
                item.IsWatered = IsWatered;
            }
            else
            {
                item.Count = ItemCount;
                item.UseCount = UseCount;
                item.SystemParam = SystemParam;
            }

            if (kind == ItemKind.Kind_MessageBottle || SelectedItemId >= 60_000)
            {
                item.AdditionalParam = AdditionalParam;
            }
            else if (IsWrapped)
            {
                item.SetWrapping((ItemWrapping)WrappingType, (ItemWrappingPaper)WrappingPaper, WrappingShowItem, Wrapping80);
            }
        }

        CurrentItem = item;
        return item;
    }

    partial void OnSelectedItemIdChanged(ushort value)
    {
        var kind = ItemInfo.GetItemKind(value);
        IsFlower = kind.IsFlowerGene(value);
        IsRecipe = kind == ItemKind.Kind_DIYRecipe || kind == ItemKind.Kind_MessageBottle;
        IsFossil = kind == ItemKind.Kind_FossilUnknown;
        IsMessageBottle = kind == ItemKind.Kind_MessageBottle;
        ShowFlowerGenes = IsFlower;
    }

    [RelayCommand]
    private void CopyItemToClipboard()
    {
        var item = SaveItem();
        var data = item.ToBytesClass();
        var u64 = System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(data);
        // Copy to clipboard
    }

    [RelayCommand]
    private void PasteItemFromClipboard()
    {
        // Paste from clipboard
    }
}

public partial class PlayerItemEditorViewModel : ViewModelBase
{
    private readonly Item[] _items;
    private readonly int _width;
    private readonly int _height;
    private readonly bool _sysbot;

    [ObservableProperty]
    private ObservableCollection<ItemViewModel> _itemGrid = new();

    [ObservableProperty]
    private ItemViewModel? _selectedItem;

    [ObservableProperty]
    private ItemEditorViewModel _itemEditor = new();

    public Item[] Items => _items;

    public PlayerItemEditorViewModel(Item[] items, int width, int height, bool sysbot = false)
    {
        _items = items;
        _width = width;
        _height = height;
        _sysbot = sysbot;
        LoadItems();
    }

    private void LoadItems()
    {
        ItemGrid.Clear();
        for (int i = 0; i < _items.Length; i++)
        {
            ItemGrid.Add(new ItemViewModel(_items[i], i));
        }
    }

    [RelayCommand]
    private void SaveItem()
    {
        if (SelectedItem != null)
        {
            SelectedItem.Item = ItemEditor.SaveItem();
            _items[SelectedItem.Index] = SelectedItem.Item;
        }
    }

    [RelayCommand]
    private void DumpItems()
    {
        // Dump items to file
    }

    [RelayCommand]
    private void LoadItemsFromFile()
    {
        // Load items from file
    }

    [RelayCommand]
    private void InjectItems()
    {
        if (_sysbot)
        {
            // Inject items via sysbot
        }
    }

    partial void OnSelectedItemChanged(ItemViewModel? value)
    {
        if (value != null)
        {
            ItemEditor.LoadItem(value.Item);
        }
    }
}

public partial class ItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private Item _item;

    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private string _itemName = "";

    public ItemViewModel(Item item, int index)
    {
        _item = item;
        _index = index;
        UpdateItemName();
    }

    partial void OnItemChanged(Item value)
    {
        UpdateItemName();
    }

    private void UpdateItemName()
    {
        var strings = GameInfo.Strings;
        ItemName = strings.GetItemName(Item.ItemId);
    }
}
