# AKK En-to-MM Dictionary - Quick Start Guide

## Installation

### Portable Executable
1. Download `AKK En-to-MM Dictionary.exe`
2. Place it anywhere on your computer (Desktop, Documents, USB drive, etc.)
3. Double-click to run - no installation required!

## First Use

### 1. Basic Search
- Type any English word in the search box
- Press Enter or wait a moment for suggestions
- Click on a result to see all definitions

### 2. Reverse Search (Myanmar→English)
- Click the checkbox "Search in definitions (MM→EN)"
- Or use Menu > View > Search in definitions
- Type Myanmar text to search definitions

### 3. Import Dictionary Data

#### From Excel File:
- Menu > File > Rebuild database from Excel...
- Select your Excel file (must have columns: Word, state, def)
- Wait for import to complete

#### From SQLite Database:
- Menu > File > Import from SQLite...
- Browse and select the source database file
- Enter table and column names
- Click OK to import

## Settings

To customize the application:
- Click Menu > Settings
- Adjust:
  - Enable/disable suggestions
  - Default search mode (forward/reverse)
  - UI font family
  - Font size scale
  - Remember window position

## Features

✓ Fast English→Myanmar lookups  
✓ Reverse search (Myanmar→English)  
✓ Auto-suggestions while typing  
✓ Import from Excel or SQLite  
✓ Click on links within definitions to search  
✓ Customizable font and interface  
✓ No internet connection required  

## System Requirements

- **OS**: Windows 10 or later (any edition)
- **Memory**: 256 MB RAM minimum
- **Disk Space**: 200+ MB for portable version
- **No additional software needed** - includes .NET 8 runtime

## Troubleshooting

### Application won't start
- Ensure you're on Windows 10 or later
- Try running from a different location
- Check that the folder has write permissions

### No results showing
- Ensure `dictionary.db` is in the application folder
- Try rebuilding the database from Excel
- Check that the database file is not corrupted

### Import fails
- Verify the Excel/SQLite file format is correct
- Ensure column names match exactly
- Try opening the file in another application first

### Performance issues
- Close other applications to free up memory
- Rebuild the database
- Restart the application

## Getting Help

If you encounter issues:
1. Check the troubleshooting section above
2. Review the BUILD_AND_DISTRIBUTION.md file
3. Contact the development team

## About

**AKK En-to-MM Dictionary**  
Version: 1.0.0  
Built with: .NET 8 + WPF + SQLite

---

Enjoy using the AKK Dictionary! 📚
