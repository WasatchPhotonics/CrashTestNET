# CrashTestNET

A program to try to crash spectrometers using Wasatch.NET.

## Theory of Operation

This program is similar to WasatchNET.MultiChannelDemo -- so similar, you may ask
"why have two?"

Because MultiChannelDemo was designed as a simple example of how to make 
spectrometers work smoothly and well, while CrashTestNET was designed to beat the
ever-living crap out of them and make them howl.  Mixing those two sets of 
requirements into one app is just begging for trouble.

Also, MultiChannelDemo has a specific focus on HW triggering, which is not 
appropriate or useful for the majority of CrashTestNET's use cases.

## Threading

At the moment I'm going with BackgroundWorkers, not because I don't understand 
async methods, but because I want everything segmented into proper Threads (vs
Tasks).  We may add a Task option down the road.

## Command-Line

Unusually for a WinForms GUI, this app runs with a visible Console window (DOS 
shell).  This allows Console output to be viewed in the shell, and captured to a
file, when run from batch scripts.

Two batch scripts are provided for automated command-line execution, if start/stop
(enumeration) behavior is being tested.

- run-loop.bat
    - allows each execution of CrashTestNET to close normally and shutdown 
      cleanly
    - spectrometers are expected to function and resume without error across 
      executions

- run-ragged.bat 
    - abruptly and randomly kills each iteration to deliberately disrupt 
      communication state
    - USB spectrometers may "fall over" at some point during this test and 
      require power-cycle / reset

## Changelog

- see [Changelog](README_CHANGELOG.md)
