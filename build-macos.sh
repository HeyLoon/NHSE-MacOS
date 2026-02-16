#!/bin/bash

# Build script for NHSE macOS Avalonia port

echo "Building NHSE.macOS..."
cd "$(dirname "$0")/NHSE-MacOS"

dotnet restore NHSE.macOS/NHSE.macOS.csproj
if [ $? -ne 0 ]; then
    echo "Restore failed!"
    exit 1
fi

dotnet build NHSE.macOS/NHSE.macOS.csproj --configuration Release
if [ $? -ne 0 ]; then
    echo "Build failed!"
    exit 1
fi

echo "Build successful!"
echo ""
echo "To run the application:"
echo "  dotnet run --project NHSE.macOS/NHSE.macOS.csproj"
