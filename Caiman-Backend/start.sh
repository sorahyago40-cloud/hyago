#!/bin/bash

# CAIMAN Backend Quick Start Script for macOS/Linux

echo "🐊 CAIMAN Backend - Quick Start"
echo "================================"
echo ""

# Check Node.js
if ! command -v node &> /dev/null; then
    echo "❌ Node.js not found. Please install Node.js 18+"
    echo "   Visit: https://nodejs.org"
    exit 1
fi

echo "✅ Node.js: $(node -v)"

# Check MongoDB
if ! command -v mongod &> /dev/null; then
    echo ""
    echo "⚠️  MongoDB not found in PATH"
    echo ""
    echo "Options:"
    echo "1. Instalar MongoDB:"
    echo "   brew install mongodb-community"
    echo "   brew services start mongodb-community"
    echo ""
    echo "2. Usar Docker (recomendado):"
    echo "   docker-compose up -d"
    echo ""
    read -p "Continue anyway? (y/n) " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
else
    echo "✅ MongoDB: $(mongod --version | head -n 1)"
fi

echo ""
echo "📦 Installing dependencies..."
npm install

echo ""
echo "⚙️  Checking .env file..."
if [ ! -f .env ]; then
    echo "⚠️  .env not found. Creating from .env.example"
    cp .env.example .env
    echo "✅ Created .env - adjust values if needed"
fi

echo ""
echo "🚀 Starting CAIMAN Backend..."
echo ""
npm start
