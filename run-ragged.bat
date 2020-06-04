@echo off

REM This script is provided to run CrashTestNET in a loop, forcing it to end
REM abruptly before each test run completes.  

set ITERATIONS=%1

set OLD_DIR=%CD%
if exist src\bin\x86\Debug (
    cd src\bin\x86\Debug
)
echo Running in %CD%

del CrashTestNET.log 2>NUL

for /l %%i in (1, 1, %ITERATIONS%) do (
    set /a TIMEOUT_SEC=%RANDOM% * 50 / 32768 + 10

    echo Iteration %%i (timeout %TIMEOUT_SEC% sec)
    echo Iteration %%i (timeout %TIMEOUT_SEC% sec) >> CrashTestNET.log
    timeout %TIMEOUT_SEC% CrashTestNET --start --duration-sec 60 >> CrashTestNET.log
)

cd %OLD_DIR%
echo Done crashing CrashTestNET
