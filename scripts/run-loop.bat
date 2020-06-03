@echo off

REM This script is provided to run CrashTestNET in a loop, letting it exit
REM normally when each test run completes.  

set ITERATIONS=%1
set COUNT=0

del CrashTestNET.log 2>NUL

:LOOP_START

echo Iteration %COUNT%: running CrashTestNET
CrashTestNET --start >> CrashTestNET.log

set /a COUNT+=1
if %COUNT% LSS %ITERATIONS% goto LOOP_START

echo Done running CrashTestNET
