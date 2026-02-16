# NHSE macOS Avalonia UI Port

This is a complete port of the NHSE (Animal Crossing: New Horizons Save Editor) WinForms application to Avalonia UI for macOS.

## Architecture

### MVVM Pattern
The application follows the Model-View-ViewModel (MVVM) pattern with ReactiveUI and CommunityToolkit.Mvvm for data binding.

### Project Structure

```
NHSE.macOS/
├── App.axaml                     # Application definition
├── App.axaml.cs                  # Application code-behind
├── Program.cs                    # Entry point
├── ViewLocator.cs                # View location service
├── NHSE.macOS.csproj             # Project file
│
├── Views/                        # XAML Views
│   ├── MainWindow.axaml          # Main launcher window
│   ├── EditorWindow.axaml        # Main editor window
│   ├── PlayerView.axaml          # Player editor view
│   ├── MainSaveView.axaml        # Main save settings view
│   ├── VillagerEditorView.axaml  # Villager editor view
│   └── Player/                   # Player-specific editors
│       ├── RecipeEditorView.axaml
│       ├── ReactionEditorView.axaml
│       ├── AchievementEditorView.axaml
│       └── FlagEditorView.axaml
│
├── ViewModels/                   # ViewModels
│   ├── ViewModelBase.cs          # Base ViewModel class
│   ├── MainWindowViewModel.cs    # Main window logic
│   ├── EditorWindowViewModel.cs  # Editor window logic
│   ├── PlayerViewModel.cs        # Player data logic
│   ├── MainSaveViewModel.cs      # Main save logic
│   ├── VillagerEditorViewModel.cs
│   ├── ItemViewModels.cs         # Item-related ViewModels
│   ├── Player/                   # Player-specific ViewModels
│   │   └── PlayerEditorsViewModel.cs
│   ├── Map/                      # Map editor ViewModels
│   │   └── MapEditorsViewModel.cs
│   └── SysBot/                   # SysBot ViewModels
│       └── SysBotViewModels.cs
│
├── Controls/                     # Custom Avalonia controls
│   ├── ItemGridControl.cs        # Grid-based item display
│   └── ItemEditorControl.cs      # Item editing control
│
├── Services/                     # Platform services
│   └── DialogService.cs          # Dialog and file operations
│
├── Converters/                   # Value converters
│   ├── CommonConverters.cs       # Bool, int, enum converters
│   └── ImageConverters.cs        # Item and byte array to image
│
├── Helpers/                      # Utility helpers
│   └── FileHelpers.cs            # File operations
│
└── Styles/                       # Styling resources
    ├── Colors.axaml              # Color definitions
    └── Styles.axaml              # Control themes

```

## Features Ported

### Main Window
- File open/save dialogs
- Recent files list
- Settings persistence
- Theme switching (Light/Dark/System)
- Drag and drop support

### Editor Window
- Player selection
- Tab-based navigation
- Save/Dump/Verify operations
- Language selection

### Player Editor
- Player profile (name, town, photo)
- Financial management (Bells, Nook Miles, Poki)
- Inventory counts
- Quick access to sub-editors:
  - Items/Storage editor
  - Recipe editor
  - Reaction editor
  - Achievement editor
  - Flag editor
  - PostBox editor
  - Misc editor

### Villager Editor
- Villager list with selection
- Basic villager data display
- Editor launchers for:
  - Memory editor
  - Flags editor
  - DIY timer editor
  - Save room editor

### Main Save Editor
- Island settings:
  - Hemisphere (North/South)
  - Airport color
  - Weather seed
  - Tour settings
- Map editor access:
  - Field items
  - Land flags
  - Player houses
  - Campsite
  - Fruits & flowers
  - Recycle bin
- Design editors:
  - Patterns
  - PRO designs
  - Pattern flag
  - Tailor designs
- Other editors:
  - Museum
  - Bulletin
  - Field goods
  - Visitors
  - Turnip exchange

### Item System
- Item editor with:
  - Item selection from game data
  - Count/Uses editing
  - Flag editing
  - Wrapping options
  - Flower gene editing
  - Extension item support
- Item grid display
- Drag and drop support
- Import/Export (.nhi files)

### Player Sub-Editors

#### Recipe Editor
- Full recipe list with checkboxes
- Known/New flags
- Batch operations (Set All, Clear All)

#### Reaction Editor
- Full reaction list
- Known flags
- Batch operations

#### Achievement Editor
- Achievement name list
- Value editing
- DataGrid interface

#### Flag Editor
- Event flags list
- Search functionality
- Set/Clear All operations
- Checkboxes for quick toggling

### Map Editors

#### Field Item Editor
- Map grid display
- Item selection
- Bulk spawn operations
- Import/Export layer data

#### Pattern Editor
- Pattern list
- Image preview
- Import/Export patterns

#### House Editors
- Player houses
- Villager houses

#### Museum Editor
- Fish, insects, sea creatures
- Fossils
- Art
- Donation tracking

### SysBot Integration
- Network connection (IP/Port)
- USB connection
- Memory read/write
- Auto-injector
- Pocket injection
- Batch editor
- Hex editor

## Value Converters

- `BoolToVisibilityConverter`: Boolean to visibility
- `InverseBoolConverter`: Inverts boolean
- `EnumToIntConverter`: Enum to integer for ComboBox
- `IntToStringConverter`: Integer to string
- `ByteArrayToImageConverter`: Photo data to image
- `ItemToImageConverter`: Item to sprite image
- `ItemIdToImageConverter`: Item ID to sprite

## Custom Controls

### ItemGridControl
- Custom drawn grid control
- Displays items as sprites
- Selection support
- Click handling

### ItemEditorControl
- Comprehensive item editing
- Dynamic UI based on item type
- Flower gene editing
- Wrapping options
- Extension item support

## Styling

### Themes
- Light theme (default)
- Dark theme support
- System theme detection

### Color Scheme
- Primary: Blue (#2196F3)
- Accent: Pink (#FF4081)
- Success: Green (#4CAF50)
- Warning: Yellow (#FFC107)
- Error: Red (#F44336)

### Card Style
- Elevated cards with shadows
- Consistent padding and margins
- Border treatment

## Services

### DialogService
- Information/Warning/Error dialogs
- Confirmation dialogs
- File open/save dialogs
- Folder browser dialogs

### ClipboardService
- Text clipboard operations
- Item hex string support

### NotificationService
- macOS native notifications

## Dependencies

```xml
<PackageReference Include="Avalonia" Version="11.2.4" />
<PackageReference Include="Avalonia.Desktop" Version="11.2.4" />
<PackageReference Include="Avalonia.Themes.Fluent" Version="11.2.4" />
<PackageReference Include="Avalonia.Controls.DataGrid" Version="11.2.4" />
<PackageReference Include="Avalonia.ReactiveUI" Version="11.2.4" />
<PackageReference Include="ReactiveUI" Version="20.1.63" />
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.3.2" />
<PackageReference Include="SkiaSharp.NativeAssets.macOS" Version="3.116.1" />
```

## Building

```bash
dotnet build NHSE.macOS/NHSE.macOS.csproj
```

## Running

```bash
dotnet run --project NHSE.macOS/NHSE.macOS.csproj
```

## Notes

- Uses compiled bindings for performance
- Supports macOS 10.15+
- ARM64 native support
- Respects macOS UI conventions
- Integrated with existing NHSE.Core, NHSE.Injection, NHSE.Sprites, NHSE.Villagers libraries
