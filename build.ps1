# Exit on any error
$ErrorActionPreference = "Stop"

$FrameworkDll = "external/MonoGame/Artifacts/MonoGame.Framework/DesktopGL/Debug/MonoGame.Framework.dll"
$PipelineDll = "external/MonoGame/Artifacts/MonoGame.Framework.Content.Pipeline/Debug/MonoGame.Framework.Content.Pipeline.dll"

# Check if submodules are initialized
$submoduleStatus = git submodule status
if ($submoduleStatus -match '^-') {
    Write-Host "Submodules not initialized. Running git submodule update..." -ForegroundColor Yellow
    git submodule update --init --recursive
}

# Check if dlls are already built
if (-not (Test-Path $FrameworkDll) -or -not (Test-Path $PipelineDll)) {
    Write-Host "Required Assemblies for documentation not found. Building assemblies..." -ForegroundColor Yellow
    dotnet build external/MonoGame/MonoGame.Framework.Content.Pipeline/MonoGame.Framework.Content.Pipeline.csproj -p:DisableNativeBuild=true
}

# Build documentation
Write-Host "Building DocFx..." -ForegroundColor Green
dotnet docfx metadata docfx.json

# Build documentation
Write-Host "Building DocFx..." -ForegroundColor Green
dotnet docfx build docfx.json

New-Item -ItemType Directory -Force -Path "_pdf" | Out-Null

# Generate PDF (using PDF-specific config that includes pdf/** files)
Write-Host "Generating PDF..." -ForegroundColor Green
dotnet docfx pdf docfx.pdf.json --output _pdf

# Copy PDF to downloads folder and clean up
Write-Host "Copying PDF to downloads folder..." -ForegroundColor Green
New-Item -ItemType Directory -Force -Path "_site/downloads" | Out-Null
Copy-Item "_pdf/pdf/MonoGameGuide.pdf" "_site/downloads/"
Remove-Item -Path "_pdf" -Recurse -Force

Write-Host "Build and documentation generation completed successfully!" -ForegroundColor Green