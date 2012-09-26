namespace REWEQ2EQPreset
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.radioReaEQ = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.radioFabfilterProQ = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(41, 106);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(203, 61);
			this.label1.TabIndex = 0;
			this.label1.Text = "Drag REW - Room EQ Wizard EQ Filter files here !";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(61, 158);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(168, 48);
			this.label2.TabIndex = 1;
			this.label2.Text = "Use either FBQ2496 or Generic Equaliser format. Remember to export filter as text" +
			"!";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(93, 7);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(100, 96);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// radioReaEQ
			// 
			this.radioReaEQ.Location = new System.Drawing.Point(12, 237);
			this.radioReaEQ.Name = "radioReaEQ";
			this.radioReaEQ.Size = new System.Drawing.Size(104, 24);
			this.radioReaEQ.TabIndex = 3;
			this.radioReaEQ.TabStop = true;
			this.radioReaEQ.Text = "ReaEQ";
			this.radioReaEQ.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 224);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 19);
			this.label3.TabIndex = 4;
			this.label3.Text = "Choose EQ Plugin:";
			// 
			// radioFabfilterProQ
			// 
			this.radioFabfilterProQ.Location = new System.Drawing.Point(102, 237);
			this.radioFabfilterProQ.Name = "radioFabfilterProQ";
			this.radioFabfilterProQ.Size = new System.Drawing.Size(104, 24);
			this.radioFabfilterProQ.TabIndex = 5;
			this.radioFabfilterProQ.TabStop = true;
			this.radioFabfilterProQ.Text = "FabFilter Pro-Q";
			this.radioFabfilterProQ.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.radioFabfilterProQ);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.radioReaEQ);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "MainForm";
			this.Text = "REW EQ to VST EQ Preset";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.RadioButton radioFabfilterProQ;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton radioReaEQ;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
	}
}
