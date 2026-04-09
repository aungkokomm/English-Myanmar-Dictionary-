#!/usr/bin/env pwsh
# Delete all failed GitHub Actions workflow runs using GitHub CLI

Write-Host "🔐 Starting GitHub CLI authentication..." -ForegroundColor Cyan
Write-Host "This will open your browser to authenticate with GitHub." -ForegroundColor Yellow
Write-Host ""

# Authenticate
gh auth login -p https -w

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Authentication failed" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Successfully authenticated!" -ForegroundColor Green
Write-Host ""

$owner = "aungkokomm"
$repo = "AkkDictionaryApp"

Write-Host "🔍 Fetching failed workflow runs for $owner/$repo..." -ForegroundColor Cyan
Write-Host ""

# Get failed run IDs
$failedRuns = @(gh run list --repo "$owner/$repo" --status failure --json databaseId,number,name,updatedAt -q '.[] | "\(.databaseId)|\(.number)|\(.name)|\(.updatedAt)"')

if ($failedRuns.Count -eq 0) {
    Write-Host "✅ No failed workflow runs found!" -ForegroundColor Green
    exit 0
}

Write-Host "Found $($failedRuns.Count) failed workflow runs:" -ForegroundColor Yellow
Write-Host ""

# Display failed runs
$failedRuns | ForEach-Object {
    $parts = $_ -split "\|"
    $id = $parts[0]
    $number = $parts[1]
    $name = $parts[2]
    $updated = $parts[3]
    Write-Host "  ❌ Run #$number - $name - $updated"
}

Write-Host ""
$response = Read-Host "Delete all $($failedRuns.Count) failed workflow runs? (type 'yes' to confirm)"

if ($response -ne "yes") {
    Write-Host "❌ Cancelled" -ForegroundColor Yellow
    exit 0
}

Write-Host ""
Write-Host "🗑️  Deleting failed workflow runs..." -ForegroundColor Red
Write-Host ""

$deleted = 0
$failed = 0

$failedRuns | ForEach-Object {
    $parts = $_ -split "\|"
    $id = $parts[0]
    $number = $parts[1]
    
    try {
        gh run delete --repo "$owner/$repo" $id --confirm 2>&1 | Out-Null
        $deleted++
        Write-Host "  ✓ Deleted run #$number" -ForegroundColor Green
    } catch {
        $failed++
        Write-Host "  ✗ Failed to delete run #$number" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "=" * 60
Write-Host "✅ Successfully deleted $deleted out of $($failedRuns.Count) failed workflow runs!" -ForegroundColor Green
if ($failed -gt 0) {
    Write-Host "⚠️  Failed to delete $failed runs" -ForegroundColor Yellow
}
Write-Host "=" * 60
