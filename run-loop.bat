@echo off

REM This script is provided to run CrashTestNET in a loop, letting it exit
REM normally when each test run completes.  

set ITERATIONS=%1

set LOGFILE=%CD%\CrashTestNET.log
set OLD_DIR=%CD%

if exist src\bin\x86\Debug (
    cd src\bin\x86\Debug
)
echo Running in %CD%

del /f %LOGFILE% 2>NUL

for /l %%i in (1, 1, %ITERATIONS%) do (
    echo Iteration %%i of %ITERATIONS%
    echo Iteration %%i of %ITERATIONS% >> %LOGFILE%
    CrashTestNET --start --duration-sec 60 --integ-min 100 --integ-max 500 --metrics --report >> %LOGFILE%
)

cd %OLD_DIR%

grep Report %LOGFILE%

echo Done running CrashTestNET
