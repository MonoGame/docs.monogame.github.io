# Exit on any error
$ErrorActionPreference = "Stop"

# Run the build script to make sure there's something to serve
try {
    .\build.ps1
}
catch {
    if (-not (Test-Path ".\_site")) {
        throw
    }
    Write-Host "Build reported an error: $_" -ForegroundColor Yellow
    Write-Host "Continuing to serve the existing _site output." -ForegroundColor Yellow
}

# Start DocFx serve
dotnet docfx serve .\_site