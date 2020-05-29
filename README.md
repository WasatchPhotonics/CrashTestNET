# CrashTestNET

A program to try to crash spectrometers using Wasatch.NET.

## Theory of Operation

This program is similar to WasatchNET.MultiChannelDemo -- so similar, you may ask
"why have two?"

Because MultiChannelDemo was designed as a simple example of how to make 
spectrometers work smoothly and well, while CrashTestNET was designed to beat the
ever-living crap out of them and make them howl.  Mixing those two sets of 
requirements into one app is just begging for trouble.

## Threading

At the moment I'm going with BackgroundWorkers, not because I don't understand 
async methods, but because I want everything segmented into proper Threads (vs
Tasks).  We may add a Task option down the road.

## Backlog

Add "Summary" tab with table of iterations, failures, shifts etc by spectrometer.
