using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace REWEQ2EQPreset
{
	/// <summary>
	/// Convert Room EQ Wizard EQ (REW) Filters to VST EQ Preset files
	/// e.g. ReaEQ preset file
	/// http://www.hometheatershack.com/roomeq/
	/// http://www.reaper.fm/reaplugs/
	/// </summary>
	public partial class MainForm : Form
	{
		static string _version = "1.1.3";
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			labelVersion.Text = "Version: " + _version;
			
			this.AllowDrop = true;
			this.DragEnter += new DragEventHandler(MainForm_DragEnter);
			this.DragDrop += new DragEventHandler(MainForm_DragDrop);
		}
		
		void MainForm_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
		}

		void MainForm_DragDrop(object sender, DragEventArgs e) {
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string inputFilePath in files) {
				string fileExtension = Path.GetExtension(inputFilePath);
				string directoryName = Path.GetDirectoryName(inputFilePath);
				string fileName = Path.GetFileNameWithoutExtension(inputFilePath);
				if (fileExtension.Equals(".txt")) {
					REWEQFilters filters = REWEQ.ReadREWEQFiltersFile(inputFilePath);
					if (filters != null && filters.Count > 0) {
						if (radioFabfilterProQ.Checked) {
							string outputFilePath = directoryName + Path.DirectorySeparatorChar + fileName + ".ffp";
							FabfilterProQ.Convert2FabfilterProQ(filters, outputFilePath);
						} else if (radioReaEQ.Checked) {
							string outputFilePath = directoryName + Path.DirectorySeparatorChar + fileName + ".fxp";
							ReaEQ.Convert2ReaEQ(filters, outputFilePath);
						}
					}
				}
			}
		}
	}
}
