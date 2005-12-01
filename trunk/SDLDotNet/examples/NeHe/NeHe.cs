using System;
using System.Drawing;
using System.Collections;
using System.Reflection;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// Summary description for NeHe.
	/// </summary>
	public class NeHe : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox lstExamples;
		private System.Windows.Forms.Button startButton;
		private System.Collections.ArrayList neheTypes = new ArrayList();
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
			this.lstExamples = new System.Windows.Forms.ListBox();
			this.startButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstExamples
			// 
			this.lstExamples.Location = new System.Drawing.Point(8, 8);
			this.lstExamples.Name = "lstExamples";
			this.lstExamples.Size = new System.Drawing.Size(360, 381);
			this.lstExamples.Sorted = true;
			this.lstExamples.TabIndex = 0;
			this.lstExamples.DoubleClick += new System.EventHandler(this.startButton_Click);
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(136, 400);
			this.startButton.Name = "startButton";
			this.startButton.TabIndex = 1;
			this.startButton.Text = "Start Demo";
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// NeHe
			// 
			this.AcceptButton = this.startButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(378, 475);
			this.Controls.Add(this.startButton);
			this.Controls.Add(this.lstExamples);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "NeHe";
			this.Text = "SDL.NET - NeHe OpenGL Examples";
			this.Load += new System.EventHandler(this.NeHe_Load);
			this.Closed += new System.EventHandler(this.NeHe_Closed);
			this.ResumeLayout(false);

		}
		#endregion

		private void NeHe_Load(object sender, System.EventArgs e)
		{
			// Load app.ico as the form icon.
			Assembly asm = Assembly.GetExecutingAssembly();
			string iconName = "";
			foreach (string s in asm.GetManifestResourceNames())
			{
				if (s.EndsWith("App.ico"))
				{
					iconName = s;
					break;
				}
			}
			if (iconName.Length > 0)
			{
				this.Icon = new Icon(asm.GetManifestResourceStream(iconName));
			}

			// Get the NeHe examples.
			Type[] types = asm.GetTypes();

			foreach(Type type in types)
			{
				if(type.Name.StartsWith("NeHe") && type.Name.Length == 7) // NeHeXXX
				{
					try
					{
						// Get the title of the NeHe example class
						object result = type.InvokeMember("Title",
							BindingFlags.GetProperty, null, type, null);

						// Add the example to the array and display it on the listbox
						lstExamples.Items.Add((string)result);
						neheTypes.Add(type);
					}
					catch(System.MissingMethodException)
					{
						// NeHe demo missing static Title property - do nothing
					}
				}
			}
		}

		object dynObj;

		private void startButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				try
				{
					if (dynObj != null)
					{
						SdlDotNet.Events.QuitApplication();
						dynObj = null;
					}
				}
				catch(SdlDotNet.SdlException)
				{
					// already quit SDL - Do nothing
				}
				
				// Get the desired NeHe example type.
				Type dynClassType = (Type)neheTypes[lstExamples.SelectedIndex];

				// Make an instance of it.
				dynObj = Activator.CreateInstance(dynClassType);
				if(dynObj != null)
				{
					this.SendToBack(); // Make the SDL window appear on top of this form.
					MethodInfo invokedMethod = dynClassType.GetMethod("Run");
					invokedMethod.Invoke(dynObj, null);
				}
			}
			catch(System.Reflection.TargetInvocationException)
			{
				// User changed demo - do nothing
			}
			catch(System.ArgumentOutOfRangeException)
			{
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[STAThread]
		public static void Main()
		{
			Application.Run(new NeHe());
		}

		private void NeHe_Closed(object sender, System.EventArgs e)
		{
			// Quit SDL if it's not quit already
			try
			{
				SdlDotNet.Events.QuitApplication();
			}
			catch(SdlDotNet.SdlException)
			{
				// already quit SDL - Do nothing
			}

			// End the thread and the application.
			Application.Exit();
			//System.Threading.Thread.CurrentThread.Abort();
		}
	}
}
