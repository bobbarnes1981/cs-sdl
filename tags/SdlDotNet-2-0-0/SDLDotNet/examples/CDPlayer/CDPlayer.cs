/*
 * $RCSfile$
 * Copyright (C) 2003 Will Weisser (ogl@9mm.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Windows.Forms;
using SdlDotNet;

namespace SdlDotNet.Examples {
	public class CDPlayer : System.Windows.Forms.Form {
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
		//private System.Windows.Forms.Timer timer;
		//private System.ComponentModel.IContainer components;

		public CDPlayer() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_track = 0;
			//_drive = null;

			try {
				_cd = CDAudio.Instance;
				int num = _cd.NumberOfDrives;
				_drive = _cd.OpenDrive(0);
				for (int i = 0; i < num; i++)
					comboBoxDrive.Items.Add(_cd.DriveName(i));

				if (comboBoxDrive.Items.Count > 0) {
					comboBoxDrive.SelectedIndex = 0;
				//	timer.Start();
				}
			} catch (SdlException ex) {
				HandleError(ex);
			}
		}

		private void HandleError(SdlException ex) {
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
//				if (components != null) 
//				{
//					components.Dispose();
//				}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CDPlayer));
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxDrive = new System.Windows.Forms.ComboBox();
			this.buttonPlay = new System.Windows.Forms.Button();
			this.buttonPause = new System.Windows.Forms.Button();
			this.buttonStop = new System.Windows.Forms.Button();
			this.buttonEject = new System.Windows.Forms.Button();
			this.labelStatus = new System.Windows.Forms.Label();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonPrev = new System.Windows.Forms.Button();
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
			this.labelStatus.Click += new System.EventHandler(this.labelStatus_Click);
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
			// CDPlayer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(354, 134);
			this.Controls.Add(this.buttonPrev);
			this.Controls.Add(this.buttonNext);
			this.Controls.Add(this.labelStatus);
			this.Controls.Add(this.buttonEject);
			this.Controls.Add(this.buttonStop);
			this.Controls.Add(this.buttonPause);
			this.Controls.Add(this.buttonPlay);
			this.Controls.Add(this.comboBoxDrive);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CDPlayer";
			this.Text = "SDL.NET CD Player";
			this.Load += new System.EventHandler(this.CDPlayer_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new CDPlayer());
		}

		private void comboBoxDrive_SelectedIndexChanged(object sender, System.EventArgs e) {
			try {
//				if (_drive != null) {
//					_drive.Stop();
//					_drive.Close();
//				}

				//_drive = _cd.OpenDrive(comboBoxDrive.SelectedIndex);
				//_drive = _cd.OpenDrive(0);
			} catch (SdlException ex) {
				HandleError(ex);
			}
		}

		private void buttonPlay_Click(object sender, System.EventArgs e) {
			try {
				if (_drive != null)
					_drive.PlayTracks(_track, _drive.NumberOfTracks - _track);
			} catch (SdlException ex) {
				HandleError(ex);
			}
		}

		private void buttonPause_Click(object sender, System.EventArgs e) {
			try {
				if (_drive != null)
					_drive.Pause();
			} catch (SdlException ex) {
				HandleError(ex);
			}
		}

		private void buttonStop_Click(object sender, System.EventArgs e) {
			try {
				if (_drive != null) {
					_drive.Stop();
					_track = 0;
				}
			} catch (SdlException ex) {
				HandleError(ex);
			}
		}

		private void buttonEject_Click(object sender, System.EventArgs e) {
			try {
				if (_drive != null) {
					_drive.Eject();
					_track = 0;
				}
			} catch (SdlException ex) {
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
			} catch (SdlException ex) {
				HandleError(ex);
			}
		}

		private void buttonNext_Click(object sender, System.EventArgs e) {
			try {
				if (_drive != null) {
					if (_track != _drive.NumberOfTracks - 1)
						_track++;
					buttonPlay_Click(null, null);
				}
			} catch (SdlException ex) {
				HandleError(ex);
			}
		}

		private void CDPlayer_Load(object sender, System.EventArgs e)
		{
		
		}

		private void labelStatus_Click(object sender, System.EventArgs e)
		{
		
		}

//		private void timer_Tick(object sender, System.EventArgs e) {
//			try {
//				if (_drive != null) {
//					Tao.Sdl.Sdl.CDstatus status = _drive.Status;
//					StringBuilder statstr = new StringBuilder();
//					statstr.Append(status.ToString());
//					statstr.Append("\r\n");
//					if (status == Tao.Sdl.Sdl.CDstatus.CD_PLAYING) {
//						int min, sec, f;
//						CDAudio.FramesToMinSecFrames(_drive.CurrentFrame, out min, out sec, out f);
//						statstr.AppendFormat("{0:00} {1:00}:{2:00}", _drive.CurrentTrack, min, sec);
//					}
//					labelStatus.Text = statstr.ToString();
//				}
//			} catch (SdlException ex) {
//				HandleError(ex);
//				labelStatus.Text = "Error";
//			}
//		}
	}
}
