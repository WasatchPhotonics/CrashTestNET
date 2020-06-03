@echo off

REM This script is provided to run CrashTestNET through a "ragged" series of rude 
REM aborts, to qualify spectrometer behavior when the controlling application
REM rudely exits.

set ITERATIONS=%1
set COUNT=0

del CrashTestNET.log 2>NUL

:LOOP_START
set /a TIMEOUT_SEC=%RANDOM% * 50 / 32768 + 10

echo Iteration %COUNT%: running CrashTestNET (timeout %TIMEOUT_SEC% sec)
timeout %TIMEOUT_SEC% CrashTestNET --start >> CrashTestNET.log

set /a COUNT+=1
if %COUNT% LSS %ITERATIONS% goto LOOP_START

echo Done crashing CrashTestNET
