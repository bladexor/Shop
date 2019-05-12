@echo off
:bucle
adb connect 192.168.0.202
timeout 30 /nobreak
goto bucle
exit