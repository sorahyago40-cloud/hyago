#!/bin/bash

# CAIMAN Mobile App - MAUI Installation Script for macOS/Linux

echo "🐊 CAIMAN Mobile - MAUI Setup"
echo "=============================="
echo ""

# Check .NET SDK
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK not found. Please install .NET 8 SDK"
    echo "   Visit: https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

DOTNET_VERSION=$(dotnet --version)
echo "✅ .NET SDK: $DOTNET_VERSION"

if [[ ! $DOTNET_VERSION == 8.* ]]; then
    echo "⚠️  .NET 8 is required (found $DOTNET_VERSION)"
    echo "   Please install .NET 8 SDK"
    exit 1
fi

echo ""
echo "📦 Installing MAUI workloads..."
echo ""

# Install MAUI
dotnet workload install maui

# Install Android workload
echo ""
echo "📦 Installing Android workload..."
dotnet workload install android

# Check for iOS on macOS
if [[ "$OSTYPE" == "darwin"* ]]; then
    echo ""
    echo "📦 Installing iOS workload..."
    dotnet workload install ios
fi

echo ""
echo "✅ All workloads installed successfully!"
echo ""
echo "🚀 To run the app:"
echo ""
echo "   Android (emulator/device):"
echo "   dotnet maui run -f net8.0-android"
echo ""

if [[ "$OSTYPE" == "darwin"* ]]; then
    echo "   iOS (simulator/device - Mac only):"
    echo "   dotnet maui run -f net8.0-ios"
    echo ""
fi

echo "📝 Before running, edit Services/ApiService.cs and set:"
echo "   private const string BaseUrl = \"http://your-backend:3000\";"
echo ""
