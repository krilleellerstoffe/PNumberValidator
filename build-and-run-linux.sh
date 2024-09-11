#!/bin/bash

# Check if .NET SDK is installed
dotnet_path=$(command -v dotnet)
if [ -n "$dotnet_path" ]
then
    echo ".NET SDK is installed at $dotnet_path."
else
    echo ".NET SDK is not installed or not found in PATH. Please install it from https://dotnet.microsoft.com/download."
    exit 1
fi


# Build the project
dotnet build

# Check if the build succeeded
if [ $? -ne 0 ]; then
    echo "Build failed. Please check for errors."
    exit 1
fi

# Run the project
echo ".............................."
dotnet run

# Check if the run succeeded
if [ $? -ne 0 ]; then
    echo "Execution failed. Please check for errors."
    exit 1
fi
