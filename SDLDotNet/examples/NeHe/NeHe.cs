using System;
using System.Reflection;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// Summary description for NeHe.
	/// </summary>
	public class NeHe : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.TextBox lessonBox;
		private System.Windows.Forms.Label lessonLabel;
		private System.Windows.Forms.Label lessonDescriptions;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// 
		/// </summary>
		public NeHe()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NeHe));
			this.startButton = new System.Windows.Forms.Button();
			this.lessonBox = new System.Windows.Forms.TextBox();
			this.lessonLabel = new System.Windows.Forms.Label();
			this.lessonDescriptions = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(144, 432);
			this.startButton.Name = "startButton";
			this.startButton.TabIndex = 2;
			this.startButton.Text = "Start Demo";
			this.startButton.Click += new System.EventHandler(this.button1_Click);
			// 
			// lessonBox
			// 
			this.lessonBox.AcceptsTab = true;
			this.lessonBox.Location = new System.Drawing.Point(152, 392);
			this.lessonBox.Name = "lessonBox";
			this.lessonBox.Size = new System.Drawing.Size(72, 20);
			this.lessonBox.TabIndex = 1;
			this.lessonBox.Text = "";
			// 
			// lessonLabel
			// 
			this.lessonLabel.Location = new System.Drawing.Point(88, 392);
			this.lessonLabel.Name = "lessonLabel";
			this.lessonLabel.Size = new System.Drawing.Size(56, 24);
			this.lessonLabel.TabIndex = 0;
			this.lessonLabel.Text = "Lesson";
			// 
			// lessonDescriptions
			// 
			this.lessonDescriptions.Location = new System.Drawing.Point(0, 0);
			this.lessonDescriptions.Name = "lessonDescriptions";
			this.lessonDescriptions.Size = new System.Drawing.Size(392, 376);
			this.lessonDescriptions.TabIndex = 3;
			this.lessonDescriptions.Text = @"Lesson 1: Setting Up An OpenGL Window
Lesson 2: Your First Polygon
Lesson 3: Adding Color
Lesson 4: Rotation
Lesson 5: 3D Shapes
Lesson 6: Texture Mapping
Lesson 7: Texture Filters, Lighting, and Keyboard Control
Lesson 8: Blending
Lesson 9: Moving Bitmaps in 3D Space
Lesson 10: Loading and Moving through a 3D World
Lesson 11: Flag Effect (Waving Texture)
Lesson 12: Display Lists
Lesson 13: Bitmap Fonts
Lesson 16: Cool Looking Fog
Lesson 17: 2D Texture Font
Lesson 18: Quadrics
Lesson 19: Particle Engine Using Triangle Strips
Lesson 20: Masking
Lesson 23: Sphere Mapping, Multi-Texturing and Extensions";
			// 
			// NeHe
			// 
			this.AcceptButton = this.startButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 473);
			this.Controls.Add(this.lessonDescriptions);
			this.Controls.Add(this.lessonLabel);
			this.Controls.Add(this.lessonBox);
			this.Controls.Add(this.startButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "NeHe";
			this.Text = "SDL.NET - NeHe OpenGL Examples";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		[STAThread]
		public static void Main(string[] args)
		{
			Application.Run(new NeHe());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			try
			{
				string tempLesson = this.lessonBox.Text.Trim();
				this.lesson = padding.Substring(tempLesson.Length - 1) + this.lessonBox.Text.Trim();
				Type dynClassType = asm.GetType("SdlDotNet.Examples.NeHe" + lesson, true, false);
				object dynObj = Activator.CreateInstance(dynClassType);
				if (dynObj != null) 
				{
					// Verify that the method exists and get its MethodInfo obj
					MethodInfo invokedMethod = dynClassType.GetMethod("Run");
					invokedMethod.Invoke(dynObj, null);
				}
			}
			catch (System.TypeLoadException)
			{
				MessageBox.Show("Lesson does not exist");
			}
		}

		private string lesson;
		private string padding = "00";
	}
}
