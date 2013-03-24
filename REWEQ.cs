using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;

namespace REWEQ2EQPreset
{
	/// <summary>
	/// Read and process REW EQ filter files
	/// </summary>
	public static class REWEQ
	{
		/// <summary>
		/// Parse a REW filters output file
		/// The users regional locale is also taken into consideration
		/// </summary>
		/// <param name="filePath">path to file</param>
		/// <returns>a rew eq filter object</returns>
		public static REWEQFilters ReadREWEQFiltersFile(string filePath) {
			
			REWEQFilters filters = new REWEQFilters();
			
			// Get current culture's NumberFormatInfo object.
			NumberFormatInfo nfi = CultureInfo.CurrentCulture.NumberFormat;
			//NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat; // For debugging

			// Assign needed property values to variables.
			string decimalSeparator = nfi.NumberDecimalSeparator;
			
			// Form regular expression pattern using the current culture's decimal seperator.
			Regex digitsAndDecimalSeparatorOnly = new Regex(@"[^\d" + Regex.Escape(decimalSeparator) + "]");
			
			using (StreamReader r = new StreamReader(filePath))
			{
				string line;
				int filterCount = 0;
				string regexpPattern = @"^Filter\s+\d+";
				bool usingBWOct = false;
				while ((line = r.ReadLine()) != null)
				{
					if (line.StartsWith("Equaliser:")) {
						// find out what filter parse rule to use
						if (line.Equals("Equaliser: FBQ2496")) {
							// Filter  1: ON  PEQ      Fc    64,0 Hz  Gain  -5,0 dB  BW Oct 0,167
							regexpPattern = @"^Filter\s+\d+:\s(\w+)\s+(\w+)\s+Fc ([\D\d" +
								Regex.Escape(decimalSeparator) + @"]+) Hz  Gain ([\s\d" +
								Regex.Escape(decimalSeparator) + @"\-]+) dB  BW Oct ([\s\d" +
								Regex.Escape(decimalSeparator) + @"]+)$";
							usingBWOct = true;
						} else if(line.Equals("Equaliser: Generic")) {
							// Filter  1: ON  PK       Fc    63,8 Hz  Gain  -5,0 dB  Q  8,06
							regexpPattern = @"^Filter\s+\d+:\s(\w+)\s+(\w+)\s+Fc ([\D\d" +
								Regex.Escape(decimalSeparator) + @"]+) Hz  Gain ([\s\d" +
								Regex.Escape(decimalSeparator) + @"\-]+) dB  Q ([\s\d" +
								Regex.Escape(decimalSeparator) + @"]+)$";
							usingBWOct = false;
						} else {
							Console.Error.WriteLine("No known equaliser format!", line);
							return null;
						}
					}
					
					// skip all lines that does not start with "Filter"
					if (line.StartsWith("Filter")) {
						
						// remove any non breaking spaces
						line = Regex.Replace(line, "\xA0", String.Empty);
						
						Match match = Regex.Match(line, regexpPattern);
						if (match.Success) {
							filterCount++;
							
							string enabled = match.Groups[1].Value.Trim();
							string type = match.Groups[2].Value.Trim();
							string freq = match.Groups[3].Value.Trim();
							freq = digitsAndDecimalSeparatorOnly.Replace(freq, "");
							string gain = match.Groups[4].Value.Trim();
							string q = match.Groups[5].Value.Trim();
							
							REWEQBand band = new REWEQBand();
							if (enabled.Equals("ON")) band.Enabled = true;
							if (type.Equals("PEQ") || type.Equals("PK")) band.FilterType = REWEQFilterType.PK;
							try {
								band.FilterFreq = Double.Parse(freq, nfi);
								band.FilterGain = Double.Parse(gain, nfi);
								
								if (usingBWOct) {
									band.FilterBWOct = Double.Parse(q, nfi);
									band.FilterQ = BWOct2Q(band.FilterBWOct);
								} else {
									band.FilterQ = Double.Parse(q, nfi);
									band.FilterBWOct = Q2BWOct(band.FilterQ);
								}
							} catch (Exception e) {
								Console.Error.WriteLine("Parse error", e.Message);
								return null;
							}
							filters.EqBands.Add(band);
						}
					}
				}
			}
			return filters;
		}
		
		
		public static double Q2BWOct(double Qin) {
			// y =(2*E26^2+1)/(2*E26^2)+SQRT(((((2*E26^2+1)/E26^2)^2)/4)-1)
			// =LOG10(y)/0,301
			double Q2bw1st = ((2*Qin*Qin)+1)/(2*Qin*Qin);
			double Q2bw2nd = Math.Pow(2*Q2bw1st,2)/4;
			double Q2bw3rd = Math.Sqrt(Q2bw2nd-1);
			double Q2bw4th = Q2bw1st+Q2bw3rd;
			return (Math.Log(Q2bw4th)/Math.Log(2));
		}
		
		public static double BWOct2Q(double q) {
			// =(SQRT(2^E18))/(2^E18-1)
			return (Math.Sqrt(Math.Pow(2,q)))/(Math.Pow(2, q)-1);
		}
	}

	
	public enum REWEQFilterType {
		PK = 0, // PK for a peaking (parametric) filter
		LP = 1, // LP for a 12dB/octave Low Pass filter (Q=0.7071)
		HP = 2, // HP for a 12dB/octave High Pass filter (Q=0.7071)
		LS = 3, // LS for a Low Shelf filter
		HS = 4, // HS for a High Shelf filter
		NO = 5, // NO for a notch filter
		MO = 6, // Modal for a Modal filter

		// The Generic and DCX2496 also have shelving filters implemented per the DCX2496
		LS6dB = 7, // LS 6dB for a 6dB/octave Low Shelf filter
		HS6dB = 8, // HS 6dB for a 6dB/octave High Shelf filter
		LS12dB = 9, // LS 12dB for a 12dB/octave Low Shelf filter
		HS12dB = 10, // HS 12dB for a 12dB/octave High Shelf filter

		// The Generic equaliser setting also has
		LPQ = 11, // LPQ, a 12dB/octave Low Pass filter with adjustable Q
		HPQ = 12, // HPQ, a 12dB/octave High Pass filter with adjustable Q
	}
	
	public class REWEQBand {
		public REWEQFilterType FilterType { get; set; }
		public bool Enabled { get; set; }
		public double FilterFreq { get; set; }		// Hz
		public double FilterGain { get; set; }		// dB
		public double FilterQ { get; set; }			// Q value
		public double FilterBWOct { get; set; }		// Bandwith per Octave
		
		public override string ToString() {
			return String.Format("{0}: {1:0.00} Hz  {2:0.00} dB  Q: {3:0.0000}  BWOct: {4:0.0000}", FilterType, FilterFreq, FilterGain, FilterQ, FilterBWOct);
		}
	}
	
	public class REWEQFilters {
		
		public List<REWEQBand> EqBands { get; set; }
		public int Count {
			get { return EqBands.Count; }
		}
		
		public REWEQFilters() {
			EqBands = new List<REWEQBand>();
		}
		
		public IEnumerator<REWEQBand> GetEnumerator() {
			return EqBands.GetEnumerator();
		}
	}
}
