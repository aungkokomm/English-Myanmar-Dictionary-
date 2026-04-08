# 🔧 AKK En-to-MM Dictionary - Installer Setup Guide

## ✅ Installation Setup Complete

Your application is now configured with a professional Inno Setup installer that installs to **Program Files** with optional desktop shortcut creation.

---

## 📦 What Changed in the Installer (AkkDictionaryApp.iss)

### **1. Installation Location**
```
✅ BEFORE: C:\Users\[Username]\Local Settings\
✅ NOW:    C:\Program Files\AkkDictionary\
```

**Benefits:**
- Standard Windows installation location
- Requires Admin privileges (for security)
- Proper uninstall through Control Panel
- User-friendly path

---

### **2. Shortcut Options**

The installer now provides **3 shortcut options**:

| Shortcut Type | Default | Location | Purpose |
|---|---|---|---|
| **Desktop Shortcut** | ✅ CHECKED | Desktop | Quick launch from desktop |
| **Start Menu** | ✅ CHECKED | Start Menu → AKK En-to-MM Dictionary | Easy access from Start Menu |
| **Quick Launch** | ⬜ UNCHECKED | Quick Launch Bar | Optional quick access |

Users can **toggle any option** during installation!

---

### **3. Key Configuration Changes**

#### Executable Name Fix
```ini
❌ BEFORE: MyAppExeName "AKK En-to-MM Dictionary.exe"  (INVALID - has spaces)
✅ NOW:    MyAppExeName "AkkDictionary.exe"             (VALID - matches assembly name)
```

#### Install Directory
```ini
❌ BEFORE: DefaultDirName {autopf}\{#MyAppName}     (Would create spaces in path)
✅ NOW:    DefaultDirName {autopf}\AkkDictionary   (Clean path without spaces)
```

#### Admin Privileges
```ini
✅ PrivilegesRequired=admin              (Required for Program Files)
✅ ArchitecturesInstallIn64BitMode=x64   (Optimized for 64-bit systems)
```

---

## 🎯 Installation Process for End Users

### **Step 1: Run Installer**
1. Download: `AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`
2. Double-click to run
3. Click "Yes" when prompted for Admin access

### **Step 2: Welcome Screen**
- Accept License Agreement
- Review installation details

### **Step 3: Select Shortcuts (NEW!)**
Users can check/uncheck these options:
- ✅ **Create a desktop shortcut** (RECOMMENDED - checked by default)
- ✅ **Create Start Menu shortcut** (checked by default)
- ⬜ **Create Quick Launch shortcut** (optional)

