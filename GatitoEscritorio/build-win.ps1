# build-win.ps1
Write-Host "🏗️  Construyendo para Windows..." -ForegroundColor Cyan
dotnet publish -c Release -r win-x64 --self-contained
Write-Host "✅ Ejecutable en: bin/Release/net8.0/win-x64/publish/GatitoEscritorio.exe" -ForegroundColor Green
