# Delete all failed GitHub Actions workflow runs
# Usage: ./delete-failed-workflows.ps1

param(
    [Parameter(Mandatory=$true)]
    [string]$Owner,
    
    [Parameter(Mandatory=$true)]
    [string]$Repo,
    
    [Parameter(Mandatory=$true)]
    [string]$Token,
    
    [Parameter(ValueFromPipeline=$false)]
    [switch]$DryRun = $false
)

$headers = @{
    "Authorization" = "Bearer $Token"
    "Accept" = "application/vnd.github.v3+json"
}

Write-Host "🔍 Fetching failed workflow runs for $Owner/$Repo..." -ForegroundColor Cyan

try {
    # Get all failed runs
    $runs = @()
    $page = 1
    
    do {
        $url = "https://api.github.com/repos/$Owner/$Repo/actions/runs?status=failure&per_page=100&page=$page"
        $response = Invoke-RestMethod -Uri $url -Headers $headers -Method Get
        
        if ($response.workflow_runs.Count -eq 0) {
            break
        }
        
        $runs += $response.workflow_runs
        $page++
        
        Write-Host "  Fetched $($response.workflow_runs.Count) runs from page $page..." -ForegroundColor Gray
    } while ($response.workflow_runs.Count -eq 100)
    
    if ($runs.Count -eq 0) {
        Write-Host "✅ No failed workflow runs found!" -ForegroundColor Green
        exit 0
    }
    
    Write-Host "Found $($runs.Count) failed workflow runs:" -ForegroundColor Yellow
    Write-Host ""
    
    # Display failed runs
    $runs | ForEach-Object {
        $status = $_.conclusion
        $date = $_.updated_at
        Write-Host "  ❌ Run #$($_.run_number) - $($_.name) - $date" -ForegroundColor Red
    }
    
    Write-Host ""
    
    if ($DryRun) {
        Write-Host "🧪 [DRY RUN] Would delete $($runs.Count) failed workflow runs" -ForegroundColor Yellow
        exit 0
    }
    
    # Confirm deletion
    $confirm = Read-Host "Delete all $($runs.Count) failed workflow runs? (yes/no)"
    if ($confirm -ne "yes") {
        Write-Host "❌ Cancelled" -ForegroundColor Yellow
        exit 0
    }
    
    # Delete runs
    $deleted = 0
    $runs | ForEach-Object {
        $runId = $_.id
        $runNumber = $_.run_number
        
        $url = "https://api.github.com/repos/$Owner/$Repo/actions/runs/$runId"
        
        try {
            Invoke-RestMethod -Uri $url -Headers $headers -Method Delete | Out-Null
            $deleted++
            Write-Host "  ✓ Deleted run #$runNumber" -ForegroundColor Green
        } catch {
            Write-Host "  ✗ Failed to delete run #$runNumber - $($_.Exception.Message)" -ForegroundColor Red
        }
    }
    
    Write-Host ""
    Write-Host "✅ Successfully deleted $deleted out of $($runs.Count) failed workflow runs!" -ForegroundColor Green
    
} catch {
    Write-Host "❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
