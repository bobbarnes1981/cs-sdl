using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using System.Threading;

using SdlDotNet;

namespace SdlDotNet.Windows
{
	/// <summary>
	/// Summary description for SurfaceForm.
	/// </summary>
	public class SurfaceForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Surface m_Surface;
		private Image m_Image;
		private Graphics m_Graphics;

		public Surface Surface
		{
			get
			{
				return m_Surface;
			}
		}

		public SurfaceForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			try
			{
				m_Surface = new Surface(this.ClientSize);
				m_Image = m_Surface.Bitmap;
				SdlDotNet.Events.Tick+=new TickEventHandler(Events_Tick);
				SdlDotNet.Events.Fps = 30;
				m_Graphics = Graphics.FromHwnd(this.Handle);

				Thread t = new Thread(new ThreadStart(SdlDotNet.Events.Run));
				t.IsBackground = true;
				t.Name = "SDL.NET";
				t.Priority = ThreadPriority.Normal;
				t.Start();
			}
			catch
			{
			}
		}

		protected override void OnResize(EventArgs e)
		{
			try
			{
				Surface newSurface = m_Surface.CreateCompatibleSurface(this.ClientSize);
				newSurface.Blit(m_Surface);
				m_Surface = newSurface;
				this.Invalidate();
			}
			catch
			{
			}
			base.OnResize (e);
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
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Name = "SurfaceForm";
			this.Text = "SDL.NET Surface Form";
		}
		#endregion

		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				m_Graphics.DrawImage(m_Image, 0,0);
			}
			catch
			{
			}
			base.OnPaint(e);
		}


		private void Events_Tick(object sender, TickEventArgs e)
		{
			try
			{
				m_Image = m_Surface.Bitmap;
			}
			catch
			{
			}
			this.Invalidate();
		}
	}
}
