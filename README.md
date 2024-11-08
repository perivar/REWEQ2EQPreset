# Room EQ Wizard EQ Filter to VST EQ Plugin Preset Converter

**⚠️ Deprecated Repository**  
This repository is deprecated. The updated code is now available as a web application and can be found here:  
[Room EQ Wizard to VST EQ Preset Web App](https://github.com/perivar/reweq2eqpreset-cloudflare).

---

## Overview

**Author:** Per Ivar Nerseth, 2013  
**Contact:** perivar@nerseth.com  

For a detailed walkthrough, watch this video:  
[Room EQ Wizard to VST EQ Plugin Preset Conversion](http://www.youtube.com/watch?v=Fa9qlB6LK4c)  
Created by Dozerbeatz (dozerbeatz@gmail.com).

### Supported EQ Plugins
This tool supports the following EQ plugins instead of apQualizr:
- **ReaEQ** (Free): [Download ReaEQ](http://www.reaper.fm/reaplugs/)
- **FabFilter Pro-Q** (Commercial): [Download Pro-Q](http://www.fabfilter.com/products/pro-q-equalizer-plug-in)

---

## Summary of the Process

1. **Calibrate the SPL Meter**  
2. **Make a Measurement**  
3. **Apply Smoothing**  
   - Choose Graph: *Apply Smoothing 1/24 Octave Smoothing*  
4. **Click the big "EQ" Button**  
5. **Setup a Good EQ View**  
   - Configure EQ View (Click the "Config" wheel):  
     - *Smooth 1/24*  
     - Check *Fill Filter responses*  
     - Uncheck the rest  

6. **Set the Following Before Generating EQ Filters**:  
   - **Equaliser**: Generic  
   - **Target Settings**:  
     - Speaker Type: *Full Range*  
     - LF Slope: *24 dB/Octave*  
     - LF Cutoff (Hz): *10*  
     - Target Level (dB): *75.0* (or *80*)  
   - **Filter Tasks**:  
     - Match Range: *20 - 10,000 Hz*  
     - Individual Max Boost: *12 dB*  
     - Overall Max Boost: *9 dB*  
     - Flatness Target: *1*  

7. **Match Response to Target**  
   - This creates an inverse EQ filter for the VST EQ plugin preset.  
8. **Export the EQ Filters**  
   - Go to "EQ Filters" view → Click the double arrow to sort filters low-to-high → Export as *Filter Settings as Text*.  

9. **Run "REW2EQPreset" Tool**  
   - Use the tool to generate a preset for your chosen EQ plugin, e.g., *FabFilter Pro-Q*.  
   - **Note:** The tool automatically uses the correct decimal separator based on your regional settings.  

10. **For FabFilter Pro-Q Users**  
    - Copy the generated preset file (*.ffp) to the FabFilter EQ directory:  
      `Documents\FabFilter\Pro-Q`  

11. **Generate a "PinkPN" Signal**  
    - Use the "Generator" and keep *65536* as length.  

12. **Test in Your DAW**  
    - Play the signal in your DAW with the EQ plugin inserted.  

13. **Verify Using Real Time Analyser (RTA) in REW**  
    - [REW RTA Setup Guide](http://www.hometheatershack.com/roomeq/wizardhelpv5/help_en-GB/html/spectrum.html#top)  
    - Choose *dB (Not dB Full Scale)*  

### RTA Configuration:
- **Mode**: RTA 1/24 octave  
- **FFT Length**: 65536  
- **Averages**: Exponential 0.94  
- **Window**: Rectangular  
- **Max Overlap**: 93.75%  
- **Update Interval**: 1  
- *No checkboxes checked*  

> **Note:** Use *Rectangular* window for the best frequency resolution, especially with periodic noise signals like *Pink Noise PN*.

---
