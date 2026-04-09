# Simple script to delete failed GitHub Actions workflow runs
# This script uses the GitHub REST API via PowerShell

param(
    [string]$Owner = "aungkokomm",
    [string]$Repo = "AkkDictionaryApp"
)

Write-Host "=" * 60
Write-Host "GitHub Failed Workflow Runs Cleaner" -ForegroundColor Cyan
Write-Host "=" * 60
Write-Host ""

# Instructions
Write-Host "To delete failed workflow runs, you have two options:" -ForegroundColor Yellow
Write-Host ""
Write-Host "OPTION 1: Using GitHub Web UI (Easiest)" -ForegroundColor Green
Write-Host "  1. Go to: https://github.com/$Owner/$Repo/actions"
Write-Host "  2. Click the 'Failed' filter on the left"
Write-Host "  3. Select all failed runs (use Ctrl+A)"
Write-Host "  4. Click 'Delete workflow runs'"
Write-Host ""

Write-Host "OPTION 2: Using GitHub CLI (Automated)" -ForegroundColor Green
Write-Host "  1. Get a Personal Access Token from GitHub:"
Write-Host "     https://github.com/settings/tokens?type=beta"
Write-Host "     (Select 'actions:write' scope)"
Write-Host "  2. Run: gh auth login"
Write-Host "  3. Then run:"
Write-Host ""
Write-Host "     # Delete all failed runs"
Write-Host '     gh run list --repo "$Owner/$Repo" --status failure --json databaseId -q ".[].databaseId" | % { gh run delete --repo "$Owner/$Repo" $_ }'
Write-Host ""

Write-Host "OPTION 3: Using REST API Directly (This Script)" -ForegroundColor Green
Write-Host "  1. Create a Personal Access Token (same as Option 2)"
Write-Host "  2. Run this command:"
Write-Host ""
Write-Host "     `$token = 'ghp_YOUR_TOKEN_HERE'"
Write-Host "     `$owner = '$Owner'"
Write-Host "     `$repo = '$Repo'"
Write-Host "     `$headers = @{Authorization='Bearer ' + `$token; Accept='application/vnd.github.v3+json'}"
Write-Host "     `$runs = (Invoke-RestMethod -Uri ""https://api.github.com/repos/`$owner/`$repo/actions/runs?status=failure"" -Headers `$headers).workflow_runs"
Write-Host "     `$runs | ForEach-Object { Invoke-RestMethod -Uri ""https://api.github.com/repos/`$owner/`$repo/actions/runs/`$_.id"" -Headers `$headers -Method Delete; Write-Host ""Deleted run #`$_.run_number"" }"
Write-Host ""

Write-Host "=" * 60
Write-Host "Recommendation: Use OPTION 1 (Web UI) - No setup needed!" -ForegroundColor Cyan
Write-Host "=" * 60
