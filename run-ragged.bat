@echo off

REM This script is provided to run CrashTestNET in a loop, forcing it to end
REM abruptly before each test run completes.  
REM
REM It is HIGHLY LIKELY that this script may leave some spectrometers in an
REM unresponsive state, requiring power-cycle to restore to operation.  That
REM is it's purpose: this script is provided specifically to crash spectrometers
REM and allow profiling of how, when and why they fail, and how that behavior
REM may be improved in subsequent versions.

set ITERATIONS=%1

set LOGFILE=%CD%\CrashTestNET.log
set OLD_DIR=%CD%

if exist src\bin\x86\Debug (
    cd src\bin\x86\Debug
)
echo Running in %CD%

del /f %LOGFILE% 2>NUL

for /l %%i in (1, 1, %ITERATIONS%) do (
    set /a TIMEOUT_SEC=%RANDOM% * 50 / 32768 + 10

    echo Iteration %%i of %ITERATIONS% (timeout %TIMEOUT_SEC% sec)
    echo Iteration %%i of %ITERATIONS% (timeout %TIMEOUT_SEC% sec) >> %LOGFILE%
    timeout %TIMEOUT_SEC% CrashTestNET --start --duration-sec 60 --metrics --report >> %LOGFILE%
)

cd %OLD_DIR%

grep Report %LOGFILE%

echo Done crashing CrashTestNET
