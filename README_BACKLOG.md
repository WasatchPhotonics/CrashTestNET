# Backlog

There are literally hundreds of things we could add, and most of them I think
exceed the envisioned remit of this app.  That said...

## Spectral Analysis

The program needs some way of telling if spectra gets "corrupted" (walks, shifts,
flatlines, saturates, whatever).  

Of course, who knows what "correct" spectra looks like for a given test: 

- is it room lights? what do they look like from a 785 vs a NIR1 vs a VISNIR?
- is it darks, with the SMA cap on?
- is it an Xe, or Ar, or Hg-1, or LS-1, and again, what does that look like from different models?
- ...all noting that integration time is randomly changing

One option would be to have a human, at the beginning of a long test run, manually
sample and save a few representative spectra of what each measurement SHOULD look
like (perhaps at the first, middle, and last configured integration time), and let
the program "interpolate" toward an expected match.  Or perhaps just configure 2-3
peaks which should either be found or saturated?  Dunno.

## Spectrometer Features

Right now, the program doesn't do much (anything) with the TEC or laser, because the
original test article (an ambient VISNIR) had neither.

It could be good to include randomly changing the TEC setpoint (within the EEPROM
range), not EVERY cycle, but perhaps every 60sec, and confirm temperature "grossly
converged" toward it.

It could be good to randomly set laser power (by percentage, or mW if calibrated) and 
confirm that, for a given integration time, intensities increased or decreased with
changes in laser power.
