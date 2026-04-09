# COPY-PASTE THIS COMMAND TO DELETE ALL FAILED WORKFLOWS
# 
# Step 1: Get your Personal Access Token from:
#         https://github.com/settings/tokens?type=beta
#         (Select: actions:write scope)
#
# Step 2: Replace YOUR_TOKEN_HERE with your actual token and run this:

# ========== COMMAND START ==========

$token = "YOUR_TOKEN_HERE"
$owner = "aungkokomm"
$repo = "AkkDictionaryApp"

$headers = @{
    "Authorization" = "Bearer $token"
    "Accept" = "application/vnd.github.v3+json"
}

# Get all failed runs
$response = Invoke-RestMethod `
    -Uri "https://api.github.com/repos/$owner/$repo/actions/runs?status=failure&per_page=100" `
    -Headers $headers

$failedRuns = $response.workflow_runs

Write-Host "Found $($failedRuns.Count) failed runs. Deleting..." -ForegroundColor Yellow

# Delete each run
$failedRuns | ForEach-Object {
    $runId = $_.id
    $runNumber = $_.run_number
    
    Invoke-RestMethod `
        -Uri "https://api.github.com/repos/$owner/$repo/actions/runs/$runId" `
        -Headers $headers `
        -Method Delete | Out-Null
    
    Write-Host "✓ Deleted run #$runNumber" -ForegroundColor Green
}

Write-Host "✅ All failed runs deleted!" -ForegroundColor Green

# ========== COMMAND END ==========
