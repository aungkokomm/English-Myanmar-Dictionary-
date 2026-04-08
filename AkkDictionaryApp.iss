; Inno Setup Script for AKK En-to-MM Dictionary
; .NET 8 WPF Application with Desktop Shortcut Support
; Installs to Program Files with full uninstall support

#define MyAppName "AKK En-to-MM Dictionary"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "AKK Dictionary Team"
#define MyAppExeName "AkkDictionary.exe"
#define MyAppUrl "https://github.com/aungkokomm/English-Myanmar-Dictionary-"
#define SourcePath "bin\Release\net8.0-windows\win-x64\publish"

[Setup]
AppId={{8F2E3E8C-9E7D-4F3D-8C5B-2E1A4F7C3B9D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppUrl}
AppSupportURL={#MyAppUrl}
AppUpdatesURL={#MyAppUrl}
AppComments=A comprehensive English to Myanmar dictionary powered by .NET 8, WPF, and SQLite
DefaultDirName={autopf}\AkkDictionary
DefaultGroupName={#MyAppName}
AllowNoIcons=no
LicenseFile=LICENSE.txt
OutputDir=bin\Installers
OutputBaseFilename=AKK_En-to-MM_Dictionary_Setup_v{#MyAppVersion}
SetupIconFile=Assets\akk.ico
WizardStyle=modern
WizardSizePercent=100
UsePreviousAppDir=yes
DisableReadyPage=no
UninstallDisplayName={#MyAppName}
VersionInfoVersion={#MyAppVersion}
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a desktop shortcut"; GroupDescription: "Shortcuts:"; Flags: checked
Name: "startmenu"; Description: "Create Start Menu shortcut"; GroupDescription: "Shortcuts:"; Flags: checked
Name: "quicklaunchicon"; Description: "Create Quick Launch shortcut"; GroupDescription: "Shortcuts:"; Flags: unchecked

[Files]
; Main executable
Source: "{#SourcePath}\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
; All runtime files and dependencies
Source: "{#SourcePath}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; Database file
Source: "dictionary.db"; DestDir: "{app}"; Flags: ignoreversion onlyifdoesntexist
; License file
Source: "LICENSE.txt"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
; Start Menu shortcut
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: startmenu
; Desktop shortcut
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: desktopicon
; Quick Launch shortcut
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: quicklaunchicon
; Uninstall shortcut
Name: "{group}\Uninstall {#MyAppName}"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch {#MyAppName}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{app}"

[Registry]
Root: "HKCU"; Subkey: "Software\{#MyAppPublisher}\{#MyAppName}"; Flags: uninsdeletekey

[Code]
function FileExists(const FileName: string): boolean;
begin
  Result := FileExists(ExpandConstant(FileName));
end;
