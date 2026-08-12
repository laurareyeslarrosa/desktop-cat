; Installer.iss
; Script para Inno Setup

[Setup]
; Información básica
AppName=DesktopCat
AppVersion=1.0.0
AppPublisher=Laura

; Configuración del instalador
DefaultDirName={pf}\DesktopCat
DefaultGroupName=DesktopCat
AllowNoIcons=yes
UninstallDisplayIcon={app}\DesktopCat.exe

; Compresión y tamaño
Compression=lzma2
SolidCompression=yes
OutputDir=installers
OutputBaseFilename=DesktopCat-Setup

; Permisos y requisitos
PrivilegesRequired=admin
SetupIconFile=Assets\icons\desktop-cat.ico
Uninstallable=yes

; Idiomas
ShowLanguageDialog=yes
LanguageDetectionMethod=uilanguage

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
; Copiar TODOS los archivos de la carpeta Releases\Windows
Source: "Releases\Windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
; Atajo en el menú inicio
Name: "{group}\DesktopCat"; Filename: "{app}\DesktopCat.exe"; WorkingDir: "{app}"
; Atajo en el escritorio
Name: "{commondesktop}\DesktopCat"; Filename: "{app}\DesktopCat.exe"; WorkingDir: "{app}"

[Run]
; Ejecutar la app después de instalar (opcional)
Filename: "{app}\DesktopCat.exe"; Description: "Ejecutar DesktopCat"; Flags: postinstall nowait skipifsilent

[UninstallDelete]
; Limpiar archivos residuales
Type: filesandordirs; Name: "{app}"

[Registry]
; Registrar para desinstalación (opcional)
Root: HKCU; Subkey: "Software\DesktopCat"; ValueType: string; ValueName: "InstallPath"; ValueData: "{app}"; Flags: uninsdeletekey