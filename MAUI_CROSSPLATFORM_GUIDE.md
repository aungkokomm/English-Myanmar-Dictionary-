# 🚀 MAUI CROSS-PLATFORM PROJECT - SETUP GUIDE

## 📋 PROJECT PLAN

**Goal:** Create Mac App & Android APK from existing Windows app  
**Framework:** .NET MAUI  
**Language:** C#  
**Database:** SQLite (shared)  
**Build Time:** 3-6 weeks total  

---

## 🎯 PHASE 1: MAC APP (Week 1-2)

### Step 1: Create MAUI Project
```bash
dotnet new maui -n AkkDictionary.MAUI
```

### Step 2: Migrate Shared Code
```
✓ Database layer (SQLiteConnection code)
✓ Search algorithms
✓ Utils.cs functions
✓ Settings system
✓ SettingsWindow logic → Settings Page
✓ Dictionary database
```

### Step 3: Build Mac UI
```
✓ Mobile-optimized layouts
✓ Touch-friendly buttons
✓ Responsive design
✓ Search interface
✓ Dictionary display
✓ Settings screen
✓ About dialog (with GitHub link!)
```

### Step 4: Mac Configuration
```
✓ Configure for macOS target
✓ Create app icon
✓ Set up entitlements
✓ Build DMG installer
```

### Step 5: Test & Deploy
```
✓ Test on Mac
✓ Create DMG package
✓ Upload to Mac App Store (optional)
```

---

## 🎯 PHASE 2: ANDROID APP (Week 3-4)

### Step 1: Configure Android Target
```
✓ Add Android build configuration
✓ Set up Android SDK
✓ Configure permissions
```

### Step 2: Android UI Optimization
```
✓ Adapt for touch screens
✓ Mobile layout adjustments
✓ Android-specific navigation
```

### Step 3: Build APK
```
✓ Create signing certificate
✓ Build release APK
✓ Test on Android emulator/device
```

### Step 4: Distribution
```
✓ Release APK for direct download
✓ Optional: Google Play Store submission
```

---

## 📁 NEW PROJECT STRUCTURE

```
E:\Dictionary\
├── AKK_En_to_MM_Dictionary_v2f_sqlite_import\  (Windows WPF - Keep)
│   ├── bin\
│   ├── AkkDictionaryApp.csproj
│   └── ...
│
└── AkkDictionary.MAUI\  (NEW - Cross-platform)
    ├── MauiProgram.cs
    ├── App.xaml
    ├── AppShell.xaml
    ├── Resources/
    ├── Views/
    │   ├── MainPage.xaml
    │   ├── SettingsPage.xaml
    │   ├── AboutPage.xaml
    │   └── ...
    ├── ViewModels/
    │   ├── MainViewModel.cs
    │   ├── SettingsViewModel.cs
    │   └── ...
    ├── Services/
    │   ├── DatabaseService.cs
    │   ├── SettingsService.cs
    │   └── ...
    ├── Models/
    │   └── (shared from WPF)
    ├── Platforms/
    │   ├── MacCatalyst/
    │   ├── Android/
    │   ├── iOS/
    │   └── Windows/
    └── AkkDictionary.MAUI.csproj
```

---

## 🔄 CODE REUSE

### **KEEP (100% Reusable)**
```
✅ Database queries (SQL)
✅ Search algorithms
✅ Dictionary entries data
✅ Business logic
✅ Utility functions
✅ Settings structure
```

### **ADAPT (50% Reusable)**
```
⚠️ UI layouts (WPF → MAUI)
⚠️ Event handlers → ViewModel Commands
⚠️ Settings UI → Settings Page
⚠️ About dialog → About Page
```

### **RECREATE (New)**
```
⭕ MAUI shell navigation
⭕ Mobile-optimized layouts
⭕ Touch gestures
⭕ Platform-specific code
```

---

## 📊 COMPARISON: WPF vs MAUI

```
WINDOWS (WPF):
├── MainWindow.xaml (Desktop UI)
├── AboutWindow.xaml (Dialog)
└── SettingsWindow.xaml (Window)

MAUI (Mac/Android):
├── AppShell.xaml (Navigation)
├── Views/
│   ├── MainPage.xaml (Search screen)
│   ├── SettingsPage.xaml (Settings)
│   └── AboutPage.xaml (About info)
└── ViewModels/
    ├── MainViewModel.cs
    ├── SettingsViewModel.cs
    └── AboutViewModel.cs
```

---

## 🛠️ PREREQUISITES

Install:
- ✅ .NET 8 SDK (already have)
- ✅ Visual Studio 2026 (already have)
- ✅ MAUI workload: `dotnet workload install maui`
- 🔶 For Mac: Xcode Command Line Tools
- 🔶 For Android: Android SDK (optional, can use emulator)

---

## 📱 MAUI FEATURES YOU'LL USE

```csharp
// XAML for cross-platform UI
<StackLayout>
    <Entry Placeholder="Search..." />
    <Button Text="Search" />
    <CollectionView ItemsSource="{Binding Results}" />
</StackLayout>

// C# ViewModels (MVVM pattern)
public class MainViewModel : INotifyPropertyChanged
{
    // Shared business logic
    // Property binding
    // Commands (click handlers)
}

// Platform-specific code
#if __MACCATALYST__
    // Mac-specific code
#elif __ANDROID__
    // Android-specific code
#endif
```

---

## 🎯 STEP-BY-STEP EXECUTION

### **Week 1: Setup & Mac Preparation**
- [ ] Install MAUI workload
- [ ] Create MAUI project
- [ ] Migrate shared code
- [ ] Create basic UI structure
- [ ] Set up database layer
- [ ] Implement search functionality

### **Week 2: Mac App Finalization**
- [ ] Build Mac-optimized UI
- [ ] Implement Settings page
- [ ] Create About page with GitHub link
- [ ] Test on Mac
- [ ] Build DMG installer
- [ ] Release v1.0 for macOS

### **Week 3: Android Setup**
- [ ] Configure Android target
- [ ] Optimize UI for mobile
- [ ] Test on Android emulator
- [ ] Create release build

### **Week 4: Android Release**
- [ ] Create signing certificate
- [ ] Build APK
- [ ] Test on real device
- [ ] Release APK download

---

## 📦 FINAL DELIVERABLES

**After Completion:**

1. **Windows** (Existing)
   - `AKK_En-to-MM_Dictionary_Setup_v1.0.0.exe`
   - Installed via installer
   - GitHub: Releases

2. **Mac** (NEW)
   - `AkkDictionary-1.0.0.dmg`
   - Download and drag to Applications
   - Mac App Store (optional)

3. **Android** (NEW)
   - `AkkDictionary-1.0.0.apk`
   - Direct download
   - Google Play Store (optional)

4. **iOS** (BONUS - free with MAUI)
   - `AkkDictionary-1.0.0.ipa`
   - App Store (optional)

---

## ✅ VERSION TRACKING

```
Windows:  AKK En-to-MM Dictionary v1.0.0 ✅ (Released)
macOS:    AKK En-to-MM Dictionary v1.0.0 📱 (Coming soon)
Android:  AKK En-to-MM Dictionary v1.0.0 📱 (Coming next)
iOS:      AKK En-to-MM Dictionary v1.0.0 📱 (Bonus)

All with shared database and features!
```

---

## 🚀 LET'S BEGIN!

Ready to start with **Mac app setup**?

I'll:
1. Create MAUI project structure
2. Set up the project file
3. Migrate your shared code
4. Create basic Mac UI
5. Build configuration

**Confirm:** Ready to proceed? 🎯
