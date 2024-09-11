@echo off
setlocal

:: Check if .NET SDK is installed
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo .NET SDK is not installed. Please install it from https://dotnet.microsoft.com/download.
    exit /b 1
)

:: Build the main project
echo Building the main project...
dotnet build
if errorlevel 1 (
    echo Build failed. Please check for errors.
    exit /b 1
)

:: Run the tests
echo Running tests...
dotnet test
if errorlevel 1 (
    echo Tests failed. Please check for errors.
    exit /b 1
)

:: Run the main project
echo ...........................................
cd Main
dotnet run
if errorlevel 1 (
    echo Execution failed. Please check for errors.
    exit /b 1
)
cd ..

endlocal
pause
