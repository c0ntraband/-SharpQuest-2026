@echo off
dotnet run --project "%~dp0tools\sq" -- %* & exit /b
