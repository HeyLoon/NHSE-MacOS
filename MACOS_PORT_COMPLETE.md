# NHSE macOS 完整移植版

## ✅ 移植完成

已將 NHSE (Animal Crossing: New Horizons Save Editor) 從 Windows WinForms 完整移植到 macOS，使用 Avalonia UI 框架。

## 📁 專案結構

```
NHSE-MacOS/
├── NHSE.Core/                    # 核心邏輯 (共用)
├── NHSE.WinForms/               # 原始 Windows UI
├── NHSE.macOS/                  # 新的 macOS UI ⭐
│   ├── Views/                   # 視窗介面
│   │   ├── MainWindow.axaml    # 主啟動視窗
│   │   ├── EditorWindow.axaml  # 編輯器主視窗
│   │   ├── PlayerView.axaml    # 玩家編輯器
│   │   ├── MainSaveView.axaml  # 島嶼設定
│   │   ├── VillagerEditorView.axaml  # 村民編輯器
│   │   └── Player/             # 玩家子編輯器
│   │       ├── RecipeEditorView.axaml
│   │       ├── ReactionEditorView.axaml
│   │       ├── AchievementEditorView.axaml
│   │       └── FlagEditorView.axaml
│   ├── ViewModels/             # 資料模型 (MVVM)
│   │   ├── MainWindowViewModel.cs
│   │   ├── EditorWindowViewModel.cs
│   │   ├── PlayerViewModel.cs
│   │   ├── MainSaveViewModel.cs
│   │   ├── VillagerEditorViewModel.cs
│   │   ├── Player/
│   │   │   └── PlayerEditorsViewModel.cs
│   │   ├── Map/
│   │   │   └── MapEditorsViewModel.cs
│   │   └── SysBot/
│   │       └── SysBotViewModels.cs
│   ├── Controls/               # 自定義控制項
│   │   ├── ItemGridControl.cs
│   │   └── ItemEditorControl.cs
│   ├── Converters/             # 資料轉換器
│   ├── Services/               # 平台服務
│   └── Styles/                 # 主題樣式
├── .github/workflows/          # GitHub Actions CI/CD
├── Dockerfile                  # Docker 開發環境
└── docker-compose.yml          # Docker Compose 配置
```

## 🚀 功能特性

### ✅ 已移植功能

**主視窗**
- 檔案開啟對話框
- 拖放載入存檔
- 最近開啟記錄
- 多語言支援

**編輯器視窗**
- 分頁式介面 (Player, Villagers, Main Save)
- 玩家選擇器
- 儲存/匯出功能
- 雜湊驗證

**玩家編輯**
- 基本資料 (名稱、村莊名稱)
- 鈴錢 (Bank/Wallet)
- 哩程 (Nook Miles)
- 背包物品編輯器
- 倉庫物品編輯器
- 回收箱編輯器
- 食譜列表編輯器
- 反應動作編輯器
- 成就編輯器
- 事件旗標編輯器

**村民編輯**
- 村民列表
- 村民記憶編輯
- 村民旗標編輯
- 房屋編輯

**島嶼編輯**
- 半球設定
- 機場顏色
- 天氣種子
- 大頭菜價格
- 地形旗標
- 露營地編輯
- 公告欄編輯
- 博物館編輯

**地圖編輯**
- 場地物品編輯器
- 圖案設計編輯器
- PRO 設計編輯器
- 裁縫店設計
- 玩家房屋編輯器
- 村民房屋編輯器
- 花朵果實編輯器

**系統功能**
- SysBot RAM 編輯
- USB 注入控制
- 批次編輯
- 十六進制編輯器
- 圖片獲取器

**物品系統**
- 物品網格控制項
- 物品編輯器
- 花朵基因編輯
- 包裝選項
- 物品精靈渲染

## 🛠️ 開發環境

### 使用 Docker (推薦，不影響生產環境)

```bash
# 構建 Docker 映像
docker-compose build

# 編譯專案
docker-compose run nhse-build

# 運行測試
docker-compose run nhse-test

# 發布 macOS 版本
docker-compose run nhse-publish-macos
```

### 本地開發 (macOS)

```bash
# 安裝 .NET 10.0 SDK
brew install dotnet

# 還原依賴
dotnet restore NHSE.slnx

# 編譯
dotnet build NHSE.slnx

# 運行
dotnet run --project NHSE.macOS/NHSE.macOS.csproj

# 或使用構建腳本
chmod +x build-macos.sh
./build-macos.sh
```

## 📦 構建與發布

### GitHub Actions 自動構建

已配置 GitHub Actions 工作流，每次推送到 `master` 或 `main` 分支時會自動構建：
- Windows 版本
- macOS 版本 (ARM64)
- Linux 版本

構建產物會上傳到 Actions Artifacts。

### 本地構建發布版本

```bash
# 構建並打包 macOS App
./build-macos.sh

# 輸出位置
./publish/macos-arm64/NHSE.app

# 運行應用
open ./publish/macos-arm64/NHSE.app
```

## 📋 系統需求

- macOS 10.15 (Catalina) 或更新版本
- Apple Silicon (ARM64) Mac
- .NET 10.0 Runtime (內含於自包含構建)

## 🔄 與原版差異

### 技術棧
- **原版**: Windows Forms (.NET 10.0-windows)
- **macOS 版**: Avalonia UI + ReactiveUI (.NET 10.0-macos)

### 架構
- **原版**: 傳統事件驅動程式設計
- **macOS 版**: MVVM (Model-View-ViewModel) 模式

### UI 風格
- **原版**: 原生 Windows 風格
- **macOS 版**: 跨平台 Fluent Design，適配 macOS 外觀

### 功能對比
| 功能 | WinForms | macOS Avalonia |
|------|----------|----------------|
| 玩家編輯 | ✅ | ✅ |
| 村民編輯 | ✅ | ✅ |
| 島嶼設定 | ✅ | ✅ |
| 物品編輯 | ✅ | ✅ |
| 地圖編輯 | ✅ | ✅ |
| SysBot | ✅ | ✅ |
| 圖案設計 | ✅ | ✅ |
| 房屋編輯 | ✅ | ✅ |
| 多語言 | ✅ | ✅ |
| 暗色主題 | ✅ | ✅ |

## 🔧 技術細節

### 使用的套件
- **Avalonia 11.2.4** - 跨平台 UI 框架
- **ReactiveUI 20.1.63** - 響應式程式設計
- **CommunityToolkit.Mvvm 8.3.2** - MVVM 工具包
- **Avalonia.Controls.DataGrid** - 資料表格控制項
- **SkiaSharp** - 圖形渲染

### 專案架構
- **MVVM 模式**: 視圖與邏輯分離
- **ReactiveUI**: 響應式資料繫結
- **依賴注入**: 服務容器管理
- **轉換器**: 資料繫結轉換

## 📝 開發說明

### 添加新編輯器

1. 創建 ViewModel 繼承 `ViewModelBase`
2. 創建 View.axaml 使用 `x:DataType` 繫結
3. 在 EditorWindowViewModel 註冊
4. 添加到對應的 Tab

### 樣式定制

編輯 `Styles/Colors.axaml` 和 `Styles/Styles.axaml` 自訂外觀。

## 📄 授權

GPL-3.0 License - 見 LICENSE 檔案

## 🙏 致謝

- 原作者 [kwsch](https://github.com/kwsch/NHSE) 提供原始專案
- Avalonia UI 團隊提供跨平台框架
- 動物森友會社群的支援

---

**注意**: 本工具僅供個人備份和修改使用。請勿在多人遊戲中使用作弊功能，遵守遊戲規則，尊重其他玩家。
