@echo off
cls
cd %0\..\..
call start /BELOWNORMAL /B /WAIT .\php\php %*
