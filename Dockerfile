FROM mcr.microsoft.com/dotnet/sdk:10.0-preview

WORKDIR /app

# Install dependencies for macOS build
RUN apt-get update && apt-get install -y \
    clang \
    zlib1g-dev \
    libkrb5-dev \
    libssl-dev \
    && rm -rf /var/lib/apt/lists/*

# Copy project files
COPY . .

# Restore dependencies
RUN dotnet restore NHSE.slnx

# Build the solution
RUN dotnet build NHSE.slnx --configuration Release

# Test
RUN dotnet test NHSE.slnx --no-build

# Publish
RUN dotnet publish NHSE.macOS/NHSE.macOS.csproj \
    --configuration Release \
    --runtime osx-arm64 \
    --self-contained true \
    --output /app/publish
