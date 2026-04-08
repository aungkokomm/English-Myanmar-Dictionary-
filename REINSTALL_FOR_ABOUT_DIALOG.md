# 🔧 FIX ABOUT DIALOG - UNINSTALL & REINSTALL GUIDE

## ❌ Problem Identified

The installed version is showing the **old About dialog** because:

1. ✅ The code is correct in MainWindow.xaml.cs (line 196-200)
2. ✅ AboutWindow.xaml and AboutWindow.xaml.cs are correct
3. ✅ The new installer was built successfully (127.77 MB)
4. ❌ **BUT**: You're still running the OLD installed version from `C:\Program Files\AkkDictionary\`

The old executable was installed before we made the About dialog changes, so it still has the old MessageBox code.

---

## ✅ SOLUTION - 3 SIMPLE STEPS

### **Step 1: Uninstall the Old Version** (2 minutes)

**Option A: Via Control Panel (Recommended)**
1. Open: **Control Panel** → **Programs** → **Programs and Features**
2. Find: **"AKK En-to-MM Dictionary"**
3. Click: **"Uninstall"**
4. Confirm removal
5. Wait for completion

**Option B: Direct Uninstall**
1. Go to: `C:\Program Files\AkkDictionary\`
2. Run: `uninstall.exe`
3. Confirm removal

**Option C: Manual Cleanup**
1. Delete folder: `C:\Program Files\AkkDictionary\`
2. Delete Desktop shortcut
3. Delete Start Menu folder

---

### **Step 2: Install the New Version** (3 minutes)

**New Installer Location:**
```
E:\Dictionary\AKK_En_to_MM_Dictionary_v2f_sqlite_import\
  └─ bin\Installers\
     └─ AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe
```

**Installation Steps:**
1. **Run the installer:**
   ```
   Double-click: AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe
   ```

2. **Click through wizard:**
   - Accept License
   - Choose installation folder (default is fine)
   - **CHECK BOTH:**
     - ✅ Create a desktop shortcut
     - ✅ Create Start Menu shortcut
   - Click "Install"

3. **Launch the app:**
   - Let it auto-launch after install, OR
   - Click Desktop shortcut, OR
   - Use Start Menu

---

### **Step 3: Verify the About Dialog** (1 minute)

**Test the New About Dialog:**

1. **Open the application**
2. **Click Menu:** Help → About
3. **Verify you see:**
   - ✅ "AKK En-to-MM Dictionary" title
   - ✅ "A comprehensive English-Myanmar dictionary"
   - ✅ "Built with .NET 8 + WPF + SQLite"
   - ✅ **"GitHub: View on GitHub"** (clickable link)
   - ✅ Close button

4. **Test the GitHub link:**
   - Click on "View on GitHub"
   - Should open in your default browser
   - Should go to: https://github.com/aungkokomm/English-Myanmar-Dictionary-

---

## 📊 Expected Results

### **OLD (Current - Wrong)**
```
About

AKK En-to-MM Dictionary

Built with .NET 8 + WPF + SQLite.

         [OK]
```

### **NEW (After Reinstall - Correct)**
```
AKK En-to-MM Dictionary

A comprehensive English-Myanmar dictionary

Built with .NET 8 + WPF + SQLite

GitHub: View on GitHub

         [Close]
```

---

## 🎯 Quick Command to Verify

**Check what's installed:**
```powershell
Get-Item "C:\Program Files\AkkDictionary\*" | Select-Object Name, LastWriteTime
```

**Should show recent dates after reinstall!**

---

## ⏱️ Total Time Required

- Uninstall: 2 minutes
- Install: 3 minutes
- Test: 1 minute
- **TOTAL: ~6 minutes**

---

## ✅ Checklist

- [ ] Uninstalled old version from Program Files
- [ ] Verified uninstall (shortcuts gone, folder empty)
- [ ] Run new installer (127.77 MB)
- [ ] Installed successfully
- [ ] Launched application
- [ ] Clicked Help → About
- [ ] See new About dialog with GitHub link
- [ ] Clicked "View on GitHub"
- [ ] Browser opened to GitHub repo
- [ ] Clicked Close button
- [ ] App continues working normally

---

## 🆘 If Issues Persist

### **About dialog still shows old version?**
```
Solution: Check Task Manager
1. Open Task Manager (Ctrl+Shift+Esc)
2. Look for: AkkDictionary.exe
3. If found, right-click → End Task
4. Try again
```

### **Uninstall fails?**
```
Solution: Manual cleanup
1. Delete: C:\Program Files\AkkDictionary\
2. Delete: Desktop shortcut
3. Delete: Start Menu → AKK En-to-MM Dictionary
```

### **Installer won't run?**
```
Solution: Run as Administrator
1. Right-click installer
2. Select "Run as administrator"
3. Click "Yes" on UAC prompt
```

### **GitHub link won't open?**
```
Solution: Check browser
1. Verify you have a default browser set
2. Try manually opening: 
   https://github.com/aungkokomm/English-Myanmar-Dictionary-
```

---

## 🔍 Technical Details

**Why the old About dialog showed:**
- Old executable: `AkkDictionary.exe` (had MessageBox code)
- Old code: `MessageBox.Show("AKK En-to-MM Dictionary\n\nBuilt with .NET 8 + WPF + SQLite.", "About")`

**Why the new one will show:**
- New executable: `AkkDictionary.exe` (has AboutWindow code)
- New code: `AboutWindow { Owner = this }.ShowDialog()`

**What changed in code:**
1. Created: `AboutWindow.xaml` (UI with GitHub link)
2. Created: `AboutWindow.xaml.cs` (Click handler)
3. Updated: `MainWindow.xaml.cs` line 196-200 (opens AboutWindow)

---

## 📦 Installer Details

**File:** `AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`  
**Size:** 127.77 MB  
**Built:** April 8, 2026, 18:38:33  
**Includes:**
- ✅ Latest AboutWindow
- ✅ Latest MainWindow changes
- ✅ All .NET 8 runtime
- ✅ SQLite database
- ✅ Complete dependencies

---

## ✨ After Reinstalling

Your application will have:
- ✅ Beautiful About dialog
- ✅ Professional layout
- ✅ Clickable GitHub link
- ✅ Opens in browser
- ✅ Close button
- ✅ All features working

---

**NEXT ACTION:** Follow the 3-step uninstall & reinstall process above!

This will give you the **correct About dialog with the GitHub link**. 🎉
