# NHSE macOS Port

This is a macOS port of NHSE (Animal Crossing: New Horizons Save Editor), originally a Windows WinForms application.

## Project Structure

- `NHSE.Core` - Core save editing logic (shared)
- `NHSE.WinForms` - Original Windows UI
- `NHSE.macOS` - New macOS UI using Avalonia
- `NHSE.Tests` - Unit tests

## Development Setup

### Prerequisites

- Docker (for isolated development environment)
- Git

### Using Docker (Recommended for isolated development)

1. Build the Docker image:
```bash
docker-compose build
```

2. Build the project:
```bash
docker-compose run nhse-build
```

3. Run tests:
```bash
docker-compose run nhse-test
```

4. Publish for macOS:
```bash
docker-compose run nhse-publish-macos
```

### Local Development (macOS)

If you prefer to develop locally without Docker:

1. Install .NET 10.0 SDK:
```bash
brew install dotnet
```

2. Restore dependencies:
```bash
dotnet restore NHSE.slnx
```

3. Build:
```bash
dotnet build NHSE.slnx
```

4. Run macOS app:
```bash
dotnet run --project NHSE.macOS/NHSE.macOS.csproj
```

## Building for macOS

### Using GitHub Actions

The project includes a GitHub Actions workflow that automatically builds for:
- Windows (original)
- macOS (arm64)
- Linux

To trigger a build, push to the `master`, `main`, or `macos-port` branch.

### Manual Build

To create a self-contained macOS app:

```bash
dotnet publish NHSE.macOS/NHSE.macOS.csproj \
  --configuration Release \
  --runtime osx-arm64 \
  --self-contained true \
  --output ./publish/macos
```

This will create a `NHSE.app` bundle in the output directory.

## Port Status

### Completed
- [x] Basic project structure
- [x] GitHub Actions workflow
- [x] Docker development environment
- [x] Main window with file open dialog
- [x] Drag and drop support
- [x] Basic editor window layout

### In Progress
- [ ] Player editor UI
- [ ] Villager editor UI
- [ ] Island editor UI
- [ ] Item editor UI

### Not Started
- [ ] Settings persistence
- [ ] Automatic backups
- [ ] Injection support
- [ ] Sprite rendering

## Contributing

This is a work in progress. The WinForms UI has many features that need to be ported to the macOS version. Contributions are welcome!

## License

GPL-3.0 - See LICENSE file
