@echo off
setlocal

:: Check if .NET SDK is installed
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo .NET SDK is not installed. Please install it from https://dotnet.microsoft.com/download.
    exit /b 1
)

:: Build the project
dotnet build

:: Check if the build succeeded
if errorlevel 1 (
    echo Build failed. Please check for errors.
    exit /b 1
)

:: Run the project
echo ...........................................
dotnet run

:: Check if the run succeeded
if errorlevel 1 (
    echo Execution failed. Please check for errors.
    exit /b 1
)

endlocal
pause
