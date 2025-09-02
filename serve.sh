#!/bin/bash

set -e

# Run the build script to make sure there's something to serve
./build.sh

# Start DocFx serve
dotnet docfx docfx.json --serve