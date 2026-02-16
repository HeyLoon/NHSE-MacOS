using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NHSE.Core;
using System.Collections.ObjectModel;

namespace NHSE.macOS.ViewModels.Player;

public partial class RecipeEditorViewModel : ViewModelBase
{
    private readonly Player _player;

    [ObservableProperty]
    private ObservableCollection<RecipeViewModel> _recipes = new();

    [ObservableProperty]
    private bool _known;

    [ObservableProperty]
    private bool _newFlag;

    public RecipeEditorViewModel(Player player)
    {
        _player = player;
        LoadRecipes();
    }

    private void LoadRecipes()
    {
        Recipes.Clear();
        var personal = _player.Personal;
        var count = RecipeList.Recipes.Length;
        
        for (int i = 0; i < count; i++)
        {
            var recipeId = RecipeList.Recipes[i];
            var isKnown = personal.GetRecipeKnown(i);
            var isNew = personal.GetRecipeNew(i);
            
            Recipes.Add(new RecipeViewModel
            {
                Index = i,
                RecipeId = recipeId,
                RecipeName = GameInfo.Strings.GetItemName(recipeId),
                IsKnown = isKnown,
                IsNew = isNew
            });
        }
    }

    public void Save()
    {
        var personal = _player.Personal;
        foreach (var recipe in Recipes)
        {
            personal.SetRecipeKnown(recipe.Index, recipe.IsKnown);
            personal.SetRecipeNew(recipe.Index, recipe.IsNew);
        }
    }

    [RelayCommand]
    private void SetAllKnown()
    {
        foreach (var recipe in Recipes)
        {
            recipe.IsKnown = true;
        }
    }

    [RelayCommand]
    private void SetAllUnknown()
    {
        foreach (var recipe in Recipes)
        {
            recipe.IsKnown = false;
            recipe.IsNew = false;
        }
    }

    [RelayCommand]
    private void ClearNewFlags()
    {
        foreach (var recipe in Recipes)
        {
            recipe.IsNew = false;
        }
    }
}

public partial class RecipeViewModel : ObservableObject
{
    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private ushort _recipeId;

    [ObservableProperty]
    private string _recipeName = "";

    [ObservableProperty]
    private bool _isKnown;

    [ObservableProperty]
    private bool _isNew;
}

public partial class ReactionEditorViewModel : ViewModelBase
{
    private readonly Personal _personal;

    [ObservableProperty]
    private ObservableCollection<ReactionViewModel> _reactions = new();

    public ReactionEditorViewModel(Personal personal)
    {
        _personal = personal;
        LoadReactions();
    }

    private void LoadReactions()
    {
        Reactions.Clear();
        var count = GameLists.Reactions.Length;
        
        for (int i = 0; i < count; i++)
        {
            var reactionId = GameLists.Reactions[i];
            var isKnown = _personal.GetReaction(i);
            
            Reactions.Add(new ReactionViewModel
            {
                Index = i,
                ReactionId = reactionId,
                ReactionName = GameInfo.Strings.GetReactionName(reactionId),
                IsKnown = isKnown
            });
        }
    }

    public void Save()
    {
        foreach (var reaction in Reactions)
        {
            _personal.SetReaction(reaction.Index, reaction.IsKnown);
        }
    }

    [RelayCommand]
    private void SetAllKnown()
    {
        foreach (var reaction in Reactions)
        {
            reaction.IsKnown = true;
        }
    }

    [RelayCommand]
    private void SetAllUnknown()
    {
        foreach (var reaction in Reactions)
        {
            reaction.IsKnown = false;
        }
    }
}

public partial class ReactionViewModel : ObservableObject
{
    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private ushort _reactionId;

    [ObservableProperty]
    private string _reactionName = "";

    [ObservableProperty]
    private bool _isKnown;
}

public partial class AchievementEditorViewModel : ViewModelBase
{
    private readonly Personal _personal;

    [ObservableProperty]
    private ObservableCollection<AchievementViewModel> _achievements = new();

    public AchievementEditorViewModel(Personal personal)
    {
        _personal = personal;
        LoadAchievements();
    }

