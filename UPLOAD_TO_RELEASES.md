# How to Upload Executable to GitHub Releases

## 📦 Executable Information
- **File**: `AKK En-to-MM Dictionary.exe`
- **Location**: `bin\Installers\AKK En-to-MM Dictionary.exe`
- **Size**: 172.8 MB
- **Version**: 1.0.0

---

## 🚀 Method 1: Upload via GitHub Web Interface (Easiest - Recommended)

### Step 1: Go to Releases Page
1. Visit: https://github.com/aungkokomm/AkkDictionaryApp
2. Click **"Releases"** tab on the right side
3. Click **"Create a new release"** button

### Step 2: Create Release
1. **Tag version**: `v1.0.0`
2. **Release title**: `AKK Dictionary v1.0.0 - Initial Release`
3. **Description**: Copy this:

```
# AKK En-to-MM Dictionary v1.0.0

🎉 Initial release of the AKK Dictionary application!

## What's Included
- Fast English↔Myanmar dictionary search
- Reverse search capability (Myanmar→English)
- Real-time auto-suggestions
- Import from Excel and SQLite
- Customizable UI (fonts, sizes)
- Persistent settings
- Modern WPF interface
- Offline functionality

## System Requirements
- Windows 10 or later (64-bit)
- 256 MB RAM minimum
- 200+ MB disk space
- No additional software needed

## Download
See below for the executable file.

## Quick Start
1. Download the `.exe` file
2. Double-click to run
3. Start searching!

## Documentation
- [README](README.md) - Project overview
- [QUICK_START.md](QUICK_START.md) - User guide
- [BUILD_AND_DISTRIBUTION.md](BUILD_AND_DISTRIBUTION.md) - Developer guide

## License
MIT License - See LICENSE.txt for details
```

### Step 3: Upload Executable
1. Scroll down to **"Attach binaries by dropping them here or selecting them"**
2. Click the upload area
3. Select: `bin\Installers\AKK En-to-MM Dictionary.exe`
4. Wait for upload to complete (this may take a few minutes due to 172 MB file size)
5. Click **"Publish release"**

---

## 🖥️ Method 2: Upload via GitHub CLI (Faster for developers)

### Install GitHub CLI
```bash
# Windows (via winget)
winget install GitHub.cli

# Or download from: https://github.com/cli/cli/releases
```

### Authenticate
```bash
gh auth login
# Follow the prompts to authenticate
```

### Create Release and Upload
```bash
cd E:\Dictionary\AKK_En_to_MM_Dictionary_v2f_sqlite_import

# Create release
gh release create v1.0.0 `
  --title "AKK Dictionary v1.0.0 - Initial Release" `
  --notes "Initial release - See README.md for details"

# Upload executable
gh release upload v1.0.0 "bin\Installers\AKK En-to-MM Dictionary.exe"
```

---

## 📤 Method 3: Upload via PowerShell Script (Automated)

### Using GitHub CLI Script
```powershell
# Save as upload-release.ps1

$executablePath = "bin\Installers\AKK En-to-MM Dictionary.exe"
$version = "v1.0.0"
$title = "AKK Dictionary v1.0.0 - Initial Release"

# Create release
gh release create $version `
  --title $title `
  --notes @release-notes.txt

# Upload file
gh release upload $version $executablePath

Write-Host "✅ Release created and executable uploaded!" -ForegroundColor Green
```

---

## ✅ Verification

After uploading, verify:

1. Go to: https://github.com/aungkokomm/AkkDictionaryApp/releases
2. You should see **v1.0.0 release** with:
   - ✅ Release title
   - ✅ Release description
   - ✅ Executable file (172.8 MB)
3. Download link for users:
   ```
   https://github.com/aungkokomm/AkkDictionaryApp/releases/download/v1.0.0/AKK%20En-to-MM%20Dictionary.exe
   ```

---

## 📋 Release Checklist

After creating the release:

- [ ] Release page created
- [ ] Executable uploaded
- [ ] Release description filled in
- [ ] Download link works
- [ ] File size shows ~172 MB
- [ ] Release is marked as "Latest"
- [ ] README.md links to releases page

---

## 🎯 Update Your README.md

Once the release is live, the README download link will automatically work:

```markdown
**Download and Run (Easiest)**
1. Download: [`AKK En-to-MM Dictionary.exe`](https://github.com/aungkokomm/AkkDictionaryApp/releases)
2. Double-click to run
3. Start searching!
```

This link points to your releases page automatically!

---

## 💡 Tips

1. **Large File Upload**: The 172 MB file may take a few minutes to upload via web interface
2. **Multiple Releases**: You can create multiple releases for different versions
3. **Pre-Release**: Mark as "pre-release" if not fully stable
4. **Latest Release**: GitHub automatically marks the newest as "Latest"
5. **Release Notes**: Use clear, detailed release notes for users

---

## 🚀 Future Releases

For version 1.1, 1.2, etc., repeat the process:
1. Update version in `AkkDictionaryApp.csproj`
2. Rebuild executable: `.\Build-Package.ps1`
3. Create new release (v1.1.0, etc.)
4. Upload new executable

---

## 📞 Support

If upload fails:
- Try smaller chunks
- Use GitHub CLI instead of web interface
- Check internet connection
- Ensure file isn't corrupted: `Get-Item "bin\Installers\*.exe" | ForEach-Object {$_.Length / 1MB}`

---

## Next Step

**Go to**: https://github.com/aungkokomm/AkkDictionaryApp/releases

**Click**: "Create a new release"

**Upload**: Your executable

**Done!** ✅
