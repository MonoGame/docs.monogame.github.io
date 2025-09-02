#!/bin/bash

set -e

echo "Building MonoGame Assemblies..."
dotnet build external/MonoGame/MonoGame.Framework.Content.Pipeline/MonoGame.Framework.Content.Pipeline.csproj -p:DisableNativeBuild=true

echo "Running DocFX..."
dotnet docfx docfx.json

echo "Build and documentation generation completed successfully!"