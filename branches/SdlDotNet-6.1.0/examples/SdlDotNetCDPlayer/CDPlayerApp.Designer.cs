#region LICENSE
/*
 * $RCSfile: CDPlayer.cs,v $
 * Copyright (C) 2003 Will Weisser (ogl@9mm.com)
 * Copyright (C) 2004,2005 David Hudson (jendave@yahoo.com)
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
#endregion LICENSE

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Globalization;

using SdlDotNet.Audio;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Windows;

namespace SdlDotNetExamples.CDPlayer
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CDPlayerApp : System.Windows.Forms.Form
    {
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CDPlayerApp));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxDrive = new System.Windows.Forms.ComboBox();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonEject = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.surfaceControl = new SdlDotNet.Windows.SurfaceControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.surfaceControl)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 24);
            this.label1.TabIndex = 0;
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
            this.buttonPlay.Location = new System.Drawing.Point(8, 88);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(48, 40);
            this.buttonPlay.TabIndex = 2;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.Location = new System.Drawing.Point(62, 88);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(48, 40);
            this.buttonPause.TabIndex = 3;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(116, 88);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(48, 40);
            this.buttonStop.TabIndex = 4;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonEject
            // 
            this.buttonEject.Location = new System.Drawing.Point(170, 88);
            this.buttonEject.Name = "buttonEject";
            this.buttonEject.Size = new System.Drawing.Size(48, 40);
            this.buttonEject.TabIndex = 5;
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
            this.buttonNext.Location = new System.Drawing.Point(286, 88);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(58, 40);
            this.buttonNext.TabIndex = 7;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Location = new System.Drawing.Point(224, 88);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(56, 40);
            this.buttonPrevious.TabIndex = 8;
            this.buttonPrevious.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // surfaceControl
            // 
            this.surfaceControl.AccessibleDescription = "SdlDotNet SurfaceControl";
            this.surfaceControl.AccessibleName = "SurfaceControl";
            this.surfaceControl.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic;
            this.surfaceControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.surfaceControl.Location = new System.Drawing.Point(0, 0);
            this.surfaceControl.Name = "surfaceControl";
            this.surfaceControl.Size = new System.Drawing.Size(348, 226);
            this.surfaceControl.TabIndex = 0;
            this.surfaceControl.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMinSize = new System.Drawing.Size(250, 175);
            this.panel1.Controls.Add(this.surfaceControl);
            this.panel1.Location = new System.Drawing.Point(4, 134);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(348, 226);
            this.panel1.TabIndex = 9;
            // 
            // CDPlayerApp
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(362, 367);
            this.Controls.Add(this.buttonPrevious);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonEject);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.comboBoxDrive);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CDPlayerApp";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.CDPlayer_Closing);
            this.Load += new System.EventHandler(this.CDPlayer_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.surfaceControl)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxDrive;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonEject;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonPrevious;
        private SdlDotNet.Windows.SurfaceControl surfaceControl;
        private System.Windows.Forms.Button buttonNext;

        private Panel panel1;
    }
}
