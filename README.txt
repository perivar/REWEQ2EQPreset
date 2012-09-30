Room EQ Wizard EQ Filter to VST EQ Plugin Preset Converter README
=================================================================
Per Ivar Nerseth, 2012
perivar@nerseth.com

A very detailed description of the process can be seen here: 
http://www.youtube.com/watch?v=Fa9qlB6LK4c
Created by Dozerbeatz (dozerbeatz@gmail.com)

The only exception from the above video is that I don't use apQualizr but eiter ReaEQ (free) or FabFilter Pro-Q (commercial) EQ plugin.


A short summary of the process is:

1. Calibrate the SPL Meter

2. Make a measurement

3. Choose Graph: "Apply Smoothing 1/24 Octave Smoothing"

4. Click big "EQ" Button

5. Setup a good EQ View 

Configure EQ View (Click the "Config" wheel): 
- "Smooth 1/24"
- Check "Fill Filter responses" 
- The rest should be unchecked

6. Choose the following settings before generating the EQ Filters: 

Equaliser: 			Generic

Target Settings:
Speaker Type: 		Full Range
LF Slope: 			24dB/Octave
LF Cutoff (Hz): 	10
Target Level (dB): 	75.0 (or 80)

Filter Tasks:
Match Range: 			20 - 10.000 Hz
Individual Max Boost: 	12 dB
Overall Max Boost: 		9 dB
Flatness Target: 		1

7. Click "Match Response to Target"
This will create an inverse eq filter which will be the basis for our VST EQ Plugin preset file.

8. Click the "EQ Filters" view and then click the double arrow to sort the eq filters from low to high

9. Choose Export: "Filter Settings as Text" and store the txt file a place you remember

10. Run the "REW2EQPreset" tool and select the EQ plugin of choice to generate a preset for, e.g. "FabFilter Pro-Q"

If using FabFilter Pro-Q then the generated preset file (*.ffp) must be copied to the FabFilter EQ directory for the plugin to find it. On my computer this is "Documents\FabFilter\Pro-Q"

11. Generate a "PinkPN" signal (Wave) to use in your DAW.
Use the "Generator" but keep "65536" as length

12. Setup this signal to play through you DAW and insert the chosen EQ Plugin after the signal.

13. Setup the Real Time Analyser (RTA) in REW to verify that everything is working as it should
(http://www.hometheatershack.com/roomeq/wizardhelpv5/help_en-GB/html/spectrum.html#top)

Choose: dB (Not dB Full Scale)

Click the Configure button:
Mode: 			RTA 1/24 octave
FFT Length:		65536
Averages:		Exponential 0.94
Window: 		Rectangular
Max Overlap:	93.75%
Update Interv: 	1
(No checkboxes checked)

The FFT resolution is also affected by the Window settings.
Rectangular windows give the best frequency resolution but are only suitable when the signal being analysed is periodic within the FFT length or if a noise signal is being measured. 
The Rectangular window should always be used with the REW periodic noise signals. In other words, when using the Pink Noise PN signal as I am, set your "Window" to "Rectangular"
