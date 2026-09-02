@echo off
REM CAIMAN Backend Quick Start Script for Windows

echo.
echo 🐊 CAIMAN Backend - Quick Start
echo ================================
echo.

REM Check Node.js
node --version >nul 2>&1
if errorlevel 1 (
    echo ❌ Node.js not found. Please install Node.js 18+
    echo    Visit: https://nodejs.org
    pause
    exit /b 1
)

for /f "tokens=*" %%i in ('node --version') do set NODE_VERSION=%%i
echo ✅ Node.js: %NODE_VERSION%

REM Check MongoDB
mongod --version >nul 2>&1
if errorlevel 1 (
    echo.
    echo ⚠️  MongoDB not found
    echo.
    echo Options:
    echo 1. Download from: https://www.mongodb.com/try/download/community
    echo 2. Use Docker (recommended):
    echo    docker-compose up -d
    echo.
    set /p CONTINUE="Continue anyway? (y/n): "
    if /i not "%CONTINUE%"=="y" (
        exit /b 1
    )
) else (
    echo ✅ MongoDB installed
)

echo.
echo 📦 Installing dependencies...
call npm install

echo.
echo ⚙️  Checking .env file...
if not exist .env (
    echo ⚠️  .env not found. Creating from .env.example
    copy .env.example .env
    echo ✅ Created .env - adjust values if needed
)

echo.
echo 🚀 Starting CAIMAN Backend...
echo.
call npm start
