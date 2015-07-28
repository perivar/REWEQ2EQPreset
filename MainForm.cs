using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

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
					Boolean success = false;
					
					REWEQFilters filters = REWEQ.ReadREWEQFiltersFile(inputFilePath);
					if (filters != null && filters.Count > 0) {
						string outputFilePath = directoryName + Path.DirectorySeparatorChar + fileName;
						
						switch(listBoxPluginSelection.SelectedIndex) {
							// I added some asserts to ensure switch cases corresponds to the right plugin
							case  0: // ReaEQ
								Debug.Assert(listBoxPluginSelection.SelectedItem.ToString().ToLower().Contains("reaeq"));
								outputFilePath += ".fxp";
								success = ReaEQ.Convert2ReaEQ(filters, outputFilePath);
								break;
							case  1: // EasyQ
								Debug.Assert(listBoxPluginSelection.SelectedItem.ToString().ToLower().Contains("easyq"));
								outputFilePath += ".xml";
								// success = EasyQ.Convert2EasyQ(filters, outputFilePath);
								break;
							case  2: // FabFilter Pro-Q
								Debug.Assert(listBoxPluginSelection.SelectedItem.ToString().ToLower().Contains("fabfilter"));
								outputFilePath += ".ffp";
								success = FabfilterProQ.Convert2FabfilterProQ(filters, outputFilePath);
								break;
							case -1: // No plugin selected
								MessageBox.Show(new Form() { WindowState = FormWindowState.Maximized, TopMost = true },
								                "Please select a plugin !",
												"Error",
												MessageBoxButtons.OK,
												MessageBoxIcon.Exclamation,
												MessageBoxDefaultButton.Button1);
								break;
							default:
								break;
						}
					}
					
					if(success) {
						MessageBox.Show(new Form() { WindowState = FormWindowState.Maximized, TopMost = true },
						                listBoxPluginSelection.SelectedItem.ToString() + " file generated (" + filters.Count + " filters)",
										"Success",
										MessageBoxButtons.OK,
										MessageBoxIcon.Information,
										MessageBoxDefaultButton.Button1);
					}
					else if(listBoxPluginSelection.SelectedIndex != -1) {
						MessageBox.Show(new Form() { WindowState = FormWindowState.Maximized, TopMost = true },
								                listBoxPluginSelection.SelectedItem.ToString() + " file not generated",
												"Error",
												MessageBoxButtons.OK,
												MessageBoxIcon.Error,
												MessageBoxDefaultButton.Button1);
								break;
					}
				}
			}
		}
	}
}
