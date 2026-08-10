#!/bin/bash

set -e

# Run the build script to make sure there's something to serve
./build.sh

# Start DocFx serve
if [ -n "$1" ]; then
    dotnet docfx serve _site -p "$1"
else
    dotnet docfx serve _site
fi