$ErrorActionPreference = "Stop"

Write-Host "Building MonoGame Assemblies..."-ForegroundColor Green
dotnet build external/MonoGame/MonoGame.Framework.Content.Pipeline/MonoGame.Framework.Content.Pipeline.csproj -p:DisableNativeBuild=true

Write-Host "Running DocFX..." -ForegroundColor Green
dotnet docfx docfx.json

Write-Host "Build and documentation generation completed successfully!" -ForegroundColor Green