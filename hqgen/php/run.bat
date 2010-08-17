@echo off
cls
cd %0\..\..
call start /B /WAIT .\php\php %*
echo.
echo.
pause
