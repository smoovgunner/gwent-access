@echo off
rem Starts Gwent without the REDlauncher window. Steam must be running.
cd /d "%~dp0"
set SteamAppId=1284410
set SteamGameId=1284410
set SteamOverlayGameId=1284410
start "" "%~dp0REDprelauncher.exe" --launcher-skip
