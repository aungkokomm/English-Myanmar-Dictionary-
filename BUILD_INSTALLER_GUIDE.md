# 📦 BUILD INSTALLER - NEXT STEPS

## 🎯 Current Status

✅ **Application Build**: Complete  
✅ **Inno Setup Script**: Ready (`AkkDictionaryApp.iss`)  
✅ **All Required Files**: Present  
⏳ **Installer Build**: Pending

---

## 🔧 BUILD THE INSTALLER - 2 OPTIONS

### **Option 1: Install Inno Setup (Recommended)**

**Step 1: Download Inno Setup**
1. Go to: https://jrsoftware.org/isdl.php
2. Download latest version (6.3.2 or later)
3. Run the installer
4. Accept defaults (installs to `C:\Program Files (x86)\Inno Setup 6\`)

**Step 2: Build Using GUI**
1. Open Inno Setup IDE
2. File → Open → Select `AkkDictionaryApp.iss`
3. Click **Build** → **Compile** (or press Ctrl+F9)
4. Watch the progress
5. Output: `bin\Installers\AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`

**Step 3: Locate Your Installer**
```
E:\Dictionary\AKK_En_to_MM_Dictionary_v2f_sqlite_import\
  └─ bin\Installers\
     └─ AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe ✅
```

---

### **Option 2: Command Line Build** (After Installing Inno Setup)

**Step 1: Open PowerShell**
```powershell
cd "E:\Dictionary\AKK_En_to_MM_Dictionary_v2f_sqlite_import"
```

**Step 2: Run ISCC.exe**
```powershell
& "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" AkkDictionaryApp.iss
```

Or if Inno Setup 6.3.x:
```powershell
& "C:\Program Files (x86)\Inno Setup 6.3\ISCC.exe" AkkDictionaryApp.iss
```

**Step 3: Wait for Build**
- You should see compilation output
- Look for: `Successful compilation`
- Output file: `bin\Installers\AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`

---

## 📋 INSTALLER BUILD CHECKLIST

Before building, verify:

- [ ] Inno Setup installed from https://jrsoftware.org/isdl.php
- [ ] `AkkDictionaryApp.iss` exists in root
- [ ] `LICENSE.txt` exists in root
- [ ] `dictionary.db` exists in root
- [ ] `Assets\akk.ico` exists
- [ ] `bin\Release\net8.0-windows\win-x64\publish\` exists
- [ ] `bin\Installers\` directory exists

---

## 🧪 AFTER BUILDING - TEST THE INSTALLER

Once you have the `.exe` file:

### **Test 1: Install the Application**
1. Run: `bin\Installers\AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`
2. Click through installer
3. **Important**: Check the box for **"Create a desktop shortcut"** and **"Create Start Menu shortcut"**
4. Click "Install"
5. Let it launch the app automatically

### **Test 2: Verify Installation**
Check that:
- ✅ Desktop shortcut was created
- ✅ Start Menu entry exists
- ✅ Application launches and works
- ✅ Dictionary searches work
- ✅ Files are in: `C:\Program Files\AkkDictionary\`

### **Test 3: Uninstall**
1. Go to: Control Panel → Programs → Programs and Features
2. Find: "AKK En-to-MM Dictionary"
3. Click "Uninstall"
4. Verify all files removed
5. Verify shortcuts deleted

### **Test 4: Clean Reinstall**
1. Build installer again
2. Install in same location
3. Verify no conflicts
4. Verify database preserved

---

## 📤 UPLOAD TO GITHUB

Once installer is built and tested:

### **Step 1: Create GitHub Release**
1. Go to: https://github.com/aungkokomm/English-Myanmar-Dictionary-
2. Click "Releases" (right side)
3. Click "Create a new release"
4. Tag version: `v1.0.0`
5. Release title: `AKK En-to-MM Dictionary v1.0.0`

### **Step 2: Add Release Notes**
```markdown
## AKK En-to-MM Dictionary v1.0.0

### Features
- ✅ English to Myanmar dictionary (10,000+ entries)
- ✅ Myanmar to English reverse search
- ✅ Search suggestions and autocomplete
- ✅ Built with .NET 8, WPF, and SQLite
- ✅ Professional installer with Windows integration
- ✅ Desktop and Start Menu shortcuts
- ✅ About dialog with GitHub link

### Installation
1. Download: `AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`
2. Run the installer
3. Select your preferred shortcuts
4. Launch from Desktop or Start Menu

### System Requirements
- Windows 10 or later
- 64-bit system
- 200 MB disk space
- Admin privileges for installation

### Downloads
- [Installer (.exe)](direct-link-to-exe)

### What's New
- Professional Inno Setup installer
- Customizable desktop shortcut
- About dialog with GitHub repository link
- Full Program Files integration

### Known Issues
None

### Support
- Report issues: [GitHub Issues](https://github.com/aungkokomm/English-Myanmar-Dictionary-/issues)
- View source: [GitHub Repository](https://github.com/aungkokomm/English-Myanmar-Dictionary-)
```

### **Step 3: Upload Installer File**
1. Drag and drop: `AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`
2. Or click "Attach binaries" and browse
3. Wait for upload to complete

### **Step 4: Publish Release**
1. Click "Publish release"
2. Done! 🎉

---

## 📊 FILE SUMMARY

### **What You Have**

| File | Location | Size | Purpose |
|------|----------|------|---------|
| **AkkDictionary.exe** | bin\Release\...\ | 181 MB | App executable (dev build) |
| **AkkDictionaryApp.iss** | Root | 3 KB | Installer script |
| **dictionary.db** | Root | 18.9 MB | Dictionary database |
| **LICENSE.txt** | Root | 1.1 KB | License agreement |
| **akk.ico** | Assets\ | 28 KB | Application icon |

### **What You'll Get After Building**

| File | Location | Size | Purpose |
|------|----------|------|---------|
| **AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe** | bin\Installers\ | ~180-200 MB | **INSTALLER** ✨ |

---

## 🚀 COMPLETE WORKFLOW

```
1. Install Inno Setup
   └─ Download from: https://jrsoftware.org/isdl.php

2. Build Installer
   └─ Open AkkDictionaryApp.iss
   └─ Click Build → Compile
   └─ Output: AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe

3. Test Installer
   ├─ Install the application
   ├─ Verify shortcuts work
   ├─ Test uninstall
   └─ Clean reinstall

4. Upload to GitHub
   ├─ Create new release (v1.0.0)
   ├─ Add release notes
   ├─ Upload .exe file
   └─ Publish release

5. Share with Users
   ├─ Update README with download link
   ├─ Share release link
   └─ Ask for feedback
```

---

## ✅ SUCCESS INDICATORS

You'll know the installer is ready when:

- ✅ Installer file exists: `AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`
- ✅ File size is ~180-200 MB
- ✅ Installation to Program Files works
- ✅ Shortcuts are created correctly
- ✅ Application launches successfully
- ✅ Uninstall removes all files cleanly
- ✅ File uploaded to GitHub Releases

---

## 💡 TIPS

1. **Keep the installer file safe**
   - Back it up
   - Keep the .iss script for future updates

2. **For future updates**
   - Increment version: `#define MyAppVersion "1.0.1"`
   - Rebuild installer
   - Create new GitHub release

3. **If you modify the app**
   - Rebuild application: `dotnet publish -c Release ...`
   - Rebuild installer: `Build → Compile` in Inno Setup
   - Update version number

4. **File size optimization**
   - Current: ~180-200 MB (includes full .NET 8 runtime)
   - This is normal and expected
   - Users with .NET installed would have smaller file

---

## 🎯 NEXT IMMEDIATE STEP

**Download and install Inno Setup**, then come back to build the installer!

🔗 Download: https://jrsoftware.org/isdl.php

---

**Status**: 🟡 Ready for Inno Setup Installation  
**Next Step**: Download Inno Setup 6.3.x or later  
**Estimated Time**: 30 minutes (download + install + build)
