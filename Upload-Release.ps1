# Upload Release to GitHub
# This script creates a GitHub release and uploads the executable

Write-Host "`n╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║         GitHub Release Upload Script for AKK Dictionary      ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════════╝`n" -ForegroundColor Cyan

# Configuration
$executablePath = "bin\Installers\AKK En-to-MM Dictionary.exe"
$version = "v1.0.0"
$releaseTitle = "AKK Dictionary v1.0.0 - Initial Release"
$releaseNotes = @"
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
The executable file is attached below.

## Quick Start
1. Download the `.exe` file
2. Double-click to run
3. Start searching!

## Documentation
- [README](../../blob/master/README.md) - Project overview
- [QUICK_START.md](../../blob/master/QUICK_START.md) - User guide
- [BUILD_AND_DISTRIBUTION.md](../../blob/master/BUILD_AND_DISTRIBUTION.md) - Developer guide

## License
MIT License - See LICENSE.txt for details

## Support
- Report bugs: [GitHub Issues](https://github.com/aungkokomm/AkkDictionaryApp/issues)
- Questions: See documentation files
"@

# Check if executable exists
Write-Host "📋 Checking for executable..." -ForegroundColor Yellow
if (-not (Test-Path $executablePath)) {
    Write-Host "❌ ERROR: Executable not found at: $executablePath" -ForegroundColor Red
    Write-Host "   Make sure to build the project first: .\Build-Package.ps1`n" -ForegroundColor Red
    exit 1
}

$fileSize = [math]::Round((Get-Item $executablePath).Length / 1MB, 2)
Write-Host "✅ Executable found!" -ForegroundColor Green
Write-Host "   Name: AKK En-to-MM Dictionary.exe"
Write-Host "   Size: $fileSize MB`n" -ForegroundColor Green

# Check if GitHub CLI is installed
Write-Host "🔍 Checking for GitHub CLI..." -ForegroundColor Yellow
$ghInstalled = gh --version 2>$null
if (-not $ghInstalled) {
    Write-Host "❌ GitHub CLI not found!" -ForegroundColor Red
    Write-Host "   Install from: https://github.com/cli/cli/releases`n" -ForegroundColor Red
    Write-Host "   Or use web interface: https://github.com/aungkokomm/AkkDictionaryApp/releases`n" -ForegroundColor Yellow
    exit 1
}

Write-Host "✅ GitHub CLI installed: $ghInstalled`n" -ForegroundColor Green

# Check authentication
Write-Host "🔐 Checking GitHub authentication..." -ForegroundColor Yellow
$authStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Not authenticated with GitHub" -ForegroundColor Red
    Write-Host "   Run: gh auth login`n" -ForegroundColor Yellow
    exit 1
}
Write-Host "✅ Authenticated with GitHub`n" -ForegroundColor Green

# Save release notes to temp file
$notesFile = [System.IO.Path]::GetTempFileName()
$releaseNotes | Out-File $notesFile -Encoding UTF8

# Create release
Write-Host "📤 Creating release: $version..." -ForegroundColor Cyan
gh release create $version `
    --title $releaseTitle `
    --notes-file $notesFile `
    2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Failed to create release`n" -ForegroundColor Red
    Remove-Item $notesFile -Force
    exit 1
}

Write-Host "✅ Release created!`n" -ForegroundColor Green

# Upload executable
Write-Host "📦 Uploading executable (172.8 MB - this may take a few minutes)..." -ForegroundColor Cyan
Write-Host "   Please wait...$n" -ForegroundColor Yellow

gh release upload $version $executablePath 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Failed to upload executable`n" -ForegroundColor Red
    Remove-Item $notesFile -Force
    exit 1
}

Write-Host "`n✅ Upload complete!`n" -ForegroundColor Green

# Cleanup
Remove-Item $notesFile -Force

# Display success message
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║                    ✅ RELEASE CREATED! ✅                    ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════════╝`n" -ForegroundColor Green

Write-Host "📍 Release URL:" -ForegroundColor Cyan
Write-Host "   https://github.com/aungkokomm/AkkDictionaryApp/releases/tag/$version`n" -ForegroundColor Yellow

Write-Host "📥 Download URL:" -ForegroundColor Cyan
Write-Host "   https://github.com/aungkokomm/AkkDictionaryApp/releases/download/$version/AKK%20En-to-MM%20Dictionary.exe`n" -ForegroundColor Yellow

Write-Host "✨ Users can now download the executable from the releases page!`n" -ForegroundColor Green