    private void LoadAchievements()
    {
        Achievements.Clear();
        var data = _personal.Achievements;
        
        for (int i = 0; i < data.Length; i++)
        {
            Achievements.Add(new AchievementViewModel
            {
                Index = i,
                Name = GameInfo.Strings.GetAchievementName(i),
                Value = data[i]
            });
        }
    }

    public void Save()
    {
        var data = _personal.Achievements;
        foreach (var achievement in Achievements)
        {
            data[achievement.Index] = achievement.Value;
        }
        _personal.Achievements = data;
    }
}

public partial class AchievementViewModel : ObservableObject
{
    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private uint _value;
}

public partial class FlagEditorViewModel : ViewModelBase
{
    private readonly Personal _personal;
    private bool[] _flags;

    [ObservableProperty]
    private ObservableCollection<FlagViewModel> _flagsList = new();

    [ObservableProperty]
    private string _searchText = "";

    public FlagEditorViewModel(Personal personal)
    {
        _personal = personal;
        _flags = personal.GetEventFlagsPlayer();
        LoadFlags();
    }

    private void LoadFlags()
    {
        FlagsList.Clear();
        for (int i = 0; i < _flags.Length; i++)
        {
            FlagsList.Add(new FlagViewModel
            {
                Index = i,
                Name = GameInfo.Strings.GetEventFlagName(i),
                IsSet = _flags[i]
            });
        }
    }

    public void Save()
    {
        for (int i = 0; i < FlagsList.Count; i++)
        {
            _flags[i] = FlagsList[i].IsSet;
        }
        _personal.SetEventFlagsPlayer(_flags);
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

public partial class FlagViewModel : ObservableObject
{
    [ObservableProperty]
    private int _index;

    [ObservableProperty]
    private string _name = "";

    [ObservableProperty]
    private bool _isSet;
}

public partial class MiscPlayerEditorViewModel : ViewModelBase
{
    private readonly Player _player;
    private readonly MainSave _save;

    [ObservableProperty]
    private uint _birthdayMonth;

    [ObservableProperty]
    private uint _birthdayDay;

    [ObservableProperty]
    private byte[] _playedDays = [];

    [ObservableProperty]
    private uint _playedBank;

    public MiscPlayerEditorViewModel(Player player, MainSave save)
    {
        _player = player;
        _save = save;
        LoadData();
    }

    private void LoadData()
    {
        var pers = _player.Personal;
        BirthdayMonth = pers.BirthdayMonth;
        BirthdayDay = pers.BirthdayDay;
        PlayedDays = pers.PlayedDays.ToArray();
        PlayedBank = pers.PlayedBank;
    }

    public void Save()
    {
        var pers = _player.Personal;
        pers.BirthdayMonth = BirthdayMonth;
        pers.BirthdayDay = BirthdayDay;
        pers.PlayedDays = PlayedDays;
        pers.PlayedBank = PlayedBank;
    }
}

public partial class PostBoxEditorViewModel : ViewModelBase
{
    private readonly Player _player;
    private readonly Item[] _postBox;

    [ObservableProperty]
    private ObservableCollection<ItemViewModel> _letters = new();

    public PostBoxEditorViewModel(Player player, Item[] postBox)
    {
        _player = player;
        _postBox = postBox;
        LoadLetters();
    }

    private void LoadLetters()
    {
        Letters.Clear();
        for (int i = 0; i < _postBox.Length; i++)
        {
            Letters.Add(new ItemViewModel(_postBox[i], i));
        }
    }

    public void Save()
    {
        for (int i = 0; i < Letters.Count; i++)
        {
            _postBox[i] = Letters[i].Item;
        }
    }
}

public partial class ItemReceivedEditorViewModel : ViewModelBase
{
    private readonly Player _player;

    [ObservableProperty]
    private ObservableCollection<ReceivedItemViewModel> _receivedItems = new();

    public ItemReceivedEditorViewModel(Player player)
    {
        _player = player;
        LoadItems();
    }

    private void LoadItems()
    {
        ReceivedItems.Clear();
        // Load from player's received items data
    }

    public void Save()
    {
        // Save back to player
    }
}

public partial class ReceivedItemViewModel : ObservableObject
{
    [ObservableProperty]
    private ushort _itemId;

    [ObservableProperty]
    private string _itemName = "";

    [ObservableProperty]
    private bool _isReceived;
}
