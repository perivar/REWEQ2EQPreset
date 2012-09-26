using System;
using System.Collections.Generic;
using System.IO;
using CommonUtils;

namespace REWEQ2EQPreset
{
	/// <summary>
	/// ReaEQ Preset Class for saving a ReaEq Preset file (fxp)
	/// </summary>
	public static class ReaEQ
	{
		public static bool Convert2ReaEQ(REWEQFilters filters, string filePath) {
			
			List<ReaEQBand> ReaEqBands = new List<ReaEQBand>();
			foreach (REWEQBand filter in filters) {
				ReaEQBand band = new ReaEQBand();
				band.LogScaleAutoFreq = true;
				band.FilterFreq = filter.FilterFreq;
				band.FilterGain = filter.FilterGain;
				band.FilterBWOct = filter.FilterBWOct;
				band.Enabled = filter.Enabled;
				switch (filter.FilterType) {
					case REWEQFilterType.PK:
						band.FilterType = ReaEQFilterType.Band;
						break;
					case REWEQFilterType.LP:
						band.FilterType = ReaEQFilterType.LowPass;
						break;
					case REWEQFilterType.HP:
						band.FilterType = ReaEQFilterType.HighPass;
						break;
					case REWEQFilterType.LS:
						band.FilterType = ReaEQFilterType.LowShelf;
						break;
					case REWEQFilterType.HS:
						band.FilterType = ReaEQFilterType.HighShelf;
						break;
					default:
						band.FilterType = ReaEQFilterType.Band;
						break;
				}
				ReaEqBands.Add(band);
			}
			
			// store to file
			FXP fxp = new FXP();
			fxp.chunkMagic = "CcnK";
			fxp.byteSize = 0; // will be set correctly by FXP class
			fxp.fxMagic = "FPCh"; // FPCh = FXP (preset), FBCh = FXB (bank)
			fxp.version = 1; // Format Version (should be 1)
			fxp.fxID = "reeq";
			fxp.fxVersion = 1100;
			fxp.numPrograms = 1;
			fxp.name = "";
			
			using(MemoryStream memStream = new MemoryStream(10))
			{
				BinaryFile binFile = new BinaryFile(memStream, BinaryFile.ByteOrder.LittleEndian);
				binFile.Write((int)33);
				binFile.Write((int)ReaEqBands.Count);
				foreach (ReaEQBand band in ReaEqBands) {
					binFile.Write((int) band.FilterType);
					binFile.Write((int) (band.Enabled ? 1 : 0) );
					binFile.Write((double) band.FilterFreq);
					binFile.Write((double) Decibel2AmplitudeRatio(band.FilterGain));
					binFile.Write((double) band.FilterBWOct);
					binFile.Write((byte) 1);
				}
				
				binFile.Write((int)1);
				binFile.Write((int)1);
				
				binFile.Write((double) Decibel2AmplitudeRatio(0.00));
				binFile.Write((int)0);
				
				memStream.Flush();
				byte[] chunkData = memStream.GetBuffer();
				fxp.chunkSize = chunkData.Length;
				fxp.chunkDataByteArray = chunkData;
			}
			fxp.WriteFile(filePath);
			return true;
		}
		
		
		// Amplitude ratio to dB conversion
		// For amplitude of waves like voltage, current and sound pressure level:
		// GdB = 20 * log10(A2 / A1)
		// A2 is the amplitude level.
		// A1 is the referenced amplitude level.
		// GdB is the amplitude ratio or gain in dB.
		public static double AmplitudeRatio2Decibel(double value) {
			return 20 * Math.Log10(value);
		}
		
		// dB to amplitude ratio conversion
		// A2 = A1 * 10^(GdB / 20)
		// A2 is the amplitude level.
		// A1 is the referenced amplitude level.
		public static double Decibel2AmplitudeRatio(double value) {
			return Math.Pow(10, value / 20);
		}
		
	}

	public enum ReaEQFilterType {
		LowShelf = 0,
		HighShelf = 1,
		Band = 8,
		LowPass = 3,
		HighPass = 4,
		AllPass = 5,
		Notch = 6,
		BandPass = 7,
		Band_alt = 9,
		Band_alt2 = 2,
	}

	public class ReaEQBand {
		public ReaEQFilterType FilterType { get; set; }
		public bool Enabled { get; set; }
		public double FilterFreq { get; set; }		// value range 20.0 -> 24000.0 Hz
		public double FilterGain { get; set; }		// value range -90.0 (inf) -> 24.0) dB. Store the inverse=10^(dBVal/20)
		public double FilterBWOct { get; set; }		// value range 0.01 -> 4.00
		public bool LogScaleAutoFreq { get; set; }
		
		public override string ToString() {
			return String.Format("{0}: {1} Hz  {2} dB  BWOct: {3}", FilterType, FilterFreq, FilterGain, FilterBWOct);
		}
	}
}
