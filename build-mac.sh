#!/bin/bash
# build-mac.sh
echo "🏗️  Construyendo para macOS..."
dotnet publish -c Release -r osx-x64 --self-contained
echo "✅ Ejecutable en: bin/Release/net8.0/osx-x64/publish/DesktopCat"
