using System;
using System.Collections.Generic;
using System.IO;
using CommonUtils;

namespace REWEQ2EQPreset
{
	/// <summary>
	/// FabfilterProQ Preset Class for saving a Fabfilter ProQ Preset file (fft)
	/// </summary>
	public static class FabfilterProQ
	{
		public static bool Convert2FabfilterProQ(REWEQFilters filters, string filePath) {
			List<ProQBand> proQBands = new List<ProQBand>();
			foreach (REWEQBand filter in filters) {
				ProQBand band = new ProQBand();
				band.FilterFreq = filter.FilterFreq;
				band.FilterGain = filter.FilterGain;
				band.FilterQ = filter.FilterQ;
				band.Enabled = filter.Enabled;
				switch (filter.FilterType) {
					case REWEQFilterType.PK:
						band.FilterType = ProQFilterType.Bell;
						break;
					case REWEQFilterType.LP:
						band.FilterType = ProQFilterType.HighCut;
						break;
					case REWEQFilterType.HP:
						band.FilterType = ProQFilterType.LowCut;
						break;
					case REWEQFilterType.LS:
						band.FilterType = ProQFilterType.LowShelf;
						break;
					case REWEQFilterType.HS:
						band.FilterType = ProQFilterType.HighShelf;
						break;
					default:
						band.FilterType = ProQFilterType.Bell;
						break;
				}
				band.FilterLPHPSlope = ProQLPHPSlope.Slope24dB_oct;
				band.FilterStereoPlacement = ProQStereoPlacement.Stereo;
				
				proQBands.Add(band);
			}
			
			BinaryFile binFile = new BinaryFile(filePath, BinaryFile.ByteOrder.LittleEndian, true);
			binFile.Write("FPQr");
			binFile.Write((int) 2);
			binFile.Write((int) 180);
			binFile.Write((float) proQBands.Count);
			
			for (int i = 0; i < 24; i++) {
				if (i < proQBands.Count) {
					binFile.Write((float) FreqConvert(proQBands[i].FilterFreq));
					binFile.Write((float) proQBands[i].FilterGain);
					binFile.Write((float) QConvert(proQBands[i].FilterQ));
					binFile.Write((float) proQBands[i].FilterType);
					binFile.Write((float) proQBands[i].FilterLPHPSlope);
					binFile.Write((float) proQBands[i].FilterStereoPlacement);
					binFile.Write((float) (proQBands[i].Enabled ? 1 : 0) );
				} else {
					binFile.Write((float) FreqConvert(1000));
					binFile.Write((float) 0);
					binFile.Write((float) QConvert(1));
					binFile.Write((float) ProQFilterType.Bell);
					binFile.Write((float) ProQLPHPSlope.Slope24dB_oct);
					binFile.Write((float) ProQStereoPlacement.Stereo);
					binFile.Write((float) 1);
				}
			}

			binFile.Write((float) 0); // float output_gain;      // -1 to 1 (- Infinity to +36 dB , 0 = 0 dB)
			binFile.Write((float) 0); // float output_pan;       // -1 to 1 (0 = middle)
			binFile.Write((float) 2); // float display_range;    // 0 = 6dB, 1 = 12dB, 2 = 30dB, 3 = 3dB
			binFile.Write((float) 0); // float process_mode;     // 0 = zero latency, 1 = lin.phase.low - medium - high - maximum
			binFile.Write((float) 0); // float channel_mode;     // 0 = Left/Right, 1 = Mid/Side
			binFile.Write((float) 0); // float bypass;           // 0 = No bypass
			binFile.Write((float) 0); // float receive_midi;     // 0 = Enabled?
			binFile.Write((float) 3); // float analyzer;         // 0 = Off, 1 = Pre, 2 = Post, 3 = Pre+Post
			binFile.Write((float) 1); // float analyzer_resolution;  // 0 - 3 : low - medium[x] - high - maximum
			binFile.Write((float) 2); // float analyzer_speed;   // 0 - 3 : very slow, slow, medium[x], fast
			binFile.Write((float) -1); // float solo_band;        // -1
			binFile.Close();
			
			return true;
		}
		
		public static double FreqConvert(double value) {
			// =LOG(A1)/LOG(2) (default = 1000 Hz)
			return Math.Log10(value)/ Math.Log10(2);
		}
		
		public static double QConvert(double value) {
			// =LOG(F1)*0,312098175+0,5 (default = 1)
			return Math.Log10(value) * 0.312098175 + 0.5;
		}
		
	}
	
	public enum ProQFilterType {
		Bell        = 0, // (default)
		LowShelf    = 1,
		LowCut      = 2,
		HighShelf   = 3,
		HighCut     = 4,
		Notch       = 5,
	}

	public enum ProQLPHPSlope {
		Slope6dB_oct	= 0,
		Slope12dB_oct   = 1,
		Slope24dB_oct   = 2, // (default)
		Slope48dB_oct   = 3,
	}

	public enum ProQStereoPlacement {
		Left        = 0,
		Right       = 1,
		Stereo      = 2, // (default)
	}

	public class ProQBand {
		public ProQFilterType FilterType { get; set; }
		public ProQLPHPSlope FilterLPHPSlope { get; set; }
		public ProQStereoPlacement FilterStereoPlacement { get; set; }
		public bool Enabled { get; set; }
		public double FilterFreq { get; set; }		// value range 10.0 -> 30000.0 Hz
		public double FilterGain { get; set; }		// + or - value in dB
		public double FilterQ { get; set; }			// value range 0.025 -> 40.00
		
		public override string ToString() {
			return String.Format("{0}: {1} Hz  {2} dB  Q: {3}", FilterType, FilterFreq, FilterGain, FilterQ);
		}
	}
}