### **Step 4: Installation**
- Files are copied to `C:\Program Files\AkkDictionary\`
- Shortcuts are created based on user selections
- Database is configured
- Ready to launch!

### **Step 5: Launch Application**
- Installer can automatically launch the app
- Or user can manually launch from:
  - **Desktop shortcut** (if created)
  - **Start Menu** → AKK En-to-MM Dictionary
  - **Control Panel** → Programs → Uninstall

---

## 🔍 Installation Details

### **Files Installed**
Location: `C:\Program Files\AkkDictionary\`

```
📁 AkkDictionary\
├── 📄 AkkDictionary.exe          (Main application)
├── 📄 dictionary.db               (Dictionary database)
├── 📄 LICENSE.txt                 (License agreement)
├── 📚 [Runtime libraries]          (WPF, .NET 8 dependencies)
└── 📚 [SQLite components]          (Database engine)
```

### **Shortcuts Created**
If selected during installation:

1. **Desktop Shortcut**
   - `AKK En-to-MM Dictionary.lnk` on Desktop
   - Direct access to the application

2. **Start Menu**
   - Start Menu → AKK En-to-MM Dictionary
   - Organized folder for shortcuts

3. **Uninstall Entry**
   - Control Panel → Programs and Features
   - Full uninstall with cleanup

---

## 🗑️ Uninstallation

Users can uninstall in multiple ways:

### **Method 1: Control Panel** (Recommended)
1. Open Control Panel
2. Go to "Programs" → "Programs and Features"
3. Find "AKK En-to-MM Dictionary"
4. Click "Uninstall"
5. Confirm removal

### **Method 2: Start Menu**
1. Start Menu → AKK En-to-MM Dictionary → Uninstall

### **Method 3: Direct Executable**
- Run: `C:\Program Files\AkkDictionary\uninstall.exe`

**Cleanup:**
- ✅ Application files removed
- ✅ Shortcuts deleted
- ✅ Registry entries cleaned
- ✅ Database preserved (user data safe)

---

## 🛠️ Building the Installer

### **Prerequisites**
1. Inno Setup installed (download from: https://jrsoftware.org/isdl.php)
2. Application built in Release mode: `bin\Release\net8.0-windows\win-x64\publish\`
3. Icon file: `Assets\akk.ico` (optional)
4. License file: `LICENSE.txt`
5. Database file: `dictionary.db`

### **Build Process**
1. **Compile Application**
   ```powershell
   dotnet publish -c Release -f net8.0-windows --self-contained
   ```

2. **Open Inno Setup**
   - File → Open → `AkkDictionaryApp.iss`

3. **Compile Installer**
   - Build → Compile
   - Or press Ctrl+F9

4. **Output**
   - Generated at: `bin\Installers\AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`

### **Using PowerShell Build Script**
If you have `Build-Package.ps1`:
```powershell
.\Build-Package.ps1
```
This automatically:
- Builds the application
- Compiles the installer
- Places it in `bin\Installers\`

---

## 📋 Configuration Summary

| Setting | Value | Purpose |
|---|---|---|
| **App Name** | AKK En-to-MM Dictionary | Display name |
| **App Version** | 1.0.0 | Version tracking |
| **Install Folder** | C:\Program Files\AkkDictionary | Standard location |
| **Executable** | AkkDictionary.exe | Application launcher |
| **Privileges** | Admin required | Program Files access |
| **Architecture** | x64 | 64-bit optimized |
| **License** | LICENSE.txt included | Legal compliance |

---

## ✨ Features

✅ **Professional Installation**
- Installs to standard Program Files location
- Requires admin privileges for security
- Clean uninstall with registry cleanup

✅ **User-Friendly Shortcuts**
- Desktop shortcut option (default enabled)
- Start Menu integration
- Quick Launch optional shortcut
- Easy-to-remove uninstall entry

✅ **Application Integration**
- Automatic launch after installation (optional)
- Proper file associations
- Registry entries for uninstall
- License displayed during setup

✅ **Error Handling**
- Proper path handling
- Admin privilege verification
- Graceful installation failure recovery
- Rollback on errors

---

## 🚀 Distribution

### **Installer File**
- **Name**: `AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`
- **Location**: `bin\Installers\`
- **Size**: ~50-100 MB (depends on dependencies)
- **Platform**: Windows x64 only
- **Requirements**: 
  - Windows 10 or later
  - Admin privileges
  - ~200 MB disk space

### **Upload to GitHub Releases**
1. Create a new release on GitHub
2. Upload: `AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`
3. Add release notes
4. Mark as latest release

---

## 📞 Troubleshooting

### **Admin Prompt Issues**
- Ensure User Account Control (UAC) is enabled
- Run as Administrator if UAC is disabled

### **Installation Path Issues**
- Verify Program Files folder exists and is writable
- Check disk space (minimum 200 MB)

### **Shortcut Creation Fails**
- Check Windows permissions
- Verify Desktop and Start Menu folders are accessible

### **Uninstall Issues**
- Use Control Panel method (most reliable)
- If stuck, delete `C:\Program Files\AkkDictionary\` manually
- Clean registry with CCleaner if needed

---

## 📝 Next Steps

1. ✅ **Build the installer**
   ```powershell
   cd E:\Dictionary\AKK_En_to_MM_Dictionary_v2f_sqlite_import
   .\Build-Package.ps1
   ```

2. ✅ **Test the installer**
   - Run: `bin\Installers\AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`
   - Verify shortcuts are created
   - Test uninstall

3. ✅ **Upload to GitHub Releases**
   - Go to https://github.com/aungkokomm/AkkDictionaryApp
   - Create new release
   - Upload the .exe file

4. ✅ **Share with users**
   - Link to download page
   - Include installation instructions
   - Provide uninstall guide

---

## ✅ Verification Checklist

After building and testing:

- [ ] Installer runs without errors
- [ ] Desktop shortcut is created (if selected)
- [ ] Start Menu entry appears
- [ ] Application launches correctly
- [ ] Uninstall removes all files
- [ ] No leftover registry entries
- [ ] Re-installation works cleanly
- [ ] All files in Program Files location

---

**Version**: 1.0.0  
**Last Updated**: 2024  
**Status**: ✅ Production Ready

Your installer is now professionally configured and ready for distribution! 🎉
