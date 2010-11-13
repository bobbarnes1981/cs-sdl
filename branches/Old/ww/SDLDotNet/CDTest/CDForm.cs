using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using SDLDotNet;

namespace CDTest {
	public class CDForm : System.Windows.Forms.Form {
		private CDAudio _cd;
		private CDDrive _drive;
		private int _track;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBoxDrive;
		private System.Windows.Forms.Button buttonPlay;
		private System.Windows.Forms.Button buttonPause;
		private System.Windows.Forms.Button buttonStop;
		private System.Windows.Forms.Button buttonEject;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Button buttonPrev;
		private System.Windows.Forms.Timer timer;
		private System.ComponentModel.IContainer components;

		public CDForm() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_track = 0;
			_drive = null;

			try {
				_cd = (new SDL(false)).CDAudio;
				int num = _cd.NumDrives;
				for (int i = 0; i < num; i++)
					comboBoxDrive.Items.Add(_cd.DriveName(i));

				if (comboBoxDrive.Items.Count > 0) {
					comboBoxDrive.SelectedIndex = 0;
					timer.Start();
				}
			} catch (SDLException ex) {
				HandleError(ex);
			}
		}

		private void HandleError(SDLException ex) {
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxDrive = new System.Windows.Forms.ComboBox();
			this.buttonPlay = new System.Windows.Forms.Button();
			this.buttonPause = new System.Windows.Forms.Button();
			this.buttonStop = new System.Windows.Forms.Button();
			this.buttonEject = new System.Windows.Forms.Button();
			this.labelStatus = new System.Windows.Forms.Label();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonPrev = new System.Windows.Forms.Button();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select Drive:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBoxDrive
			// 
			this.comboBoxDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxDrive.Location = new System.Drawing.Point(96, 8);
			this.comboBoxDrive.Name = "comboBoxDrive";
			this.comboBoxDrive.Size = new System.Drawing.Size(112, 21);
			this.comboBoxDrive.TabIndex = 1;
			this.comboBoxDrive.SelectedIndexChanged += new System.EventHandler(this.comboBoxDrive_SelectedIndexChanged);
			// 
			// buttonPlay
			// 
			this.buttonPlay.Location = new System.Drawing.Point(16, 88);
			this.buttonPlay.Name = "buttonPlay";
			this.buttonPlay.Size = new System.Drawing.Size(48, 40);
			this.buttonPlay.TabIndex = 2;
			this.buttonPlay.Text = "Play";
			this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
			// 
			// buttonPause
			// 
			this.buttonPause.Location = new System.Drawing.Point(72, 88);
			this.buttonPause.Name = "buttonPause";
			this.buttonPause.Size = new System.Drawing.Size(48, 40);
			this.buttonPause.TabIndex = 3;
			this.buttonPause.Text = "Pause";
			this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
			// 
			// buttonStop
			// 
			this.buttonStop.Location = new System.Drawing.Point(128, 88);
			this.buttonStop.Name = "buttonStop";
			this.buttonStop.Size = new System.Drawing.Size(48, 40);
			this.buttonStop.TabIndex = 4;
			this.buttonStop.Text = "Stop";
			this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
			// 
			// buttonEject
			// 
			this.buttonEject.Location = new System.Drawing.Point(184, 88);
			this.buttonEject.Name = "buttonEject";
			this.buttonEject.Size = new System.Drawing.Size(48, 40);
			this.buttonEject.TabIndex = 5;
			this.buttonEject.Text = "Eject";
			this.buttonEject.Click += new System.EventHandler(this.buttonEject_Click);
			// 
			// labelStatus
			// 
			this.labelStatus.Location = new System.Drawing.Point(16, 40);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(328, 40);
			this.labelStatus.TabIndex = 6;
			// 
			// buttonNext
			// 
			this.buttonNext.Location = new System.Drawing.Point(296, 88);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(48, 40);
			this.buttonNext.TabIndex = 7;
			this.buttonNext.Text = "Next";
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonPrev
			// 
			this.buttonPrev.Location = new System.Drawing.Point(240, 88);
			this.buttonPrev.Name = "buttonPrev";
			this.buttonPrev.Size = new System.Drawing.Size(48, 40);
			this.buttonPrev.TabIndex = 8;
			this.buttonPrev.Text = "Prev";
			this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
			// 
			// timer
			// 
			this.timer.Interval = 1000;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// CDForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(354, 134);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonPrev,
																		  this.buttonNext,
																		  this.labelStatus,
																		  this.buttonEject,
																		  this.buttonStop,
																		  this.buttonPause,
																		  this.buttonPlay,
																		  this.comboBoxDrive,
																		  this.label1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CDForm";
			this.Text = "Ghetto CD Player";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new CDForm());
		}

		private void comboBoxDrive_SelectedIndexChanged(object sender, System.EventArgs e) {
			try {
				if (_drive != null) {
					_drive.Stop();
					_drive.Close();
				}
				_drive = _cd.OpenDrive(comboBoxDrive.SelectedIndex);
			} catch (SDLException ex) {
				HandleError(ex);
			}
		}

		private void buttonPlay_Click(object sender, System.EventArgs e) {
			try {
				if (_drive != null)
					_drive.PlayTracks(_track, _drive.NumTracks - _track);
			} catch (SDLException ex) {
				HandleError(ex);
			}
		}

		private void buttonPause_Click(object sender, System.EventArgs e) {
			try {
				if (_drive != null)
					_drive.Pause();
			} catch (SDLException ex) {
				HandleError(ex);
			}
		}

		private void buttonStop_Click(object sender, System.EventArgs e) {
			try {
				if (_drive != null) {
					_drive.Stop();
					_track = 0;
				}
			} catch (SDLException ex) {
				HandleError(ex);
			}
		}

		private void buttonEject_Click(object sender, System.EventArgs e) {
			try {
				if (_drive != null) {
					_drive.Eject();
					_track = 0;
				}
			} catch (SDLException ex) {
				HandleError(ex);
			}
		}

		private void buttonPrev_Click(object sender, System.EventArgs e) {
			try {
				if (_drive != null) {
					if (_track != 0)
						_track--;
					buttonPlay_Click(null, null);
				}
			} catch (SDLException ex) {
				HandleError(ex);
			}
		}

		private void buttonNext_Click(object sender, System.EventArgs e) {
			try {
				if (_drive != null) {
					if (_track != _drive.NumTracks - 1)
						_track++;
					buttonPlay_Click(null, null);
				}
			} catch (SDLException ex) {
				HandleError(ex);
			}
		}

		private void timer_Tick(object sender, System.EventArgs e) {
			try {
				if (_drive != null) {
					CDStatus status = _drive.Status;
					StringBuilder statstr = new StringBuilder();
					statstr.Append(status.ToString());
					statstr.Append("\r\n");
					if (status == CDStatus.Playing) {
						int min, sec, f;
						CDAudio.FramesToMinSecFrames(_drive.CurrentFrame, out min, out sec, out f);
						statstr.AppendFormat("{0:00} {1:00}:{2:00}", _drive.CurrentTrack, min, sec);
					}
					labelStatus.Text = statstr.ToString();
				}
			} catch (SDLException ex) {
				HandleError(ex);
				labelStatus.Text = "Error";
			}
		}
	}
}
