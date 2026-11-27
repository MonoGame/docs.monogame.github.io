#!/bin/bash

set -e

FRAMEWORK_DLL="external/MonoGame/Artifacts/MonoGame.Framework/DesktopGL/Debug/MonoGame.Framework.dll"
NATIVE_FRAMEWORK_DLL="external/MonoGame/Artifacts/MonoGame.Framework/Native/Debug/MonoGame.Framework.dll"
PIPELINE_DLL="external/MonoGame/Artifacts/MonoGame.Framework.Content.Pipeline/Debug/MonoGame.Framework.Content.Pipeline.dll"

# Check if submodules are initialized
if git submodule status | grep -q '^-'; then
    echo "Submodules not initialized.  Running git submodule update..."
    git submodule update --init --recursive
fi

# Check if dlls are already built
if [ ! -f "$FRAMEWORK_DLL" ] || [ ! -f "$NATIVE_FRAMEWORK_DLL" ] || [ ! -f "$PIPELINE_DLL" ]; then
    echo "Required Assemblies for documentation not found.  Building assemblies..."
    dotnet build external/MonoGame/MonoGame.Framework.Content.Pipeline/MonoGame.Framework.Content.Pipeline.csproj -p:DisableNativeBuild=true
    dotnet build external/MonoGame/MonoGame.Framework/MonoGame.Framework.Native.csproj -p:DisableNativeBuild=true
fi

# Build documentation
echo "Building DocFx..."
dotnet docfx metadata docfx.json

# Mark the API entries that only exist in the Native platform. Needs the .NET 10 SDK.
# The API reference is generated from the Native assembly, with DesktopGL as the baseline.
echo "Marking Native only API entries..."
dotnet run tools/ApiPlatformMarker.cs -- api _generated/api-desktopgl _generated/api-overwrite/native-only.md

# Build documentation
echo "Building DocFx..."
dotnet docfx docfx.json

mkdir -p _pdf

# Generate PDF (using PDF-specific config that includes pdf/** files)
echo "Generating PDF..."
dotnet docfx build docfx.pdf.json --output _pdf
dotnet docfx pdf docfx.pdf.json --output _pdf

# Copy PDF to downloads folder and clean up
echo "Copying PDF to downloads folder..."
mkdir -p _site/downloads
cp _pdf/pdf/MonoGameGuide.pdf _site/downloads/
rm -rf _pdf

echo "Build and documentation generation completed successfully!"