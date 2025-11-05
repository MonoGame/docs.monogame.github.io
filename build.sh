#!/bin/bash

set -e

FRAMEWORK_DLL="external/MonoGame/Artifacts/MonoGame.Framework/DesktopGL/Debug/MonoGame.Framework.dll"
PIPELINE_DLL="external/MonoGame/Artifacts/MonoGame.Framework.Content.Pipeline/Debug/MonoGame.Framework.Content.Pipeline.dll"

# Check if submodules are initialized
if git submodule status | grep -q '^-'; then
    echo "Submodules not initialized.  Running git submodule update..."
    git submodule update --init --recursive
fi

# Check if dlls are already built
if [ ! -f "$FRAMEWORK_DLL" ] || [ ! -f "$PIPELINE_DLL" ]; then
    echo "Required Assemblies for documentation not found.  Building assemblies..."
    dotnet build external/MonoGame/MonoGame.Framework.Content.Pipeline/MonoGame.Framework.Content.Pipeline.csproj -p:DisableNativeBuild=true
fi

# Build documentation
echo "Building DocFx..."
dotnet docfx docfx.json

# Generate PDF
echo "Generating PDF..."
dotnet docfx pdf docfx.json

# Copy PDF to downloads folder and clean up
echo "Copying PDF to downloads folder..."
mkdir -p _site/downloads
cp _site/pdf/MonoGameGuide.pdf _site/downloads/
rm -rf _site/pdf

echo "Build and documentation generation completed successfully!"