@echo off
REM CAIMAN Mobile App - MAUI Installation Script for Windows

echo.
echo 🐊 CAIMAN Mobile - MAUI Setup
echo ==============================
echo.

REM Check .NET SDK
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ❌ .NET SDK not found. Please install .NET 8 SDK
    echo    Visit: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo ✅ .NET SDK: %DOTNET_VERSION%

echo.
echo 📦 Installing MAUI workloads...
echo.

REM Install MAUI
call dotnet workload install maui

REM Install Android
echo.
echo 📦 Installing Android workload...
call dotnet workload install android

echo.
echo ✅ Workloads installed successfully!
echo.
echo 🚀 To run the app:
echo.
echo    Android (emulator/device):
echo    dotnet maui run -f net8.0-android
echo.
echo 📝 Before running, edit Services\ApiService.cs and set:
echo    private const string BaseUrl = "http://your-backend:3000";
echo.
pause
