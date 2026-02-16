using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NHSE.Core;
using NHSE.Villagers;
using System.Collections.ObjectModel;
using System.Linq;

namespace NHSE.macOS.ViewModels;

public partial class VillagerEditorViewModel : ViewModelBase
{
    private readonly HorizonSave _sav;
    private IVillager[] _villagers;
    private Personal _origin;

    [ObservableProperty]
    private ObservableCollection<VillagerViewModel> _villagerList = new();

    [ObservableProperty]
    private int _selectedVillagerIndex;

    [ObservableProperty]
    private VillagerViewModel? _selectedVillager;

    public IVillager[] Villagers
    {
        get => _villagers;
        set
        {
            _villagers = value;
            Reload();
        }
    }

    public Personal Origin
    {
        get => _origin;
        set => _origin = value;
    }

    public VillagerEditorViewModel(IVillager[] villagers, Personal origin, HorizonSave sav)
    {
        _villagers = villagers;
        _origin = origin;
        _sav = sav;
        Reload();
    }

    public void Reload()
    {
        VillagerList.Clear();
        for (int i = 0; i < _villagers.Length; i++)
        {
            VillagerList.Add(new VillagerViewModel(_villagers[i], i));
        }
        
        if (VillagerList.Count > 0)
        {
            SelectedVillagerIndex = 0;
        }
    }

    public void Save()
    {
        foreach (var vm in VillagerList)
        {
            vm.Save();
        }
    }

    partial void OnSelectedVillagerIndexChanged(int value)
    {
        if (value >= 0 && value < VillagerList.Count)
        {
            SelectedVillager = VillagerList[value];
        }
    }

    [RelayCommand]
    private void EditVillagerMemory()
    {
        // Open memory editor
    }

    [RelayCommand]
    private void EditVillagerFlags()
    {
        // Open flag editor
    }

    [RelayCommand]
    private void EditDIYTimer()
    {
        // Open DIY timer editor
    }

    [RelayCommand]
    private void EditSaveRoom()
    {
        // Open save room editor
    }
}

public partial class VillagerViewModel : ViewModelBase
{
    private readonly IVillager _villager;
    private readonly int _index;

    [ObservableProperty]
    private string _villagerName = "";

    [ObservableProperty]
    private ushort _villagerId;

    [ObservableProperty]
    private byte[] _data;

    public int Index => _index;
    public IVillager Villager => _villager;

    public VillagerViewModel(IVillager villager, int index)
    {
        _villager = villager;
        _index = index;
        _data = villager.Data.ToArray();
        
        var str = GameInfo.Strings;
        VillagerId = villager.VillagerId;
        VillagerName = VillagerResources.GetVillagerName(VillagerId, str);
    }

    public void Save()
    {
        // Save villager data back
        _villager.Data = _data.ToArray();
    }
}
