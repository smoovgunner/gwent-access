@echo off
rem Puts a "Gwent Accessible" shortcut on the desktop that starts Gwent without the launcher.
powershell -NoProfile -Command "$s=(New-Object -ComObject WScript.Shell).CreateShortcut([Environment]::GetFolderPath('Desktop')+'\Gwent Accessible.lnk'); $s.TargetPath='%~dp0Gwent-NoLauncher.bat'; $s.WorkingDirectory='%~dp0'; $s.Save()"
if errorlevel 1 (echo Could not create the shortcut.) else (echo Done. The Gwent Accessible shortcut is on your desktop.)
pause
