#define MyAppName      "AKK English-Myanmar Dictionary"
#define MyAppVersion   "1.2.0"
#define MyAppPublisher "Aung Ko Ko"
#define MyAppExeName   "AkkDictionary.exe"
#define MyAppId        "{A1B2C3D4-E5F6-7890-ABCD-EF1234567890}"

[Setup]
AppId={{#MyAppId}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\AkkDictionary
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=..\installer\output
OutputBaseFilename=AkkDictionary-{#MyAppVersion}-Setup
SetupIconFile=..\Assets\akk.ico
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
MinVersion=10.0
UninstallDisplayIcon={app}\{#MyAppExeName}
UninstallDisplayName={#MyAppName}
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany={#MyAppPublisher}
VersionInfoDescription={#MyAppName} Installer
LicenseFile=

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop shortcut"; GroupDescription: "Additional icons:"; Flags: unchecked

[Files]
; Main executable (self-contained — no .NET runtime required)
Source: "..\publish\win-x64\AkkDictionary.exe"; DestDir: "{app}"; Flags: ignoreversion
; Dictionary database
Source: "..\publish\win-x64\dictionary.db";     DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}";          Filename: "{app}\{#MyAppExeName}"
Name: "{group}\Uninstall {#MyAppName}";Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}";    Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch {#MyAppName}"; Flags: nowait postinstall skipifsilent
