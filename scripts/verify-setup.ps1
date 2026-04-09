# Verify Build Environment
Write-Host "Checking .NET SDK..." -ForegroundColor Green
dotnet --version

Write-Host "Checking Git..." -ForegroundColor Green
git --version

Write-Host "Checking Docker..." -ForegroundColor Green
docker --version

Write-Host "Build environment check complete!" -ForegroundColor Green
