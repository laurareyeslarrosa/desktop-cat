#!/bin/bash
# build-linux.sh
echo "🏗️  Construyendo para Linux..."
dotnet publish -c Release -r linux-x64 --self-contained
echo "✅ Ejecutable en: bin/Release/net8.0/linux-x64/publish/DesktopCat"
