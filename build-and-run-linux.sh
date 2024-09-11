#!/bin/bash

# Check if .NET SDK is installed
dotnet_path=$(command -v dotnet)
if [ -n "$dotnet_path" ]; then
    echo ".NET SDK is installed at $dotnet_path."
else
    echo ".NET SDK is not installed or not found in PATH. Please install it from https://dotnet.microsoft.com/download."
    exit 1
fi

# Build the main project
echo "Building the main project..."
dotnet build
if [ $? -ne 0 ]; then
    echo "Build of the main project failed. Please check for errors."
    exit 1
fi

# Run the tests
echo "Running tests..."
dotnet test
if [ $? -ne 0 ]; then
    echo "Tests failed. Please check for errors."
    exit 1
fi

# Run the main project
echo "Running the main project..."
cd Main
dotnet run
if [ $? -ne 0 ]; then
    echo "Execution of the main project failed. Please check for errors."
    exit 1
fi
cd..

# End of script
