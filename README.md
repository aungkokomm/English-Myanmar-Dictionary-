# AKK English–Myanmar Dictionary

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE.txt)
[![Platform](https://img.shields.io/badge/Platform-Windows%2010%2B-blue)](https://github.com/aungkokomm/English-Myanmar-Dictionary-/releases)
[![Android](https://img.shields.io/badge/Platform-Android%208%2B-brightgreen)](https://github.com/aungkokomm/English-Myanmar-Dictionary-/releases)
[![Framework](https://img.shields.io/badge/.NET-10-purple)](https://dotnet.microsoft.com/)

A fast, offline English ↔ Myanmar dictionary for Windows and Android. No internet connection required — the full database is bundled with the app.

---

## Screenshots

<img width="1370" alt="Windows app screenshot" src="https://github.com/user-attachments/assets/bfed872f-9448-49ba-a95e-0ba0f3fbae36" />

<img width="400" alt="Android app screenshot" src="https://github.com/user-attachments/assets/6d7627bc-d39c-4828-b614-6f2d9c0058d4" />

---

## Download

Go to the [**Releases**](https://github.com/aungkokomm/English-Myanmar-Dictionary-/releases) page.

| Platform | File | Requirements |
|----------|------|--------------|
| Windows  | `AkkDictionary-x.x.x-Setup.exe` | Windows 10/11 64-bit |
| Android  | `com.aungkokomm.akkenglishmyanmar.apk` | Android 8.0+ |

---

## Features

### Windows Desktop
| Feature | Details |
|---------|---------|
| 🔍 Search | English→Myanmar and Myanmar→English (reverse) |
| 💡 Auto-suggestions | Prefix suggestions as you type; recent history on focus |
| 🔤 Fuzzy search | "Did you mean" suggestions when no exact match is found (Levenshtein) |
| 🌙 Dark mode | Instant light/dark theme switch; persisted across sessions |
| ↔ Resizable panes | Drag the splitter to resize list vs. detail; width remembered |
| 🔖 Bookmarks | Star entries; view all bookmarks with one click |
| 🔊 Text-to-speech | Read the headword aloud via Windows built-in speech engine |
| ⎘ Copy | Copy headword + POS + all definitions to clipboard |
| 📥 Import | Import dictionaries from Excel (.xlsx) or SQLite (.db) |
| ⚙️ Settings | Font family, font scale, window size persistence |

### Android
| Feature | Details |
|---------|---------|
| 🔍 Search | Full English→Myanmar search with debounce |
| 🃏 Result cards | POS colour badge + first-line definition preview |
| 📄 Detail screen | Full definition list per entry |
| 📴 Offline | Works with no internet connection |

---

## Building from Source

**Prerequisites:** .NET 10 SDK, Visual Studio 2026 (or `dotnet` CLI)

```bash
git clone https://github.com/aungkokomm/English-Myanmar-Dictionary-.git
cd English-Myanmar-Dictionary-

# Run (debug)
dotnet run --project AkkDictionaryApp.csproj

# Release build
dotnet build AkkDictionaryApp.csproj -c Release

# Self-contained single-file EXE
dotnet publish AkkDictionaryApp.csproj -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o publish/win-x64
```

**Build the installer** (requires [Inno Setup 6](https://jrsoftware.org/isinfo.php)):
```bash
# After publishing above
"C:\Program Files (x86)\Inno Setup 6\iscc.exe" installer/AkkDictionary.iss
```
Output: `installer/output/AkkDictionary-x.x.x-Setup.exe`

---

## Importing a Custom Dictionary

**From Excel**
1. Prepare a `.xlsx` file with columns: `Word`, `state` (part of speech), `def` (definition)
2. Menu → File → **Rebuild database from Excel…**

**From SQLite**
1. Menu → File → **Import from SQLite…**
2. Select your `.db` file and map the column names

---

## Project Structure

```
├── MainWindow.xaml(.cs)          # Main window — search, list, detail pane
├── SettingsWindow.xaml(.cs)      # Settings dialog
├── SqliteImportWindow.xaml(.cs)  # SQLite import dialog
├── AboutWindow.xaml(.cs)         # About dialog
├── Utils.cs                      # DB helpers, search, fuzzy, settings
├── App.xaml(.cs)                 # Application entry; loads theme
├── Themes/
│   ├── LightTheme.xaml
│   └── DarkTheme.xaml
├── Assets/
│   └── akk.ico
├── installer/
│   └── AkkDictionary.iss         # Inno Setup script
├── AkkDictionary.Android/        # Android (.NET for Android) project
└── dictionary.db                 # Bundled SQLite dictionary database
```

---

## Contributing

1. Fork the repo
2. Create a feature branch: `git checkout -b feature/my-feature`
3. Commit your changes
4. Open a Pull Request

Please make sure the project builds without errors before submitting.

---

## License

[MIT License](LICENSE.txt) — © Aung Ko Ko
