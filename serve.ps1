# Exit on any error
$ErrorActionPreference = "Stop"

# Run the build script to make sure there's something to serve
.\build.ps1

# Start DocFx serve
dotnet docfx serve .\_site