# GitHub Release Troubleshooting Script
# Diagnoses issues with GitHub releases

Write-Host "`n╔════════════════════════════════════════════════════════════════════════════════╗" -ForegroundColor Yellow
Write-Host "║         GitHub Release Troubleshooting - AKK Dictionary                      ║" -ForegroundColor Yellow
Write-Host "╚════════════════════════════════════════════════════════════════════════════════╝`n" -ForegroundColor Yellow

# ============================================================================
# CHECK 1: GitHub CLI Installation
# ============================================================================
Write-Host "✓ CHECK 1: GitHub CLI Installation" -ForegroundColor Cyan
Write-Host "─────────────────────────────────────────────────────────────────────────────────" -ForegroundColor Cyan

$ghVersion = gh --version 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ GitHub CLI is installed" -ForegroundColor Green
    Write-Host "   Version: $ghVersion`n" -ForegroundColor Green
} else {
    Write-Host "❌ GitHub CLI is NOT installed!" -ForegroundColor Red
    Write-Host "   Install from: https://cli.github.com`n" -ForegroundColor Red
    Write-Host "   Windows: winget install GitHub.cli`n" -ForegroundColor Yellow
    exit 1
}

# ============================================================================
# CHECK 2: GitHub Authentication
# ============================================================================
Write-Host "✓ CHECK 2: GitHub Authentication" -ForegroundColor Cyan
Write-Host "─────────────────────────────────────────────────────────────────────────────────" -ForegroundColor Cyan

$authStatus = gh auth status 2>&1
Write-Host $authStatus
Write-Host ""

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ NOT authenticated with GitHub" -ForegroundColor Red
    Write-Host "   Run: gh auth login`n" -ForegroundColor Yellow
    exit 1
}

Write-Host "✅ Authenticated with GitHub`n" -ForegroundColor Green

# ============================================================================
# CHECK 3: Repository Access
# ============================================================================
Write-Host "✓ CHECK 3: Repository Access" -ForegroundColor Cyan
Write-Host "─────────────────────────────────────────────────────────────────────────────────" -ForegroundColor Cyan

$repoInfo = gh repo view aungkokomm/AkkDictionaryApp 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Can access repository: aungkokomm/AkkDictionaryApp`n" -ForegroundColor Green
} else {
    Write-Host "❌ Cannot access repository!" -ForegroundColor Red
    Write-Host $repoInfo
    Write-Host ""
    exit 1
}

# ============================================================================
# CHECK 4: Existing Releases
# ============================================================================
Write-Host "✓ CHECK 4: Existing Releases" -ForegroundColor Cyan
Write-Host "─────────────────────────────────────────────────────────────────────────────────" -ForegroundColor Cyan

$releases = gh release list 2>&1
if ($releases) {
    Write-Host "Releases found:" -ForegroundColor Yellow
    Write-Host $releases
    Write-Host ""
} else {
    Write-Host "No releases found yet" -ForegroundColor Yellow
    Write-Host ""
}

# ============================================================================
# CHECK 5: Executable File
# ============================================================================
Write-Host "✓ CHECK 5: Executable File" -ForegroundColor Cyan
Write-Host "─────────────────────────────────────────────────────────────────────────────────" -ForegroundColor Cyan

$executablePath = "bin\Installers\AKK En-to-MM Dictionary.exe"
if (Test-Path $executablePath) {
    $fileSize = [math]::Round((Get-Item $executablePath).Length / 1MB, 2)
    Write-Host "✅ Executable found" -ForegroundColor Green
    Write-Host "   Path: $executablePath"
    Write-Host "   Size: $fileSize MB`n" -ForegroundColor Green
} else {
    Write-Host "❌ Executable NOT found!" -ForegroundColor Red
    Write-Host "   Expected: $executablePath`n" -ForegroundColor Red
    exit 1
}

# ============================================================================
# CHECK 6: Check for v1.0.0 Release
# ============================================================================
Write-Host "✓ CHECK 6: Check for v1.0.0 Release" -ForegroundColor Cyan
Write-Host "─────────────────────────────────────────────────────────────────────────────────" -ForegroundColor Cyan

$release = gh release view v1.0.0 2>&1
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Release v1.0.0 EXISTS!" -ForegroundColor Green
    Write-Host $release
    Write-Host ""
    Write-Host "Status: Release was created but might be a DRAFT"
    Write-Host "Solution: Edit the release and click 'Publish release'`n" -ForegroundColor Yellow
} else {
    Write-Host "⚠️  Release v1.0.0 NOT FOUND" -ForegroundColor Yellow
    Write-Host "   This means the release wasn't created`n" -ForegroundColor Yellow
}

# ============================================================================
# SUMMARY
# ============================================================================
Write-Host "╔════════════════════════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║                         TROUBLESHOOTING SUMMARY                               ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════════════════════════╝`n" -ForegroundColor Green

Write-Host "Status: All checks passed ✅" -ForegroundColor Green
Write-Host ""
Write-Host "If release v1.0.0 was NOT found above:" -ForegroundColor Yellow
Write-Host "  1. Run: .\Upload-Release.ps1 (again)"
Write-Host "  2. Or manually create: gh release create v1.0.0 --title 'AKK Dictionary v1.0.0'`n" -ForegroundColor Green

Write-Host "If release v1.0.0 WAS found above:" -ForegroundColor Yellow
Write-Host "  1. It might be a DRAFT (not published)"
Write-Host "  2. Go to: https://github.com/aungkokomm/AkkDictionaryApp/releases"
Write-Host "  3. Find v1.0.0 draft"
Write-Host "  4. Click 'Publish release'`n" -ForegroundColor Green

Write-Host "Repository: https://github.com/aungkokomm/AkkDictionaryApp`n" -ForegroundColor Cyan
