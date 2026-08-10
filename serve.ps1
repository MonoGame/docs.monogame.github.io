param (
    # Accepts an optional port number as the first argument.
    [int]$Port
)

# Exit on any error
$ErrorActionPreference = "Stop"

# Run the build script to make sure there's something to serve
.\build.ps1

# Start DocFx serve
if ($Port) {
    dotnet docfx serve .\_site -p $Port
}
else {
    dotnet docfx serve .\_site
}