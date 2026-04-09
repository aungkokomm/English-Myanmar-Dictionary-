# 📦 How to Upload Executable to GitHub Releases - Quick Reference

## ⚡ TL;DR (Quick Steps)

### Via Web Interface (Easiest)
```
1. Go to: https://github.com/aungkokomm/AkkDictionaryApp
2. Click: Releases → Create a new release
3. Tag: v1.0.0
4. Title: AKK Dictionary v1.0.0 - Initial Release
5. Upload: bin\Installers\AKK En-to-MM Dictionary.exe
6. Click: Publish release ✅
```

### Via GitHub CLI (Fastest)
```powershell
gh auth login
.\Upload-Release.ps1
```

---

## 📸 Step-by-Step Visual Guide

### Step 1: Navigate to Releases
```
GitHub Repo Page
https://github.com/aungkokomm/AkkDictionaryApp
        ↓
    Look for "Releases" tab (right side)
        ↓
    Click "Create a new release"
```

### Step 2: Fill in Release Information
```
Tag version:           v1.0.0
Release title:         AKK Dictionary v1.0.0 - Initial Release
Release description:   [See template below]
Pre-release checkbox:  ☐ (uncheck for stable release)
```

### Step 3: Add Release Description
Copy and paste:
```
# AKK En-to-MM Dictionary v1.0.0

🎉 **Initial Release!**

## Features
- English↔Myanmar dictionary search
- Reverse search (Myanmar→English)
- Real-time auto-suggestions
- Excel & SQLite import
- Customizable UI
- Offline functionality

## Requirements
- Windows 10+ (64-bit)
- 256 MB RAM
- 200+ MB disk space

## Quick Start
1. Download .exe below
2. Double-click to run
3. Start searching!

## Documentation
- [README](../../blob/master/README.md)
- [Quick Start Guide](../../blob/master/QUICK_START.md)
- [Developer Guide](../../blob/master/BUILD_AND_DISTRIBUTION.md)

## License
MIT License
```

### Step 4: Upload File
```
"Attach binaries by dropping them here..."
        ↓
    Click or drag-and-drop:
    bin\Installers\AKK En-to-MM Dictionary.exe
        ↓
    Wait for upload (172 MB ≈ 5-10 minutes)
        ↓
    See "✓ Uploaded: AKK En-to-MM Dictionary.exe"
```

### Step 5: Publish Release
```
Click "Publish release" button
        ↓
    Release page created
        ↓
    Users can download! ✅
```

---

## 🔗 Final URLs

After upload, these URLs will be active:

**Release Page:**
```
https://github.com/aungkokomm/AkkDictionaryApp/releases/tag/v1.0.0
```

**Direct Download Link:**
```
https://github.com/aungkokomm/AkkDictionaryApp/releases/download/v1.0.0/AKK%20En-to-MM%20Dictionary.exe
```

**Releases Page (All releases):**
```
https://github.com/aungkokomm/AkkDictionaryApp/releases
```

---

## ✅ Verification Checklist

After publishing:
- [ ] Release page shows "v1.0.0"
- [ ] Release title displays correctly
- [ ] Description is formatted nicely
- [ ] Executable appears in "Assets" section
- [ ] File size shows ~172 MB
- [ ] Download button works
- [ ] "Latest Release" badge appears (if you set it)

---

## 🐛 Troubleshooting

### Upload Hangs or Times Out
- **Solution**: Use GitHub CLI (`.\Upload-Release.ps1`) instead
- GitHub CLI handles large files better

### "Release already exists"
- **Solution**: Delete the empty draft release first
- Go to Releases → Edit → Delete draft

### File Doesn't Upload
- **Solution**: Check internet connection
- Try smaller chunks (compress if possible)
- Use GitHub CLI with `gh release upload`

### Wrong File Uploaded
- **Solution**: Edit release → Delete file → Re-upload
- Or delete release and create new one

---

## 📈 For Future Versions

To release v1.1.0:

1. Update version in `AkkDictionaryApp.csproj`:
   ```xml
   <Version>1.1.0.0</Version>
   ```

2. Rebuild: `.\Build-Package.ps1`

3. Create new release:
   - Tag: `v1.1.0`
   - Title: `AKK Dictionary v1.1.0`
   - Upload: New executable
   - Describe changes in release notes

---

## 🎁 What Users See

After you publish:

```
📦 Releases
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Latest
  ⭐ AKK Dictionary v1.0.0 - Initial Release
     🎉 Initial release...
     
     ▼ Assets
     └─ AKK En-to-MM Dictionary.exe (172 MB)
        ↓ Download button
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

## 💡 Pro Tips

1. **Automatic Version**: Add version tag to releases
2. **Release Notes**: Write clear, helpful release notes
3. **Multiple Assets**: Can upload multiple files per release
4. **Pre-release**: Mark as pre-release if not fully stable
5. **Changelog**: Link to changelog in release notes
6. **Assets**: GitHub auto-generates checksums for download
7. **Search**: Users can search for releases on GitHub
8. **Notifications**: Users who watch repo get release notifications

---

## 🚀 Now Go Upload!

**Choose Your Method:**

### 🌐 Method A: Web Interface
```
1. Visit: https://github.com/aungkokomm/AkkDictionaryApp
2. Click: Releases → Create new release
3. Follow steps above
4. Click: Publish release
```

### 💻 Method B: GitHub CLI
```powershell
.\Upload-Release.ps1
```

**Done!** ✅

Users can now download your executable from the releases page!

---

## 📞 Need Help?

See these files:
- `UPLOAD_TO_RELEASES.md` - Detailed guide
- `Upload-Release.ps1` - Automated script
- `QUICK_START.md` - User guide
- `README.md` - Project overview

---

**Repository**: https://github.com/aungkokomm/AkkDictionaryApp  
**Status**: Ready to receive v1.0.0 release ✅
