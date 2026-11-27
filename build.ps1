# Exit on any error
$ErrorActionPreference = "Stop"

$FrameworkDll = "external/MonoGame/Artifacts/MonoGame.Framework/DesktopGL/Debug/MonoGame.Framework.dll"
$NativeFrameworkDll = "external/MonoGame/Artifacts/MonoGame.Framework/Native/Debug/MonoGame.Framework.dll"
$PipelineDll = "external/MonoGame/Artifacts/MonoGame.Framework.Content.Pipeline/Debug/MonoGame.Framework.Content.Pipeline.dll"

# Check if submodules are initialized
$submoduleStatus = git submodule status
if ($submoduleStatus -match '^-') {
    Write-Host "Submodules not initialized. Running git submodule update..." -ForegroundColor Yellow
    git submodule update --init --recursive
}

# Check if dlls are already built
if (-not (Test-Path $FrameworkDll) -or -not (Test-Path $NativeFrameworkDll) -or -not (Test-Path $PipelineDll)) {
    Write-Host "Required Assemblies for documentation not found. Building assemblies..." -ForegroundColor Yellow
    dotnet build external/MonoGame/MonoGame.Framework.Content.Pipeline/MonoGame.Framework.Content.Pipeline.csproj -p:DisableNativeBuild=true
    dotnet build external/MonoGame/MonoGame.Framework/MonoGame.Framework.Native.csproj -p:DisableNativeBuild=true
}

# Build documentation
Write-Host "Building DocFx..." -ForegroundColor Green
dotnet docfx metadata docfx.json

# Mark the API entries that only exist in the Native platform. Needs the .NET 10 SDK.
# The API reference is generated from the Native assembly, with DesktopGL as the baseline.
Write-Host "Marking Native only API entries..." -ForegroundColor Green
dotnet run tools/ApiPlatformMarker.cs -- api _generated/api-desktopgl _generated/api-overwrite/native-only.md
if ($LASTEXITCODE -ne 0) {
    throw "ApiPlatformMarker exited with code $LASTEXITCODE"
}

# Build documentation
Write-Host "Building DocFx..." -ForegroundColor Green
dotnet docfx build docfx.json

New-Item -ItemType Directory -Force -Path "_pdf" | Out-Null

# Generate PDF (using PDF-specific config that includes pdf/** files)
Write-Host "Generating PDF..." -ForegroundColor Green
try {
    dotnet docfx build docfx.pdf.json --output _pdf
    dotnet docfx pdf docfx.pdf.json --output _pdf
    if ($LASTEXITCODE -ne 0) {
        throw "docfx pdf exited with code $LASTEXITCODE"
    }

    # Copy PDF to downloads folder
    Write-Host "Copying PDF to downloads folder..." -ForegroundColor Green
    New-Item -ItemType Directory -Force -Path "_site/downloads" | Out-Null
    Copy-Item "_pdf/pdf/MonoGameGuide.pdf" "_site/downloads/" -ErrorAction Stop
}
catch {
    Write-Warning "PDF generation failed: $_"
    Write-Warning "Continuing without PDF - the site will be served/built without the downloadable guide."
}
finally {
    if (Test-Path "_pdf") {
        Remove-Item -Path "_pdf" -Recurse -Force -ErrorAction SilentlyContinue
    }
}

Write-Host "Build and documentation generation completed successfully!" -ForegroundColor Green