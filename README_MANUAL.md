![GUI](https://github.com/WasatchPhotonics/CrashTestNET/raw/master/screenshots/gui-01.png)

# CrashTestNET Operator's Manual

The basic purpose of CrashTestNET is to run as many spectrometers as possible,
as fast as possible, pushing them to do as many things in parallel as they can,
in order to try to "crash" either the spectrometer firmware (microcontroller and
FPGA) or the USB application library (Wasatch.NET).

# Interactive GUI Operation

These are the key buttons and indicators on the GUI:

- [x] verbose -- if checked, will add a lot of extra detail in the Event Log
  (especially during Initialization)
- Initialize -- enumerates and initializes all currently connected USB
  Wasatch Photonics spectrometers (has been tested with 6+ in parallel)
- Start / Stop -- begins or terminates a test iteration, during which
  each spectrometer is continuously assigned a random integration time within
  prescribed limits, commanded to generate one spectrum (graphed), then start
  over in rapid order.

The key arguments to a test are:

- how long you want it to run (e.g. 1min, 5min etc)
- the valid range of random integration times (e.g., 10ms to 3000ms)

There are other, less-commonly used options described elsewhere in this document.

During such a test, the program continually updates a graph (similar to ENLIGHTEN)
showing the most-recent spectrum read from each spectrometer, and a summary table
underneath listing each spectrometer's:

- serial number
- model
- microcontroller firmware version
- FPGA firmware version
- current integration time (constantly changing)
- current detector temperature (if supported)
- whether the spectrometer is currently "running" in an ongoing test iteration
- how many acquisitions (and therefore, random integration times) have been
  taken from that spectrometer during this iteration
- how many "read failures" have occurred (attempts to read a spectrum which
  returned NULL or an incomplete number of pixels for whatever reason)
- how many x-axis "shifts" have been detected in measured spectra (only valid
  for R&D spectrometers with a special start-of-spectrum 0xffff "marker" 
  hardcoded as the first pixel)
- current tally of "consecutive failures" meaning "read failures" which occurred
  one-right-after-another, as after a set threshold of consecutive failures the
  spectrometer is deemed "unresponsive" and excluded from the remainder of an
  iteration

Other options you can configure in a test include:

- min/max "read delay (ms)" -- an extra randomized pause, in milliseconds, from
  when the last-requested spectrum is presumably "ready" to be read-out from 
  the spectrometer to the PC, but we inject a little extra pause anyway, leaving
  the pixel intensities in the peripheral bulk endpoint output buffer just a little
  longer.  This is to test whether a slow laptop / spectroscopy application, which
  doesn't read-out new spectra as quickly as the spectrometer expects to send it, 
  can lead to communication errors.
- min/max "iteration delay (ms)" -- a randomized delay injected between "randomize
  integration time + read spectrum" loops on an individual spectrometer.  This has
  the effect of reducing the number of measurements done per spectrometer over the
  course of a test, while maintaining the overall test length (essentially, reducing
  load on the host PC and the spectrometer, while reducing USB bus contention).
- [x] throwaways -- if checked, automatically do a throwaway "stability acquisition"
  after each change in integration time 
- [x] serialize spectrometers -- not sure; I think it means, "only give one spectrometer
  access to the USB bus at a time", which would massively reduce parallelization and
  lead to far fewer measurements during the test
- [x] serialize reads -- not sure; I think it means, "only allow one spectrometer to
  be actively writing spectra over its bulk endpoint at any given time".  Since the
  uSb bus is Serial in any case, this is probably the default case, but technically
  a bulk write of 4096 bytes could well get "negotiated" down to several smaller writes
  of 512 or even 64 bytes, which could reasonably be multiplexed across spectrometers;
  this option tries to avoid that, and keep spectra in full-length "chunks".
- [x] has marker -- indicates that the spectrometer(s) under test have a firmware 
  which embeds a start-of-frame marker at the beginning of each spectrometer (rare;
  also, should be made an EEPROM FeatureMask flag if it ever goes into production)
- [x] track metrics -- not sure; think it adds additional output in the logfile
- extra reads -- this one is interesting.  There are many things you can read from
  a spectrometer other than spectra: detector temperature, the internal frame counter,
  the primary or secondary ADC, the battery charge level or charging state, detector
  gain and offset (even or odd), firmware and FPGA versions, high gain mode or laser
  state, etc.  There are actually 41 different settings that can be read at writing,
  and this option will randomly read _n_ of them after each acqusition (a different
  random _n_ each time).  So if you're looking to crash something, this would be a
  good way to test a lot of unique corner-cases.

# Command-Line Batch Operation

In order to support heavyweight, overnight / 3-day weekend testing, CrashTestNET
supports a command-line (DOS) interface, and comes with two (2) sample DOS Batch
scripts demonstrating how that command-line interface can be used.

## run-loop.bat

This script simply runs the CrashTestNET GUI _n_ number of times, launching the
application each time, running a full "test" of whatever configuration, exiting
the application, then starting all over again.  

This is useful if you want to include "application startup / shutdown" sequence
in your testing, for instance to determine if the spectrometer (or your driver
library) tends to throw errors when shutting down, or only after shutting down
and then re-spawning (or perhaps only after numerous shutdown / respawn events).

A sample execution of run-loop.bat from a DOS Cmd shell follows:

    16:36:16 Z:\work\code\CrashTestNET> run-loop 3
    Iteration 1
    Iteration 2
    Iteration 3
    7/28/20 4:37:50 PM Report: 2 spectrometers collected 329 measurements with 0 read failures and 0 shifts
    7/28/20 4:38:55 PM Report: 2 spectrometers collected 341 measurements with 0 read failures and 0 shifts
    7/28/20 4:39:59 PM Report: 2 spectrometers collected 330 measurements with 0 read failures and 0 shifts
    Done running CrashTestNET

    16:40:02 Z:\work\code\CrashTestNET>

Note a few things about that transcript: 

- A single argument was provided, the value "3".  This determined how many "loops"
  would be run.
- The program printed the number of each iteration, as it was run.  This makes it
  easy to check the screen and know "where the test is" in terms of overall 
  progress.
- At the END of execution, the run-loop.bat script prints a short report of the
  results of the test.  This report is literally a "grep" search of CrashTestNET.log,
  showing 

It is now worth looking at the source code of run-loop.bat itself:

- https://github.com/WasatchPhotonics/CrashTestNET/blob/master/run-loop.bat

If you're not familiar with batch file syntax, in short this is what it's doing:

- directing all CrashTestNET.exe output to CrashTestNET.log, which will be in the
  current directory (same directory as run-loop.bat)
- erasing that logfile if it already existed (so as to only capture new results)
- runs a "for loop" of 1..n iterations, where 'n' is the argument passed on the 
  command-line
- for each iteration, runs CrashTestNET with these settings (you can edit the
  script, or copy it to a new script, if you prefer different settings)
    - --start (automatically begin the test after launching the GUI)
    - --duration-sec 60 (each iteration should run for 60sec)
    - --integ-min 100 and --integ-max 500 (continually randomize integration time
      for each spectrometer in the range (100, 500ms)
    - --metrics I don't remember
    - --report (output a single report line at the end of each iteration)
- display lines matching "Report" from the logfile

## run-ragged.bat

This batch file is similar to run-loop.bat above, but with one big difference. 
If you execute run-loop.bat with the argument "3" and default settings, it will
run 3 full 60sec iterations of CrashTestNET through to completion, cleanly exiting 
and shutting down the USB connections at the end of each.  It will exit each
iteration _nicely._

Run-ragged.bat does not do that.  It starts each iteration in the same way, but 
then uses a Monte-Carlo draw to randomly terminate ('kill') each iteration at a
random point during execution:

- https://github.com/WasatchPhotonics/CrashTestNET/blob/master/run-ragged.bat

(The current script is set to run tests with 60sec iterations, killing each
at a random point between 10-60sec in).

This is very likely to corrupt the internal state of a spectrometer's firmware
(microcontroller or FPGA state machine), leading to errors on the subsequent
iteration's ability to startup and resume acquisitions normally.

This is the purpose of the test -- it is _designed_ to break the spectrometer
firmware, so do not be surprised if, when running it, you encounter errors. That
is the whole point.  (Stimulating such error conditions helps firmware engineers
better test and strengthen their code to make it harder to crash the hardware
under various conditions and timing corner-cases.)
