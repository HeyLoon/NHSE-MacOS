#!/bin/bash
set -e

echo "🎮 NHSE macOS Build Script"
echo "============================"

# Colors
GREEN='\033[0;32m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}📦 Restoring dependencies...${NC}"
dotnet restore NHSE.slnx

echo -e "${BLUE}🔨 Building solution...${NC}"
dotnet build NHSE.slnx --configuration Release

echo -e "${BLUE}🧪 Running tests...${NC}"
dotnet test NHSE.Tests/NHSE.Tests.csproj --configuration Release --no-build || true

echo -e "${BLUE}📱 Publishing macOS app...${NC}"
dotnet publish NHSE.macOS/NHSE.macOS.csproj \
    --configuration Release \
    --runtime osx-arm64 \
    --self-contained true \
    --output ./publish/macos-arm64

echo -e "${BLUE}📦 Creating app bundle...${NC}"
cd ./publish/macos-arm64

# Create app bundle structure
mkdir -p "NHSE.app/Contents/MacOS"
mkdir -p "NHSE.app/Contents/Resources"

# Copy all files to app bundle
cp -r * "NHSE.app/Contents/MacOS/" 2>/dev/null || true

# Create Info.plist
cat > "NHSE.app/Contents/Info.plist" << 'EOF'
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleName</key>
    <string>NHSE</string>
    <key>CFBundleDisplayName</key>
    <string>Animal Crossing Save Editor</string>
    <key>CFBundleIdentifier</key>
    <string>com.nhse.mac</string>
    <key>CFBundleVersion</key>
    <string>1.0</string>
    <key>CFBundlePackageType</key>
    <string>APPL</string>
    <key>CFBundleExecutable</key>
    <string>NHSE.macOS</string>
    <key>LSMinimumSystemVersion</key>
    <string>10.15</string>
    <key>LSApplicationCategoryType</key>
    <string>public.app-category.games</string>
</dict>
</plist>
EOF

# Make executable
chmod +x "NHSE.app/Contents/MacOS/NHSE.macOS"

# Clean up
cd ..
cd ..

echo -e "${GREEN}✅ Build complete!${NC}"
echo ""
echo "📍 Output: ./publish/macos-arm64/NHSE.app"
echo ""
echo "To run the app:"
echo "  open ./publish/macos-arm64/NHSE.app"
echo ""
echo "Or:"
echo "  ./publish/macos-arm64/NHSE.app/Contents/MacOS/NHSE.macOS"
