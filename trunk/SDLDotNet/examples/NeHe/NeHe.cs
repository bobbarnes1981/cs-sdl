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
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
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
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(144, 432);
			this.button1.Name = "button1";
			this.button1.TabIndex = 0;
			this.button1.Text = "Start Demo";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(152, 392);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(72, 20);
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "";
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(88, 392);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 24);
			this.label1.TabIndex = 2;
			this.label1.Text = "Lesson";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(392, 376);
			this.label2.TabIndex = 3;
			this.label2.Text = 
				NeHe001.Title + "\r\n" + 
				NeHe002.Title + "\r\n" + 
				NeHe003.Title + "\r\n" + 
				NeHe004.Title + "\r\n" + 
				NeHe005.Title + "\r\n" + 
				NeHe006.Title + "\r\n" + 
				NeHe007.Title + "\r\n" + 
				NeHe008.Title + "\r\n" + 
				NeHe009.Title + "\r\n" + 
				NeHe010.Title + "\r\n" + 
				NeHe011.Title + "\r\n" + 
				NeHe012.Title + "\r\n" + 
				NeHe013.Title + "\r\n" + 
				NeHe016.Title + "\r\n" + 
				NeHe017.Title + "\r\n" + 
				NeHe018.Title + "\r\n" + 
				NeHe019.Title + "\r\n" + 
				NeHe020.Title + "\r\n" + 
				NeHe023.Title + "\r\n";
			// 
			// NeHe
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 473);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
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

		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			string tempLesson = textBox1.Text.Trim();
			this.lesson = padding.Substring(tempLesson.Length - 1) + textBox1.Text.Trim();
		}
	}
}
