# 📚 AKK En-to-MM Dictionary

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Platform: Windows](https://img.shields.io/badge/Platform-Windows%2010%2B-blue)](https://www.microsoft.com/windows/)
[![Framework: .NET 8](https://img.shields.io/badge/Framework-.NET%208-purple)](https://dotnet.microsoft.com/)
[![Build: Passing](https://img.shields.io/badge/Build-Passing-brightgreen)](https://github.com/aungkokomm/AkkDictionaryApp)

A fast, modern English ↔ Myanmar dictionary application built with .NET 8 and WPF. Search in both directions, import custom dictionaries, and customize your experience.

![Version](https://img.shields.io/badge/Version-1.0.0-blue)
![Status](https://img.shields.io/badge/Status-Production%20Ready-green)

---
<img width="1662" height="804" alt="image" src="https://github.com/user-attachments/assets/0ccb68e3-510f-4ae3-a775-ab24c523545b" />

## ✨ Features

- 🔍 **Fast English→Myanmar Search** - Instantly find word definitions
- 🔄 **Reverse Search** - Search definitions to find Myanmar words (Myanmar→English)
- 💡 **Auto-Suggestions** - Get suggestions as you type
- 📊 **Excel Import** - Import dictionaries from Excel files
- 💾 **SQLite Import** - Import from SQLite databases
- ⚙️ **Customizable UI** - Change fonts and sizes to your preference
- 💾 **Persistent Settings** - Your preferences are saved automatically
- 🔗 **Clickable Links** - Click words in definitions to search them
- 🌐 **Offline Ready** - No internet connection required
- 🎨 **Modern Interface** - Clean, professional WPF design

---

## 🚀 Quick Start

### For Users

**Download and Run (Easiest)**
1. Clone Roposistory 
2. Double-click Build.bat wait till it finishes.
3. After built complete, in bin\Release\net8.0-windows there will be AkkDictionary.exe
3. Run it and searching!

**System Requirements:**
- Windows 10 or later (64-bit)
- 256 MB RAM minimum
- 200+ MB disk space
- No additional software needed

### For Developers

**Clone & Build**
```bash
git clone https://github.com/aungkokomm/AkkDictionaryApp.git
cd AkkDictionaryApp
dotnet build -c Release
```

**Run in Visual Studio 2022**
1. Open `AkkDictionaryApp.sln`
2. Press `F5` to run
3. Start developing!

**Build Distribution Executable**
```powershell
.\Build-Package.ps1
```

---

## 📖 Documentation

Comprehensive guides for different users:

| Document | Purpose | Audience |
|----------|---------|----------|
| [START_HERE.md](START_HERE.md) | Overview & navigation | Everyone |
| [QUICK_START.md](QUICK_START.md) | How to use the app | End users |
| [BUILD_AND_DISTRIBUTION.md](BUILD_AND_DISTRIBUTION.md) | Build & customize | Developers |
| [FILE_GUIDE.md](FILE_GUIDE.md) | File navigation | All |
| [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md) | Code organization | Developers |

---

## 🎯 Use Cases

### 1. **Dictionary Lookup**
Search for English words to find their Myanmar translations, or search Myanmar definitions to find English words.

### 2. **Language Learning**
Use auto-suggestions and clickable links to explore related words and expand vocabulary.

### 3. **Translation Work**
Import custom dictionaries from Excel or SQLite to support your translation projects.

### 4. **Dictionary Management**
Manage multiple dictionaries and customize the interface for your workflow.

---

## 🛠️ Technology Stack

- **Framework**: .NET 8
- **UI**: Windows Presentation Foundation (WPF)
- **Database**: SQLite3
- **Build**: MSBuild / dotnet CLI
- **Distribution**: Self-contained executable

---

## 📦 Installation Methods

### Method 1: Portable Executable (Recommended)
- Download `.exe` file
- No installation required
- Works on any Windows 10+ system
- Can run from USB drive

### Method 2: Windows Installer
Requires: [Inno Setup 6](https://jrsoftware.org/isinfo.php)
```powershell
.\Build-Package.ps1
```
Creates a professional installer with uninstaller support.

### Method 3: Build from Source
```bash
git clone https://github.com/aungkokomm/AkkDictionaryApp.git
dotnet publish -c Release -r win-x64 --self-contained
```

---

## 💻 System Requirements

| Requirement | Minimum | Recommended |
|-------------|---------|-------------|
| OS | Windows 10 x64 | Windows 10/11 x64 |
| RAM | 256 MB | 512 MB+ |
| Disk Space | 200 MB | 300 MB |
| Internet | Not required | Not required |
| Dependencies | None | None |

---

## 🔧 Development

### Prerequisites
- .NET 8 SDK or higher
- Visual Studio 2022 (recommended)
- Git

### Project Structure
```
AkkDictionaryApp/
├── MainWindow.xaml(.cs)       # Main application UI
├── SettingsWindow.xaml(.cs)   # Settings dialog
├── SqliteImportWindow.xaml(.cs) # SQLite import dialog
├── Utils.cs                    # Utility functions
├── App.xaml(.cs)              # Application entry point
├── dictionary.db              # SQLite database
└── Assets/                    # Application icons
```

### Building

**Debug Build**
```bash
dotnet build
```

**Release Build (Optimized)**
```bash
dotnet build -c Release
```

**Self-Contained Executable**
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

### Running Tests
```bash
dotnet test
```

---

## 📥 Importing Dictionaries

### From Excel
1. Prepare Excel file with columns: `Word`, `state` (POS), `def` (definition)
2. Menu → File → Rebuild database from Excel
3. Select your file
4. Wait for import to complete

### From SQLite
1. Menu → File → Import from SQLite
2. Select source database
3. Enter table and column names
4. Click Import

---

## ⚙️ Configuration

### Settings
Access via: Menu → Settings

- **Enable Suggestions** - Auto-complete while typing
- **Default to Reverse Search** - Switch search direction by default
- **UI Font Family** - Customize displayed font
- **Font Scale** - Adjust text size (0.9x to 1.4x)
- **Remember Window** - Save window position

All settings are automatically saved.

---

## 🐛 Troubleshooting

### Application won't start
- Ensure Windows 10 or later
- Try running from different location
- Check folder permissions

### No search results
- Verify `dictionary.db` exists
- Try rebuilding database from Excel
- Check database file isn't corrupted

### Import fails
- Verify file format (Excel or SQLite)
- Check column names match exactly
- Ensure file isn't open in another application

For more help, see [QUICK_START.md](QUICK_START.md#troubleshooting).

---

## 📝 License

This project is licensed under the **MIT License** - see the [LICENSE.txt](LICENSE.txt) file for details.

```
MIT License

Copyright (c) 2024 AKK Dictionary Team

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
...
```

---

## 🤝 Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

Please ensure:
- Code compiles without errors or warnings
- All features are tested
- Documentation is updated

---

## 📞 Support & Contact

- **GitHub Issues**: [Report bugs or request features](https://github.com/aungkokomm/AkkDictionaryApp/issues)
- **Documentation**: See [START_HERE.md](START_HERE.md)
- **User Guide**: See [QUICK_START.md](QUICK_START.md)
- **Developer Guide**: See [BUILD_AND_DISTRIBUTION.md](BUILD_AND_DISTRIBUTION.md)

---

## 🗺️ Roadmap

### Version 1.0 (Current)
- ✅ English↔Myanmar dictionary search
- ✅ Reverse search capability
- ✅ Excel/SQLite import
- ✅ Customizable UI
- ✅ Persistent settings

### Planned Features (v1.1+)
- [ ] Search history
- [ ] Bookmarks/favorites
- [ ] Multi-language support
- [ ] Advanced search filters
- [ ] Dictionary export
- [ ] Cloud sync support

---

## 📊 Project Statistics

| Metric | Value |
|--------|-------|
| Language | C# 12 |
| Framework | .NET 8 |
| UI Technology | WPF |
| Database | SQLite3 |
| Executable Size | ~172 MB |
| Code Size | ~500+ lines |
| Status | Production Ready |

---

## 🎯 Version History

### v1.0.0 (March 2024)
- Initial release
- Full dictionary search capabilities
- Import from Excel and SQLite
- Customizable UI
- Professional WPF interface
- Complete documentation

---

## 🙏 Acknowledgments

- Built with .NET 8 and WPF
- Database powered by SQLite
- Excel support via ExcelDataReader
- Community feedback and contributions

---

## 📄 Additional Resources

- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/)
- [WPF Guide](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
- [SQLite Documentation](https://www.sqlite.org/docs.html)
- [GitHub Guides](https://guides.github.com/)

---

## 💡 Tips for Users

1. **Faster Searching**: Use prefix matching (type first letters)
2. **Explore Links**: Click words in definitions to learn more
3. **Customize Look**: Adjust fonts in Settings for comfortable reading
4. **Import Data**: Keep your dictionary updated with new words
5. **Offline Use**: All data is local - works without internet

---

## ⭐ If You Find This Useful

Please consider:
- ⭐ Starring the repository
- 🐛 Reporting bugs
- 💡 Suggesting features
- 📢 Sharing with others
- 🤝 Contributing improvements

---

**Status**: ✅ Production Ready  
**Last Updated**: March 2026  
**License**: MIT

---

*For quick start, see [QUICK_START.md](QUICK_START.md) or [START_HERE.md](START_HERE.md)*
